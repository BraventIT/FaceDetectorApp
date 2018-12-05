using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FaceDetectFormsDemo.Models;
using FaceDetectFormsDemo.Services;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public class NewPersonViewModel: BaseViewModel
    {
        IBlobStorageService BlobStorageService { get { return DependencyService.Get<IBlobStorageService>(); } }
        List<Person> _persons;

        readonly PersonGroupResponse _personGroup;
        MediaFile _photo;

        #region Properties
        string _personName;
        public string PersonName
        {
            get { return _personName; }
            set { SetProperty(ref _personName, value); }
        }

        ImageSource _facePhoto;
        public ImageSource FacePhoto
        {
            get { return _facePhoto; }
            set { SetProperty(ref _facePhoto, value); }
        }

        public ICommand TakePhotoCommand { get; private set; }
        public ICommand AddPersonToGroupCommand { get; private set; }
        #endregion

        #region Init and constructor
        public NewPersonViewModel(PersonGroupResponse personGroup)
        {
            _personGroup = personGroup;
            TakePhotoCommand = new Command(async () => await OnTakePhoto());
            AddPersonToGroupCommand = new Command(async () => await OnAddPersonToGroup());
        }

        public override async Task InitAsync()
        {
            await ExecuteAsync(async () =>
            {
                FacePhoto = "face_default";
                _persons = await FaceApiService.GetPersons();
            });

        }
        #endregion

        #region Commands
        async Task OnTakePhoto()
        {
            await ExecuteAsync(async () =>
            {
                await CheckPermissionAndAskIfIsNeeded(Permission.Camera);
                await CheckPermissionAndAskIfIsNeeded(Permission.Storage);

                _photo = await GetFacePhotoTaked();
                if (_photo == null) return;
                var isValidPhoto = await IsValidPhoto();
                if(isValidPhoto)
                    FacePhoto = _photo.Path;

            });

        }


       
        async Task OnAddPersonToGroup()
        {
            if (!(await IsFormValid())) return;
            var wantUserContinue = await Application.Current.MainPage.DisplayAlert(
                string.Empty,
                       "Vamos a guardar tu foto y aparecerá en algunos apartados de la aplicación móvil de l@s demás usuari@s que se encuentran en la sala. ¡Sólo es para entretenernos un rato! Tendrás la opción de borrar la foto después si quieres y no quedará ni rastro de ella. Si tú no la borras, se borrará al final del día de hoy. Si aún así no te convence, pulsa Cancelar.",
                       "Ok",
                        "Cancelar");

            if (!wantUserContinue) return;


            await ExecuteAsync(async () =>
            {

                var photoBytes = GetByteArray(_photo);
                //save photo in blob storage
                var photoUrl = await BlobStorageService.SaveBlockBlob(photoBytes, PersonName);

                //add face to largeList
                var persistedFaceIdInLargeList = await FaceApiService.AddLargeListFace(photoUrl);

                //add person to group
                var person = await FaceApiService.AddPersonToGroup(new PersonGroupRequest()
                                    {
                                        Name = PersonName, 
                                        UserData = JsonConvert.SerializeObject(new UserDataPerson()
                                        { 
                                            LargeListTWPersistFaceId = persistedFaceIdInLargeList.PersistedFaceId, 
                                            UrlFaceImage = photoUrl 
                                        })

                                    }, 
                                                                   _personGroup.PersonGroupId);

                //add face to person
                var res = await FaceApiService.AddFaceToPerson(person.PersonId, _personGroup.PersonGroupId, photoBytes);

                //trainmodel
                await FaceApiService.TrainModel(_personGroup.PersonGroupId);

                await Navigation.PopAsync();
            });
        }
        #endregion

        #region Private voids
        async Task<bool> IsValidPhoto()
        {

            var facesDetected = await FaceApiService.DetectFaces(GetByteArray(_photo));

            if (facesDetected == null || facesDetected.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Foto no válida",
                    "No hemos detectado ninguna cara en la foto que has tomado. Saca una foto en la que salga tu cara.",
                    "Ok");
                return false;
            }
            else if (facesDetected.Count == 1)
            {
                var persons = await FaceApiService.GetPersons();

                foreach (var face in facesDetected)
                {
                    bool existsFaceId = persons.FirstOrDefault(person => person.PersistedFaceIds.Contains(face.FaceId)) != null;
                    if (existsFaceId)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                       "Foto no válida",
                       $"En el grupo {_personGroup.Name} ya hay una persona creada asociada a la cara a la que le has sacado la foto.",
                       "Ok");
                        return false;
                    }

                }
            }
            else
            {
                foreach (var face in facesDetected)
                {
                    await Application.Current.MainPage.DisplayAlert(
                    "Foto no válida",
                    "En la foto sólo puede haber una cara",
                    "Ok");
                    return false;
                }
            }

            return true;
        }

        async Task<bool> IsFormValid()
        {
            if (_persons != null && _persons.Count > 0 &&
               _persons.Select(p => p.Name).Contains(PersonName))
            {
                await Application.Current.MainPage.DisplayAlert(
                string.Empty,
                       "El nombre que has seleccionado ya está en uso. Por favor, complétalo con tus apellidos.",
                       "Ok",
                        "Cancelar");
                return false;
            }

            if(_photo == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                string.Empty,
                       "Añade tu foto",
                       "Ok",
                        "Cancelar");
                return false;
            }

            return true;
        }

        async Task CheckPermissionAndAskIfIsNeeded(Permission permission)
        {
            var status = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (status != PermissionStatus.Granted)
            {
                var request = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (request.ContainsKey(permission) && request[permission] != PermissionStatus.Granted)
                    return;
            }
        }

        async Task<MediaFile> GetFacePhotoTaked()
        {
            await CrossMedia.Current.Initialize();
            return await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "clasificator",
                Name = "source.png",
                PhotoSize = PhotoSize.Full
            });
        }

        Byte[] GetByteArray(MediaFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        #endregion

    }

    public class UserDataPerson
    {
        public string UrlFaceImage { get; set; }
        public string LargeListTWPersistFaceId { get; set; }
    }


}
