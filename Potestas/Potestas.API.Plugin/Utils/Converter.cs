using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Potestas.API.Plugin.Exceptions;

namespace Potestas.API.Plugin.Utils
{
    internal static class Converter
    {
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

        public static List<EnergyObservationAPIModel> ConvertToTypedCollection(string content)
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

        public static Coordinates ConvertToTypedValue(dynamic item)
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
