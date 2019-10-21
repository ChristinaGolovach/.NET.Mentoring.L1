using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Potestas.Exceptions;
using Potestas.Serializers;

namespace Potestas.Storages
{
    /* TASK. Implement file storage
     */
    public class FileStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private FileInfo _fileInfo;
        private string _filePath;
        private ISerializer<T> _serializer;       

        public string Description => "File storage of energy observations";

        public int Count => ReadFromStorage().Count;

        public bool IsReadOnly => _fileInfo.IsReadOnly;

        public FileStorage(string filePath, ISerializer<T> serializer)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"The {nameof(filePath)} can not bw null or empty.");
            }

            _serializer = serializer ?? throw new ArgumentNullException($"The {nameof(serializer)} can not be null.");

            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }

            _filePath = filePath;
            _fileInfo = new FileInfo(filePath);
        }

        public void Add(T item)
        {
            try
            {
                SaveToStorage(item);
            }
            catch (Exception exception)
            {
                throw new FileStorageExcepion($"Exception occurred during add item {item} in file.", exception);
            }
        }

        public void Clear()
        {
            try
            {
                File.WriteAllText(_filePath, String.Empty);
            }
            catch (Exception exception)
            {
                throw new FileStorageExcepion($"Exception occurred during clear the file {nameof(_filePath)}.", exception);
            }
        }

        public bool Contains(T item)
        {
            try
            {
                var energyObservations = ReadFromStorage();
                return energyObservations.Contains(item);
            }
            catch
            {
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array = array ?? throw new ArgumentNullException($"The {nameof(array)} can not be null.");

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(arrayIndex)} can not be less than 0.");
            }

            if (array.Length - arrayIndex < ReadFromStorage().Capacity)
            {
                throw new ArgumentException($"The available space in {nameof(array)} is not enough.");
            }

            try
            {
                ReadFromStorage().CopyTo(array, arrayIndex);
            }
            catch (Exception exception)
            {
                throw new FileStorageExcepion($"Exception occurred during copy data from the file to array.", exception);
            }
        }

        public bool Remove(T item)
        {
            try
            {
                var actualObservations = ReadFromStorage();
                var isRemoved = actualObservations.Remove(item);

                if (isRemoved)
                {
                    Clear();
                    actualObservations.ForEach(observation => SaveToStorage(observation));

                    return true;
                }

                return false;
            }
            catch
            {
                return false;               
            }
        }

        public IEnumerator<T> GetEnumerator() => ReadFromStorage().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private List<T> ReadFromStorage()
        {
            using (var stream = new FileStream(_filePath, FileMode.Open)) // == File.OpenRead()
            {
                return new List<T>(_serializer.Deserialize(stream)); 
            }
        }

        private void SaveToStorage(T item)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append))
            using (var streamWriter = new StreamWriter(stream))
            {
                _serializer.Serialize(stream, item);
                streamWriter.Write(Environment.NewLine);
            }
        }
    }
}
