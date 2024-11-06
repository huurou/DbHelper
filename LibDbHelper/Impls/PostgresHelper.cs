using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDbHelper.Impls
{
    public class PostgresHelper : DbHelper
    {
        public PostgresHelper(string connectionString) : base(connectionString)
        {
        }

        public override DbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        protected override DbCommand GetCommand(string sql, DbConnection connection)
        {
            return new NpgsqlCommand(sql, (NpgsqlConnection)connection);
        }

        public override DbParameter GetParameter(string parameterName, object value)
        {
            return new NpgsqlParameter(parameterName, value);
        }

        public override async Task BulkInsertAsync<T>(string table, IEnumerable<string> columns, IEnumerable<string> values, IEnumerable<T> entities, Func<string, T, object> getParameterValue, int chunkSize, char placeHolderSymbol)
        {
            if (chunkSize <= 0) { throw new ArgumentException("ChunkSizeには1以上を指定してください。", nameof(chunkSize)); }
            var chunks = entities.Select((x, i) => (x, i)).GroupBy(x => x.i / chunkSize, x => x.x);
            foreach (var chunk in chunks)
            {
                var sql = new StringBuilder();
                sql.AppendLine($"INSERT INTO {table}");
                if (columns.Any())
                {
                    sql.AppendLine($"({string.Join(",", columns)})");
                }
                sql.AppendLine("VALUES");
                var parameters = new List<DbParameter>();
                for (var i = 0; i < chunk.Count(); i++)
                {
                    var valuesModified = new List<string>();
                    foreach (var value in values)
                    {
                        // 先頭に{placeHolderSymbol}が付いていたらbind変数と判断する
                        // 連番を付与してパラメーターを作成する
                        if (value.StartsWith(placeHolderSymbol.ToString()))
                        {
                            var name = value.TrimStart(placeHolderSymbol);
                            valuesModified.Add($"{placeHolderSymbol}{name}{i}");
                            parameters.Add(GetParameter($"{name}{i}", getParameterValue(name, chunk.ElementAt(i))));
                        }
                        // 先頭に付いていなかったらリテラルと判断する
                        else
                        {
                            valuesModified.Add(value);
                        }
                    }
                    sql.AppendLine($"({string.Join(",", valuesModified)}){(i != chunk.Count() - 1 ? "," : "")}");
                }
                await ExecuteAsync(sql.ToString(), parameters);
            }
        }
    }
}
