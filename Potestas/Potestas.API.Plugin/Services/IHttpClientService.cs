using System.Net.Http;
using System.Threading.Tasks;

namespace Potestas.API.Plugin.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> GetAsync<T>(string url, params T[] arguments);

        Task<HttpResponseMessage> PostAsync<T>(string url, T argument);

        Task<HttpResponseMessage> DeleteAsync<T>(string url);

        Task<HttpResponseMessage> DeleteAsync<T>(string url, params T[] arguments);
    }
}
