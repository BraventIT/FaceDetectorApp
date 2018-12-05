namespace FaceDetectFormsDemo.Constants
{
    public static class FaceApiConstants
    {
        public const string BASE_ENDPOINT_FACE_API = $"{INSERT_HERE_FACE_API_ENDPOINT}";
        public const string FACE_API_SUSCRIPTION_KEY_NAME = "Ocp-Apim-Subscription-Key";
        public const string FACE_API_SUSCRIPTION_KEY_VALUE = $"{INSER_HERE_SUBSCRIPTION_KEY}";

        //Person groups
        public static string BASE_ENPOINT_PERSON_GROUPS => $"{BASE_ENDPOINT_FACE_API}/persongroups";
        public static string CreatePersonGroup(string groupId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}";
        public static string GetPersonGroups => BASE_ENPOINT_PERSON_GROUPS;
        public static string AddPersonToGroup(string groupId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}/persons";
        public static string DeletePersonFromGroup(string groupId, string personId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}/persons/{personId}";


        public static string AddFaceToPerson(string personId, string groupId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}/persons/{personId}/persistedFaces";
        public static string TrainPersonGroup(string groupId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}/train";
        public static string GetStatusTrainingPersonGroup(string groupId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}/training";
        public static string GetPersons(string groupId) => $"{BASE_ENPOINT_PERSON_GROUPS}/{groupId}/persons";

        public static string DetectFaces => $"{BASE_ENDPOINT_FACE_API}/detect?returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
        public static string IdentifyFaces => $"{BASE_ENDPOINT_FACE_API}/identify";

        public static string FindSimilartsFaces => $"{BASE_ENDPOINT_FACE_API}/findsimilars";
        public static string LargeListGenericEndpoint(string largeFaceListId = "") => $"{BASE_ENDPOINT_FACE_API}/largefacelists/{largeFaceListId}";

        public static string LargeListAddFace(string largeFaceListId = "") => $"{BASE_ENDPOINT_FACE_API}/largefacelists/{largeFaceListId}/persistedfaces";
        public static string LargeListGetFaces(string largeFaceListId = "") => $"{BASE_ENDPOINT_FACE_API}/largefacelists/{largeFaceListId}/persistedfaces";


        public static string TrainLargeListGenericEndpoint(string largeFaceListId = "") => $"{BASE_ENDPOINT_FACE_API}/largefacelists/{largeFaceListId}/train";
        public static string GetTrainingStatusLargeListGenericEndpoint(string largeFaceListId = "") => $"{BASE_ENDPOINT_FACE_API}/largefacelists/{largeFaceListId}/training";

        public const string LARGE_LIST_NAME = "Grupo1LargeList";
        public const string GROUP_NAME = "Grupo1";
        public const string MODE_FIND_SIMILAR = "matchFace";
        public const string MODE_FIND_JUST_PERSON = "matchPerson";



    }
}
