﻿
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Potestas.Exceptions.SerializerExceptions;

namespace Potestas.Serializers
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public void Serialize(Stream stream, T value)
        {
            stream = stream ?? throw new ArgumentNullException($"The {nameof(stream)} can not be null.");

            string jsonValue = "";

            try
            {
                jsonValue = JsonConvert.SerializeObject(value);
            }
            catch (JsonException jsonException)
            {
                throw new JsonSerializerException($"Can not serialize value: {value} to the Json format.", jsonException);
            }
            catch (Exception exception)
            {
                throw new JsonSerializerException($"Some exception occurred during serializing value: {value}.", exception);
            }

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(jsonValue);
                writer.Flush();
                memoryStream.WriteTo(stream);
            }
        }

        public IList<T> Deserialize(Stream stream)
        {
            stream = stream ?? throw new ArgumentNullException($"The {nameof(stream)} can not be null.");

            var serializer = new JsonSerializer();
            var resultValues = new List<T>();
            var memoryStream = new MemoryStream();

            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(memoryStream);

            using (memoryStream)
            using (var streamReader = new StreamReader(memoryStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                jsonReader.SupportMultipleContent = true;
                while (jsonReader.Read())
                {
                    resultValues.Add(serializer.Deserialize<T>(jsonReader)); //TODO try-catch
                }
            }

            return resultValues;
        }
    }
}
