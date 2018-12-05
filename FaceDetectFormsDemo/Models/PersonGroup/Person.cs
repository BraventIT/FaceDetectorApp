using System.Collections.Generic;
using Newtonsoft.Json;

namespace FaceDetectFormsDemo.Models
{
    public class Person : CreatePersonResponse
    {
        public string Name { get; set; }
        public string UserData { get; set; }
        public List<string> PersistedFaceIds { get; set; }

        [JsonIgnore]
        public string FaceImageUrl { get; set; }

        [JsonIgnore]
        public string LargeListFaceId { get; set; }

        [JsonIgnore]
        public string ConfidenceSimilarity { get; set; }
    }
}
