namespace Potestas.ORM.Plugin.Mappers
{
    internal static class CoordinatesMapper
    {
        public static Coordinates ToDomainEntity(this Models.Coordinates coordinates)
        {
            return new Coordinates(coordinates.Id, coordinates.X, coordinates.Y);
        }
    }
}
