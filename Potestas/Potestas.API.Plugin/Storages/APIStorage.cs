using System;
using System.Collections;
using System.Collections.Generic;

namespace Potestas.API.Plugin.Storages
{
    public class APIStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        public string Description => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    //https://medium.com/cheranga/calling-web-apis-using-typed-httpclients-net-core-20d3d5ce980
    //http://zetcode.com/csharp/httpclient/
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
}
