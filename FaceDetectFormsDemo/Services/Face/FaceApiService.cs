using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceDetectFormsDemo.Constants;
using FaceDetectFormsDemo.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace FaceDetectFormsDemo.Services
{
    public class FaceApiService : IFaceApiService
    {
        IHttpProvider HttpProvider { get { return DependencyService.Get<IHttpProvider>(); } }

        readonly KeyValuePair<string, string> _headers = new KeyValuePair<string, string>(
                    FaceApiConstants.FACE_API_SUSCRIPTION_KEY_NAME,
            FaceApiConstants.FACE_API_SUSCRIPTION_KEY_VALUE);



        #region Groups
        public async Task<PersonGroupResponse> GetMyGroup()
        {
            var groups = await GetGroups();

            if ((groups == null || groups.Count == 0) || groups.FirstOrDefault(g => g.Name == FaceApiConstants.GROUP_NAME) == null)
            {
                await CreatePersonGroup(Guid.NewGuid().ToString(), new PersonGroupRequest() { Name = FaceApiConstants.GROUP_NAME });
                groups = await GetGroups();
            }

            return groups.FirstOrDefault(g => g.Name == FaceApiConstants.GROUP_NAME);
        }

        async Task<List<PersonGroupResponse>> GetGroups()
        {

            return await HttpProvider.GetAsync<List<PersonGroupResponse>>(
                FaceApiConstants.GetPersonGroups,
                _headers);
        }

        async Task CreatePersonGroup(string groupId, PersonGroupRequest request)
        {
            var res = await HttpProvider.PutAsync<EmptyClass>(
                FaceApiConstants.CreatePersonGroup(groupId),
                request,
                _headers);
        }

        public async Task TrainModel(string groupId)
        {
            var status = await GetTrainingStatus(groupId);
            while (status?.Status == "Running")
            {
                status = await GetTrainingStatus(groupId);
            }
            await HttpProvider.PostAsync<EmptyClass>(
                FaceApiConstants.TrainPersonGroup(groupId),
                null,
                _headers);
        }

        public async Task<TrainingStatus> GetTrainingStatus(string groupId)
        {
            return await HttpProvider.GetAsync<TrainingStatus>(
                FaceApiConstants.GetStatusTrainingPersonGroup(groupId),
                _headers);
        }
        #endregion

        #region Persons
        public async Task<List<Person>> GetPersons()
        {
            var group = await GetMyGroup();
            var persons = await HttpProvider.GetAsync<List<Person>>(
                FaceApiConstants.GetPersons(group.PersonGroupId),
                _headers);

            foreach (var person in persons)
            {
                var userData = person.UserData != null ? JsonConvert.DeserializeObject<UserDataPerson>(person.UserData) : null;
                person.FaceImageUrl = userData?.UrlFaceImage;
                person.LargeListFaceId = userData?.LargeListTWPersistFaceId;
            }

            return persons?.OrderBy(p => p.Name)?.ToList();
        }


        public async Task<CreatePersonResponse> AddPersonToGroup(PersonGroupRequest request, string groupId)
        {
            return await HttpProvider.PostAsync<CreatePersonResponse>(
                FaceApiConstants.AddPersonToGroup(groupId),
                request,
                _headers);
        }

        public async Task DeletePersonFromGroup(string groupId, string personId)
        {
            await HttpProvider.DeleteAsync(
                FaceApiConstants.DeletePersonFromGroup(groupId, personId),
                _headers);
        }

        public async Task<FaceResponse> AddFaceToPerson(string personId, string groupId, byte[] imageData)
        {

            return await HttpProvider.PostAsync<FaceResponse>(
                FaceApiConstants.AddFaceToPerson(personId, groupId),
                imageData,
                _headers);

        }
        #endregion

        #region Detect
        public async Task<List<DetectFaceResponse>> DetectFaces(byte[] imageData)
        {
            return await HttpProvider.PostAsync<List<DetectFaceResponse>>(
                FaceApiConstants.DetectFaces,
                imageData,
                _headers);
        }

        public async Task<List<DetectFaceResponse>> DetectFaces(string url)
        {
            return await HttpProvider.PostAsync<List<DetectFaceResponse>>(
                FaceApiConstants.DetectFaces,
                new { Url = url },
                _headers);
        }
        #endregion

        #region Identify
        public async Task<List<IdentifyFaceResponse>> Identify(IdentifyFaceRequest request)
        {
            return await HttpProvider.PostAsync<List<IdentifyFaceResponse>>(
                FaceApiConstants.IdentifyFaces,
                request,
                _headers);
        }

        public async Task<List<IdentifyFaceResponse>> Identify(string url)
        {
            return await HttpProvider.PostAsync<List<IdentifyFaceResponse>>(
                FaceApiConstants.IdentifyFaces,
                new { Url = url },
                _headers);
        }
        #endregion

        #region Similar faces
        public async Task<List<FindSimilarFaceResponse>> GetSimilarFaces(string faceId)
        {
            var request = new FindSimilarFaceRequest()
            {
                FaceId = faceId,
                LargeFaceListId = (await GetTalentWomanLargeList()).LargeFaceListId,
                MaxNumOfCandidatesReturned = 10,
                Mode = FaceApiConstants.MODE_FIND_SIMILAR
            };
            var similarFaces = await HttpProvider.PostAsync<List<FindSimilarFaceResponse>>(
                FaceApiConstants.FindSimilartsFaces,
                request,
                _headers);

            return similarFaces;
        }
        #endregion

        #region Large List
        public async Task CreateLargeListFacesToFindSimilarFaces(CreateLargeListRequest request)
        {
            await HttpProvider.PutAsync<EmptyClass>(
                FaceApiConstants.LargeListGenericEndpoint(Guid.NewGuid().ToString()),
                request,
                _headers);
        }

        async Task<List<FaceResponse>> GetLargeListFacesToFindSimilarFaces(string largeListFaceId)
        {
            return await HttpProvider.GetAsync<List<FaceResponse>>(
                FaceApiConstants.LargeListGetFaces(largeListFaceId),
                _headers);
        }

        public async Task<List<GetLargeListFaceResponse>> GetLargeListFaces()
        {
            return await HttpProvider.GetAsync<List<GetLargeListFaceResponse>>(
                FaceApiConstants.LargeListGenericEndpoint(),
                _headers);
        }

        public async Task<AddLargeListFaceResponse> AddLargeListFace(string url)
        {
            var talentWomanList = await GetTalentWomanLargeList();

            if (talentWomanList == null)
            {
                await CreateLargeListFacesToFindSimilarFaces(new CreateLargeListRequest() { Name = FaceApiConstants.LARGE_LIST_NAME });
                talentWomanList = await GetTalentWomanLargeList();
            }

            var res = await HttpProvider.PostAsync<AddLargeListFaceResponse>(
                FaceApiConstants.LargeListAddFace(talentWomanList.LargeFaceListId),
                new { Url = url },
                _headers);

            await TrainLargeList();

            return res;
        }

        async Task<GetLargeListFaceResponse> GetTalentWomanLargeList()
        {
            var allLists = await GetLargeListFaces();
            return allLists.FirstOrDefault(list => list.Name == FaceApiConstants.LARGE_LIST_NAME);

        }

        async Task TrainLargeList()
        {
            var largeListFaces = await GetTalentWomanLargeList();

            var status = await GetLargeListTrainingStatus(largeListFaces.LargeFaceListId);
            while (status?.Status == "Running")
            {
                status = await GetLargeListTrainingStatus(largeListFaces.LargeFaceListId);
            }

            await HttpProvider.PostAsync<EmptyClass>(
                FaceApiConstants.TrainLargeListGenericEndpoint(largeListFaces.LargeFaceListId),
                null,
                _headers);
        }

        public async Task<TrainingStatus> GetLargeListTrainingStatus(string groupId)
        {
            return await HttpProvider.GetAsync<TrainingStatus>(
                FaceApiConstants.GetTrainingStatusLargeListGenericEndpoint(groupId),
                _headers);
        }
        #endregion
    }
}
