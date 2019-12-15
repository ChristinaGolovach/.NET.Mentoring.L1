using Newtonsoft.Json;
using Potestas.API.Plugin.Exceptions;
using Potestas.API.Plugin.Services;
using Potestas.Extensions;
using Potestas.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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

            var response = _httpClientService.GetAsync("api/energyobservations").Result;

            CheckResponse(response, $"Exception occurred during getting data from API storage.");

            var content = response.Content.ReadAsStringAsync().Result;

            var items = ConvertToTypedCollection(content);

            var genericCollection = items.AsEnumerable().ConvertObservationCollectionToGeneric<T, EnergyObservationAPIModel>();

            genericCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

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

        // из-за того что класс APIStorage generic то <T> м.б. интерфейсом, структурой или классом
        // и выходит если <T>
        // интерфейс - JsonConvert.DeserializeObject<IEnumerable<T>>(content) - упадет на рантайме
        // структура - и причем если неизменяемая то JsonConvert.DeserializeObject<IEnumerable<T>>(content) все поля инициализирует значениями по умолчанию
        // [JsonConstructor] для конструктра не помог(проверяла) https://github.com/JamesNK/Newtonsoft.Json/issues/1218
        // классом - а все варианты <T> должны реализовать IEnergyObservation у которого все св-ва только на чтение
        // тогда нужно использовать (T)Activator.CreateInstance(typeof(T), new object[] { item }), но опять если Т будет структурой, интерфейсом - упадет
        // (FormatterServices.GetUninitializedObject(typeParameterType); // если T интерфейс  - упадет)

        // решение (костыльное) но не знаю как по-другому:
        // использовать класс EnergyObservationAPIModel - который имплементит IEnergyObservation и при этом св-ва какна чтения так и на запись
        // и вручную парсить все св-ва
        // используя ExpandoObject и dynamic

        private List<EnergyObservationAPIModel> ConvertToTypedCollection(string content)
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<ExpandoObject>>(content) as dynamic;

            var typedCollection = new List<EnergyObservationAPIModel>();

            try
            {
                foreach (var item in items)
                {
                    // var itemMembers = item as IDictionary<string, object>;

                    var typedItem = new EnergyObservationAPIModel()
                    {
                        Id = (int)(long)(object)item.id,
                        EstimatedValue = (double)(object)item.estimatedValue,
                        ObservationTime = (DateTime)(object)item.observationTime,
                        ObservationPoint = ConvertToTypedValue(item.observationPoint)
                    };

                    typedCollection.Add(typedItem);
                }
            }
            catch (Exception ex)
            {
                throw new APIStorageException("Can not convert dynamic collection to EnergyObservationAPIModel collection", ex);
            }

            return typedCollection;
        }

        private Coordinates ConvertToTypedValue(dynamic item)
        {
            int id = (int)(long)(object)item.id;
            double x = (double)(object)item.x;
            double y = (double)(object)item.y;

            return new Coordinates(id, x, y);
        }
    }

    internal class EnergyObservationAPIModel : IEnergyObservation
    {
        public int Id { get; set; }
        public Coordinates ObservationPoint { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ObservationTime { get; set; }
    }
}
