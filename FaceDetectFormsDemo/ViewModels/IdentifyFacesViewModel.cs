using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FaceDetectFormsDemo.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public class IdentifyFacesViewModel : BaseViewModel
    {
        PersonGroupResponse _personGroup;
        MediaFile _photo;

        #region Properties
        ImageSource _photoTaked;
        public ImageSource PhotoTaked
        {
            get { return _photoTaked; }
            set { SetProperty(ref _photoTaked, value); }
        }

        List<FaceDetectedItemViewModel> _facesDetected;
        public List<FaceDetectedItemViewModel> FacesDetected
        {
            get { return _facesDetected; }
            set { SetProperty(ref _facesDetected, value); }
        }

        public ICommand AddImageCommnad { get; private set; }
        #endregion

        #region Init and constructor
        public IdentifyFacesViewModel(PersonGroupResponse personGroup)
        {
            _personGroup = personGroup;
            AddImageCommnad = new Command(async () => await OnAddImage());
        }
        #endregion

        #region Commands
        async Task OnAddImage()
        {
            await ExecuteAsync(async () =>
            {
                await CheckPermissionAndAskIfIsNeeded(Permission.Camera);
                await CheckPermissionAndAskIfIsNeeded(Permission.Storage);

                _photo = await GetFacePhotoTaked();
                if (_photo == null) return;
                PhotoTaked = _photo.Path;
                byte[] byteArray = GetByteArray(_photo);

                var detectResults = await FaceApiService.DetectFaces(byteArray);

                if (!(await IsPhotoValid(detectResults))) return;

                var identifyResults = await FaceApiService.Identify(new IdentifyFaceRequest()
                {
                    FaceIds = detectResults.Select(x => x.FaceId).ToList(),
                    MaxNumOfCandidatesReturned = 10,
                    PersonGroupId = _personGroup.PersonGroupId,
                    ConfidenceThreshold = 0.5
                });

                var persons = await FaceApiService.GetPersons();

                if (detectResults == null || identifyResults == null || persons == null)
                    return;

                FacesDetected = identifyResults.Select(res => new FaceDetectedItemViewModel()
                {
                    FaceId = res.FaceId,
                    FaceRectangle =  detectResults.FirstOrDefault(detResults => detResults.FaceId == res.FaceId).FaceRectangle,
                    FaceAttributes = detectResults.FirstOrDefault(detResults => detResults.FaceId == res.FaceId).FaceAttributes,
                    PersonName = persons?.FirstOrDefault(p => p.PersonId == res.Candidates?.FirstOrDefault(cand => cand.PersonId == p.PersonId)?.PersonId)?.Name
                }).ToList();


            });
        }
        #endregion

        #region Private voids
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

        async Task<bool> IsPhotoValid(List<DetectFaceResponse> detectResults)
        {
            if (detectResults == null || detectResults.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                "Foto no válida",
                "No hemos detectado ninguna cara en la foto que has tomado. Saca una foto en la que salga tu cara.",
                "Ok");
                return false;
            }

            return true;
        }
        #endregion
    }

    public class FaceDetectedItemViewModel
    {

        public string FaceId { get; set; }

        public FaceRectangle FaceRectangle { get; set; }

        public FaceAttributes FaceAttributes { get; set; }

        public Candidate Candidate { get; set; }

        public string PersonName { get; set; }
    }
}
