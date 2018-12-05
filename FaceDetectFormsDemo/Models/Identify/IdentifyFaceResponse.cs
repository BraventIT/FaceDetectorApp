using System.Collections.Generic;

namespace FaceDetectFormsDemo.Models
{
    public class IdentifyFaceResponse
    {
        public string FaceId { get; set; }
        public List<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        public string PersonId { get; set; }
        public double Confidence { get; set; }


    }
}
