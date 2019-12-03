using Potestas.MongoDB.Plugin.Entities;

namespace Potestas.MongoDB.Plugin.Mappers
{
    public static class CoordinatesMapper
    {
        public static BsonCoordinates ToBsonEntity(this Coordinates coordinates)
        {
            return new BsonCoordinates
            {
                X = coordinates.X,
                Y = coordinates.Y
            };
        }

        public static Coordinates ToDomainEntity(this BsonCoordinates coordinates)
        {
            return new Coordinates(coordinates.X, coordinates.Y);
        }
    }
}
