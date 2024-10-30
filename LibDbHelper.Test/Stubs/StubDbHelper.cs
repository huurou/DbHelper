using LibDbHelper.Test.Items;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDbHelper.Test.Stubs
{
    public class StubDbHelper : DbHelper
    {
        public event EventHandler<(string Sql, int Time)> SqlExecuted;
        public event EventHandler<(List<DbParameter> Parameters, int Time)> ParametersCreated;

        private Table table_;

        public StubDbHelper() : base("")
        {
        }

        public override DbConnection GetConnection()
        {
            return new StubDbConnection(table_);
        }

        protected override DbCommand GetCommand(string sql, DbConnection connection)
        {
            return new StubDbCommand(table_);
        }

        public override DbParameter GetParameter(string parameterName, object value)
        {
            return new StubDbParameter { ParameterName = parameterName, Value = value };
        }

        public override async Task BulkInsertAsync<T>(string table, IEnumerable<string> columns, IEnumerable<string> values, IEnumerable<T> entities, Func<string, T, object> getParameterValue, int chunkSize, char placeHolderSymbol)
        {
            if (chunkSize <= 0) { throw new ArgumentException("ChunkSizeには1以上を指定してください。", nameof(chunkSize)); }
            var chunks = entities.Select((x, i) => (x, i)).GroupBy(x => x.i / chunkSize, x => x.x);
            foreach (var chunk in chunks)
            {
                var sql = new StringBuilder();
                sql.AppendLine("INSERT ALL");
                var parameters = new List<DbParameter>();
                for (var i = 0; i < chunk.Count(); i++)
                {
                    sql.Append($"INTO {table}");
                    if (columns.Any())
                    {
                        sql.AppendLine($" ({string.Join(",", columns)})");
                    }
                    else
                    {
                        sql.AppendLine();
                    }
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
                    sql.AppendLine($"VALUES ({string.Join(",", valuesModified)})");
                }
                sql.Append("SELECT * FROM DUAL");
                await ExecuteAsync(sql.ToString(), parameters);

                // テスト用イベント
                SqlExecuted?.Invoke(this, (sql.ToString(), chunk.Key));
                ParametersCreated?.Invoke(this, (parameters, chunk.Key));
            }
        }

        public void SetTable(Table table)
        {
            table_ = table;
        }
    }
}
