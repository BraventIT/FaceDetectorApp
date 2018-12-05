using System.Threading.Tasks;

namespace FaceDetectFormsDemo.Services
{
    public interface IBlobStorageService
    {
        Task<string> SaveBlockBlob(byte[] blob, string blobTitle);
    }
}
