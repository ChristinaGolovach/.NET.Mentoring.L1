using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Potestas.MongoDB.Plugin.Entities
{
    public class BsonEnergyObservation
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("observationPoint")]
        public BsonCoordinates ObservationPoint { get; set; }

        [BsonElement("estimatedValue")]
        public double EstimatedValue { get; set; }

        [BsonElement("observationTime")]
        public DateTime ObservationTime { get; set; }
    }
}
