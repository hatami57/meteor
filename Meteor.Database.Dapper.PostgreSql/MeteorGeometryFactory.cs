using NetTopologySuite.Geometries;

namespace Meteor.Database.Dapper.PostgreSql
{
    public static class MeteorGeometryFactory
    {
        public static GeometryFactory Default { get; } = new GeometryFactory(GeometryFactory.Default.PrecisionModel,
            4326);
    }
}