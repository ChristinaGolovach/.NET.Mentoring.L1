using System.IO;
using System.Collections.Generic;

namespace Potestas.Serializers
{
    public interface ISerializer<T>
    {
        void Serialize(Stream stream, T item);
        IList<T> Deserialize(Stream stream);
    }
}
