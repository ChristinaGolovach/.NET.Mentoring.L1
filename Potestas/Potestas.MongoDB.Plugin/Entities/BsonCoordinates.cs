using MongoDB.Bson.Serialization.Attributes;

namespace Potestas.MongoDB.Plugin.Entities
{
    public class BsonCoordinates
    {
        [BsonElement("x")]
        public double X { get; set; }

        [BsonElement("y")]
        public double Y { get; set; }
    }
}
