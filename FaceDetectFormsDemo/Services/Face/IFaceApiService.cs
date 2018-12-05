using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FaceDetectFormsDemo.Models;

namespace FaceDetectFormsDemo.Services
{
    public interface IFaceApiService
    {
        Task<PersonGroupResponse> GetMyGroup();

        Task<List<Person>> GetPersons();

        Task<CreatePersonResponse> AddPersonToGroup(PersonGroupRequest request, string groupId);

        Task DeletePersonFromGroup(string groupId, string personId);

        Task<FaceResponse> AddFaceToPerson(string personId, string groupId, byte[] imageData);

        Task TrainModel(string groupId);

        Task<List<DetectFaceResponse>> DetectFaces(byte[] imageData);

        Task<List<DetectFaceResponse>> DetectFaces(string url);

        Task<List<IdentifyFaceResponse>> Identify(IdentifyFaceRequest request);

        Task<List<IdentifyFaceResponse>> Identify(string url);

        Task<List<FindSimilarFaceResponse>> GetSimilarFaces(string faceId);

        Task<AddLargeListFaceResponse> AddLargeListFace(string url);
    }
}
