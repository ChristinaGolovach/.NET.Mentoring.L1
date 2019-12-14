using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Potestas.API.Plugin.Services.Implementations
{
    internal class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService()
        {
            _client = new HttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetAsync<T>(string url, params T[] arguments)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T argument)
            => await _client.PostAsJsonAsync(url, argument);

        public Task<HttpResponseMessage> DeleteAsync<T>(string url)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteAsync<T>(string url, params T[] arguments)
        {
            throw new NotImplementedException();
        }

    }
}
