﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace LibDbHelper.Impls
{
    public class PostgresHelper : DbHelper
    {
        public PostgresHelper(string connectionString) : base(connectionString)
        {
        }

        protected override DbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        protected override DbCommand GetCommand(string sql)
        {
            return new NpgsqlCommand(sql, (NpgsqlConnection)GetConnection());
        }

        public override DbParameter GetParameter(string parameterName, object value)
        {
            return new NpgsqlParameter(parameterName, value);
        }

        public override Task BulkInsertAsync<T>(string table, IEnumerable<string> columns, IEnumerable<string> values, IEnumerable<T> entities, Func<string, T, object> getParameterValue, char placeHolderSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
