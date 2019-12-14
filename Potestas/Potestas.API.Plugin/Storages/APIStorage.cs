using Newtonsoft.Json;
using Potestas.API.Plugin.Exceptions;
using Potestas.API.Plugin.Services;
using Potestas.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Potestas.API.Plugin.Storages
{
    public class APIStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        static HttpClient client = new HttpClient();

        private readonly IHttpClientService _httpClientService;

        public string Description => "API storage of energy observations.";

        public int Count => GetCount();

        public bool IsReadOnly => false;

        public APIStorage(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException($"The {nameof(httpClientService)} can not be null.");
        }

        public void Add(T item)
        {
            var response = _httpClientService.PostAsync<T>(" api/energyobservations", item).Result;

            CheckResponse(response, $"Exception occurred during add {item} to API storage.");

            //TODO DELETE IT
            //HttpResponseMessage response = client.PostAsJsonAsync("http://localhost:53469/api/energyobservations", item).Result;
            //var result = response.IsSuccessStatusCode;
        }

        public void Clear()
        {
            //client.BaseAddress = new Uri("http://localhost:5001/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.DeleteAsync("http://localhost:5000/api/energyobservations").Result;

            var result = response.IsSuccessStatusCode;
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {

            //Type typeParameterType = typeof(T);
            //var res = Activator.CreateInstance(typeParameterType);
            //var res = FormatterServices.GetUninitializedObject(typeParameterType); // если T интерфейс  - упадет
            //var IResult = (T)res;

            var response = client.GetAsync("http://localhost:5000/api/energyobservations").Result;

            if (response.IsSuccessStatusCode)
            {

            }

            var content = response.Content.ReadAsStringAsync().Result;



            //var result = JsonConvert.DeserializeObject<IEnumerable<T>>(content); когда T - интерфейс - упадет
            //var result = JsonConvert.DeserializeObject<IEnumerable<Test>>(content);//.ConvertObservationCollectionToGeneric<T, Test>();
            var result = JsonConvert.DeserializeObject<IEnumerable<ExpandoObject>>(content) as dynamic;
            foreach (var item in result)
            {
                // var newItem = (T)Activator.CreateInstance(typeof(T), new object[] { item }); //если Т будет структурой, интерфейсом - упадет
                var t = item.observationPoint;
                var id = t.id;
                var x = t.x;
                var y = t.y;
            }

        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            //https://basquang.wordpress.com/2017/04/27/calling-untrusted-ssl-https-request-using-httpclient-give-messagethe-remote-certificate-is-invalid-according-to-the-validation-procedure/
            //https://github.com/dotnet/corefx/issues/32976
            //https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/working-with-ssl-in-web-api
            // ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:5000/api/energyobservations");
            request.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = client.SendAsync(request).Result;

            return response.IsSuccessStatusCode;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private int GetCount()
        {
            var response = client.GetAsync("http://localhost:5000/api/energyobservations/count").Result;

            if (response.IsSuccessStatusCode)
            {

            }

            var content = response.Content.ReadAsStringAsync().Result;

            return  JsonConvert.DeserializeObject<int>(content);
        }

        private void CheckResponse(HttpResponseMessage responseMessage, string message)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new APIStorageException(message);
            }
        }

    }

    //https://medium.com/cheranga/calling-web-apis-using-typed-httpclients-net-core-20d3d5ce980
    //http://zetcode.com/csharp/httpclient/
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client

    internal class Test : IEnergyObservation
    {
        public int Id { get ; set ; }
        public Coordinates ObservationPoint { get ; set; }
        public double EstimatedValue { get ; set ; }
        public DateTime ObservationTime { get ; set ; }
    }
}
