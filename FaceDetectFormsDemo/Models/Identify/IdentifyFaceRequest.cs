using System.Collections.Generic;

namespace FaceDetectFormsDemo.Models
{
    public class IdentifyFaceRequest
    {
        public string PersonGroupId { get; set; }
        public int MaxNumOfCandidatesReturned { get; set; }
        public List<string> FaceIds { get; set; }
        public double ConfidenceThreshold { get; set; }

    }
}
