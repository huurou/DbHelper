using LibDbHelper.Test.Stubs;
using System.Data.Common;
using Xunit.Abstractions;

namespace LibDbHelper.Test;

public class DbHelperTest(ITestOutputHelper testOutputHelper)
{
    private readonly DbHelper helper_ = new StubDbHelper(testOutputHelper);

    [Fact]
    public async Task QueryAsync()
    {
        // Arange
        var sql = "SELECT * FROM T";
        var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

        // Act
        var actual = await helper_.QueryAsync(sql, r => 1, parameters);

        //Assert
    }

    [Fact]
    public async Task ExecuteAsync()
    {
        // Arange
        var sql = "UPDATE T SET col_a = 1";
        var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

        // Act
        await helper_.ExecuteAsync(sql, parameters);

        //Assert
    }

    [Fact]
    public async Task BulkInsertAsync()
    {
        // Arange
        var table = "Schema.Table";
        List<string> columns = ["col_a", "col_b", "col_c"];
        List<string> values = [":a", ":b", "c"];
        List<Entity> entities = [new(0, "x"), new(1, "y"), new(2, "z")];

        // Act
        await helper_.BulkInsertAsync(
            table, columns, values, entities,
            (name, x) => name switch
            {
                "a" => x.A,
                "b" => x.B,
                _ => throw new ArgumentException($"登録されていないパラメーター名です。 name:{name}", nameof(name))
            }
        );

        //Assert
    }

    private class Entity(int a, string b)
    {
        public int A { get; set; } = a;
        public string B { get; set; } = b;
    }
}

// Arange

// Act

//Assert
