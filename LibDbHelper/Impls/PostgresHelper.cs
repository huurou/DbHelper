using Npgsql;
using System.Data.Common;

namespace LibDbHelper.Impls
{
    public class PostgresHelper : DbHelper
    {
        public PostgresHelper(string connectionString) : base(connectionString)
        {
        }

        protected override DbConnection GetConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        protected override DbCommand GetCommand(string sql, DbConnection connection)
        {
            return new NpgsqlCommand(sql, (NpgsqlConnection)connection);
        }

        protected override DbParameter GetParameter(string parameterName, object value)
        {
            return new NpgsqlParameter(parameterName, value);
        }
    }
}
