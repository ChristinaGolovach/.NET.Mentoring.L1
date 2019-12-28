using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using System;
using Potestas.Utils;

namespace Potestas.API.Plugin.Services.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;
        private readonly ILoggerManager _loggerManager;

        public HttpClientService(ILoggerManager loggerManager)
        {
            _client = new HttpClient();
            _loggerManager = loggerManager ?? throw new ArgumentNullException();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var resultUrl = string.Intern(Settings.BaseAddress + url);

            return await ExecuteCallWithRetryAsync((value) => _client.GetAsync(value), resultUrl, resultUrl, 2);
        }          

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var resultUrl = string.Intern(Settings.BaseAddress + url);

            return await ExecuteCallWithRetryAsync((value) => _client.PostAsJsonAsync(value, item), resultUrl, resultUrl, 2);
        }

        public async Task<HttpResponseMessage> DeleteAsync<T>(string url)
        {
            var resultUrl = string.Intern(Settings.BaseAddress + url);

            return await ExecuteCallWithRetryAsync((value) => _client.DeleteAsync(value), resultUrl, resultUrl, 2);
        }

        public async Task<HttpResponseMessage> DeleteAsync<T>(string url, T item)
        {
            var resultUrl = string.Intern(Settings.BaseAddress + url);
            var request = new HttpRequestMessage(HttpMethod.Delete, resultUrl);
            request.Content = new StringContent(JsonConvert.SerializeObject(item), Settings.EncodingType, Settings.MediaType);

            return await ExecuteCallWithRetryAsync((value) => _client.SendAsync(value), request, resultUrl, 2);
        }

        private async Task<HttpResponseMessage> ExecuteCallWithRetryAsync<T>(Func<T, Task<HttpResponseMessage>> func, T value, string url, int retryCount)
        {
            return await Policy.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(retryCount, i => TimeSpan.FromSeconds(3), (result, count, context) =>
                {
                    _loggerManager.LogWarn($"Request: {url} failed with {result.Result.StatusCode}. Retry attempt {count}.");
                })
                .ExecuteAsync(() => func(value));
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
