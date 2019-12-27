using Potestas.API.Plugin.Exceptions;
using Potestas.API.Plugin.Services;
using Potestas.API.Plugin.Utils;
using Potestas.Extensions;
using Potestas.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Potestas.API.Plugin.Storages
{
    public class APIStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
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
            GenericValidator.CheckInitialization(item, nameof(item));

            var response = _httpClientService.PostAsync<T>("api/energyobservations", item).Result;

            CheckResponse(response, $"Exception occurred during add {item} to API storage.");
        }

        public void Clear()
        {
            var response = _httpClientService.DeleteAsync<T>("api/energyobservations/clearing").Result;

            CheckResponse(response, $"Exception occurred during clearing API storage.");
        }

        public bool Contains(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            var response = _httpClientService.PostAsync<T>("api/energyobservations/checking/existence", item).Result;

            CheckResponse(response, $"Exception occurred during clearing API storage.");

            return response.Content.ReadAsAsync<bool>().Result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ValidateArray(array, arrayIndex);

            GetAll().CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() => GetAll().GetEnumerator();

        public bool Remove(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            var response = _httpClientService.DeleteAsync<T>("api/energyobservations", item).Result;

            CheckResponse(response, $"Exception occurred during delete {item} from API storage.");

            return response.Content.ReadAsAsync<bool>().Result;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int GetCount()
        {
            var response = _httpClientService.GetAsync("api/energyobservations/count").Result;

            CheckResponse(response, $"Exception occurred during get count of items in API storage.");

            return response.Content.ReadAsAsync<int>().Result;
        }

        private List<T> GetAll()
        {
            var response = _httpClientService.GetAsync("api/energyobservations").Result;

            CheckResponse(response, $"Exception occurred during getting data from API storage.");

            var content = response.Content.ReadAsStringAsync().Result;

            var items = Converter.ConvertToTypedCollection(content);

            return items.AsEnumerable().ConvertObservationCollectionToGeneric<T, EnergyObservationAPIModel>();
        }

        private void CheckResponse(HttpResponseMessage responseMessage, string message)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new APIStorageException(message);
            }
        }

        private void ValidateArray(T[] array, int arrayIndex)
        {
            array = array ?? throw new ArgumentNullException($"The {nameof(array)} can not be null.");

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(arrayIndex)} can not be less than 0.");
            }

            if (array.Length - arrayIndex < GetCount())
            {
                throw new ArgumentException($"The available space in {nameof(array)} is not enough.");
            }
        }
    }
}
