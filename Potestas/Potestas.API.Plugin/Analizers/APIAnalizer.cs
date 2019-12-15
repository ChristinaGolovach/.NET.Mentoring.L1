using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using Potestas.API.Plugin.Exceptions;
using Potestas.API.Plugin.Services;
using Potestas.API.Plugin.Utils;

namespace Potestas.API.Plugin.Analizers
{
    public class APIAnalizer : IEnergyObservationAnalizer
    {
        private readonly IHttpClientService _httpClientService;

        public APIAnalizer(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException($"The {nameof(httpClientService)} can not be null.");
        }
        public double GetAverageEnergy()
        {
            var response = Get("api/researches/general/averageEnergy", "get AverageEnergy from");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var response = Get($"api/researches/byDates/averageEnergy?startFrom={startFrom}&endBy={endBy}", "get AverageEnergy by date from");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var array = new Coordinates[] { rectTopLeft, rectBottomRight };

            var response = _httpClientService.PostAsync($"api/researches/byCoordinates/averageEnergy", array).Result;

            CheckResponse(response, $"Exception occurred during get AverageEnergy by coordinates from API storage.");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            var response = Get("api/researches/byCoordinates/distribution", "get DistributionCoordinates from");

            var content = response.Content.ReadAsStringAsync().Result;

            //var coordinate = JsonConvert.DeserializeObject<IDictionary<ExpandoObject, int>>(content) as dynamic;

            throw new NotImplementedException(); // ask - контроллер что-то странное возвращает

        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            var response = Get("api/researches/byEnergy/distribution", "get DistributionByEnergy from");

            return response.Content.ReadAsAsync<IDictionary<double, int>>().Result;
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            var response = Get("api/researches/byTime/distribution", "get DistributionByTimey from");

            return response.Content.ReadAsAsync<IDictionary<DateTime, int>>().Result;
        }

        public double GetMaxEnergy()
        {
            var response = Get("api/researches/general/maxEnergy", "get MaxEnergy from");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            var response = _httpClientService.PostAsync($"api/researches/byCoordinates/maxEnergy", coordinates).Result;

            CheckResponse(response, $"Exception occurred during get MaxEnergy by coordinates from API storage.");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            var response = Get($"api/researches/byDate/maxEnergy?dateTime={dateTime}", "get MaxEnergy by date from");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var response = Get("api/researches/general/maxEnergyPosition", "get MaxEnergyPosition from");

            var content  = response.Content.ReadAsStringAsync().Result;

            var coordinate = JsonConvert.DeserializeObject<ExpandoObject>(content) as dynamic;

            return Converter.ConvertToTypedValue(coordinate);
        }

        public DateTime GetMaxEnergyTime()
        {
            var response = Get("api/researches/general/maxEnergyTime", "get MaxEnergyTime from");

            return response.Content.ReadAsAsync<DateTime>().Result;
        }

        public double GetMinEnergy()
        {
            var response = Get("api/researches/general/minEnergy", "get MinEnergy from");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            var response = _httpClientService.PostAsync($"api/researches/byCoordinates/minEnergy", coordinates).Result;

            CheckResponse(response, $"Exception occurred during get MinEnergy by coordinates from API storage.");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            var response = Get($"api/researches/byDate/minEnergy?dateTime={dateTime}", "get MinEnergy by date from");

            return response.Content.ReadAsAsync<double>().Result;
        }

        public Coordinates GetMinEnergyPosition()
        {
            var response = Get("api/researches/general/minEnergyPosition", "get MinEnergyPosition from");

            var content = response.Content.ReadAsStringAsync().Result;

            var coordinate = JsonConvert.DeserializeObject<ExpandoObject>(content) as dynamic;

            return Converter.ConvertToTypedValue(coordinate);
        }

        public DateTime GetMinEnergyTime()
        {
            var response = Get("api/researches/general/minEnergyTime", "get MinEnergyTime from");

            return response.Content.ReadAsAsync<DateTime>().Result;
        }

        private void CheckResponse(HttpResponseMessage responseMessage, string message)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new APIAnalizerException(message);
            }
        }

        private HttpResponseMessage Get(string url, string exceptionMessage)
        {
            var response = _httpClientService.GetAsync(url).Result;

            CheckResponse(response, $"Exception occurred during {exceptionMessage} API storage.");

            return response;
        }
    }
}
