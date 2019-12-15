using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Potestas.API.Plugin.Services.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService()
        {
            _client = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
             => await _client.GetAsync(string.Intern(Settings.BaseAddress + url));

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
            => await _client.PostAsJsonAsync(string.Intern(Settings.BaseAddress + url), item);

        public async Task<HttpResponseMessage> DeleteAsync<T>(string url)
            => await _client.DeleteAsync(string.Intern(Settings.BaseAddress + url));

        public async Task<HttpResponseMessage> DeleteAsync<T>(string url, T item)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Intern(Settings.BaseAddress + url));
            request.Content = new StringContent(JsonConvert.SerializeObject(item), Settings.EncodingType, Settings.MediaType);

            return await _client.SendAsync(request);
        }

        //ASK не получалось псылать запросы  в API
        // ошибка - The remote certificate is invalid according to the validation procedure
        // закоментила в API строку - app.UseHttpsRedirection(); и sslPort выставила в 0 в launchSettings.json т.к код ниже не помог        
        ////https://basquang.wordpress.com/2017/04/27/calling-untrusted-ssl-https-request-using-httpclient-give-messagethe-remote-certificate-is-invalid-according-to-the-validation-procedure/
        //https://github.com/dotnet/corefx/issues/32976
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/working-with-ssl-in-web-api
        // ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    return true;
        //};


        //HttpClient
        //https://medium.com/cheranga/calling-web-apis-using-typed-httpclients-net-core-20d3d5ce980
        //http://zetcode.com/csharp/httpclient/
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    }
}
