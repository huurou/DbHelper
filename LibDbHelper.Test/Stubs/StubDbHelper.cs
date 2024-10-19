using System.Data.Common;
using System.Text;
using Xunit.Abstractions;

namespace LibDbHelper.Test.Stubs;

public class StubDbHelper(ITestOutputHelper testOutputHelper) : DbHelper("")
{
    protected override DbConnection GetConnection(string connectionString)
    {
        return new StubDbConnection();
    }

    protected override DbCommand GetCommand(string sql, DbConnection connection)
    {
        return new StubDbCommand();
    }

    public override DbParameter GetParameter(string parameterName, object value)
    {
        return new StubDbParameter { ParameterName = parameterName, Value = value };
    }

    public override async Task BulkInsertAsync<T>(string table, IEnumerable<string> columns, IEnumerable<string> values, IEnumerable<T> entities, Func<string, T, object> getParameterValue, char placeHolderSymbol)
    {
        var sql = new StringBuilder();
        sql.AppendLine("INSERT ALL");
        var parameters = new List<DbParameter>();
        for (var i = 0; i < entities.Count(); i++)
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
                    parameters.Add(GetParameter($"{name}{i}", getParameterValue(name, entities.ElementAt(i))));
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

        // テスト用標出力
        testOutputHelper.WriteLine($"sql:");
        testOutputHelper.WriteLine(sql.ToString());
        testOutputHelper.WriteLine("parameters:");
        testOutputHelper.WriteLine(string.Join("\n", parameters.Select(x => x.ToString())));
    }
}
