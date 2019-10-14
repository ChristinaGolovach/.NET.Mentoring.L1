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
    class FileStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private FileInfo _fileInfo;
        private string _filePath;
        private List<T> _energyObservations;
        private ISerializer<T> _serializer;


        public string Description => "File storage of energy observations";

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => _fileInfo.IsReadOnly;

        public FileStorage(string filePath, ISerializer<T> serializer)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"The {nameof(filePath)} can not bw null or empty.");
            }

            _serializer = serializer ?? throw new ArgumentNullException($"The {nameof(serializer)} can not be null.");

            _filePath = filePath;
            _fileInfo = new FileInfo(filePath);

            //TODO _energyObservations = read from file
        }

        public void Add(T item)
        {
            //TODO add in list and file
        }

        public void Clear()
        {
            try
            {
                File.WriteAllText(_filePath, String.Empty);
                _energyObservations.Clear();
            }
            catch (Exception exception)
            {
                throw new FileStorageExcepion($"Exception occurred during clear the file {nameof(_filePath)}.", exception);
            }
        }

        public bool Contains(T item)
        {
            if (_energyObservations.Contains(item))
            {
                return true;
            }
            else
            {
                //TODO read from file (somebody can add) and check 
                return true; 
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array = array ?? throw new ArgumentNullException($"The {nameof(array)} can not be null.");

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(arrayIndex)} can not be less than 0.");
            }

            if (array.Length - arrayIndex < _energyObservations.Capacity)
            {
                throw new ArgumentException($"The available space in {nameof(array)} is not enough.");
            }

            _energyObservations.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
            //TODO remove from list and file
        }

        public IEnumerator<T> GetEnumerator() => _energyObservations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
