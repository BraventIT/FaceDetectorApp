namespace FaceDetectFormsDemo.Models
{
    public class FindSimilarFaceRequest
    {

        public string FaceId { get; set; }
        public string Mode { get; set; }
        public int MaxNumOfCandidatesReturned { get; set; }
        public string LargeFaceListId { get; set; }

    }
}
