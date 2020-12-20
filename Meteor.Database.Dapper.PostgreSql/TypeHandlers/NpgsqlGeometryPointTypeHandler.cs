using System.Data;
using Dapper;
using NetTopologySuite.Geometries;
using Npgsql;

namespace Meteor.Database.Dapper.PostgreSql.TypeHandlers
{
    public class NpgsqlGeometryPointTypeHandler : SqlMapper.TypeHandler<Point>
    {
        public override void SetValue(IDbDataParameter parameter, Point value)
        {
            if (parameter is NpgsqlParameter param)
                param.NpgsqlValue = value;
        }

        public override Point Parse(object value)
        {
            return value as Point;
        }
    }
}