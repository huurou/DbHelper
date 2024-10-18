using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace LibDbHelper
{
    public class OracleHelper : DbHelper
    {
        public OracleHelper(string connectionString) : base(connectionString)
        {
        }

        protected override DbConnection GetConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }

        protected override DbCommand GetCommand(string sql, DbConnection connection)
        {
            return new OracleCommand(sql, (OracleConnection)connection);
        }

        protected override DbParameter GetParameter(string parameterName, object value)
        {
            return new OracleParameter(parameterName, value);
        }
    }
}
