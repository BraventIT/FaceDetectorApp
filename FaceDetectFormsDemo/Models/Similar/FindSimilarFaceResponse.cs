using Newtonsoft.Json;

namespace FaceDetectFormsDemo.Models
{
    public class FindSimilarFaceResponse
    {
        public string PersistedFaceId { get; set; }
        public double Confidence { get; set; }

        [JsonIgnore]
        public string FacePersonName { get; set; }

        [JsonIgnore]
        public string FaceImageUrl { get; set; }
    }
}
