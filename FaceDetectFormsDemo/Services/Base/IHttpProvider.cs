using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceDetectFormsDemo.Services
{
    public interface IHttpProvider
    {

        Task<TResult> PostAsync<TResult>(string endpoint,
                                         object data,
                                         params KeyValuePair<string, string>[] headers);

        Task<TResult> PutAsync<TResult>(string endpoint,
                                        object data,
                                        params KeyValuePair<string, string>[] headers);

        Task<TResult> GetAsync<TResult>(string endpoint,
                                        params KeyValuePair<string, string>[] headers);

        Task<TResult> PatchAsync<TResult>(string endpoint,
                                        object data,
                                        params KeyValuePair<string, string>[] headers);

        Task DeleteAsync(string endpoint,
                         params KeyValuePair<string, string>[] headers);

    }
}
