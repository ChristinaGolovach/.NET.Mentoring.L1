using System.Net.Http;
using System.Threading.Tasks;

namespace Potestas.API.Plugin.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> PostAsync<T>(string url, T item);

        Task<HttpResponseMessage> DeleteAsync<T>(string url);

        Task<HttpResponseMessage> DeleteAsync<T>(string url, T item);
    }
}
