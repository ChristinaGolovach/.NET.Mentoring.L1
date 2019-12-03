using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Potestas.MongoDB.Plugin.Entities
{
    public class BsonEnergyObservation : IEnergyObservation
    {
        //[BsonRepresentation(BsonType.ObjectId)]
        //[BsonElement("_id")]
        //[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        //public ObjectId ID { get; set; }

        [BsonId]
        public int Id { get; set; }

        [BsonElement("observationPoint")]
        public BsonCoordinates ObservationPoint { get; set; }

        [BsonElement("estimatedValue")]
        public double EstimatedValue { get; set; }

        [BsonElement("observationTime")]
        public DateTime ObservationTime { get; set; }

        [BsonIgnore]
        Coordinates IEnergyObservation.ObservationPoint => new Coordinates(ObservationPoint.X, ObservationPoint.Y);
    }
}
