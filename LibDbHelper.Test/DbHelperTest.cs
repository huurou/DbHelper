using LibDbHelper.Attributes;
using LibDbHelper.Test.Items;
using LibDbHelper.Test.Stubs;
using System.Data.Common;
using Xunit.Abstractions;

namespace LibDbHelper.Test;

public class DbHelperTest(ITestOutputHelper testOutputHelper)
{
    private readonly DbHelper helper_ = new StubDbHelper(testOutputHelper);

    [Fact]
    public async Task QueryAsync_ColumnName�����s�g�p()
    {
        // Arange
        var table = new Table(
            ["col_a", "col_b"],
            [
                [0, 1, 2],
                ["x", "y", "z"],
            ]
        );
        ((StubDbHelper)helper_).SetTable(table);
        var sql = "SELECT * FROM T";
        var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

        // Act
        var actual = await helper_.QueryAsync(
            sql, parameters,
            r => new Entity(r.GetInt("col_a"), r.GetString("col_b"))
        );

        //Assert
        Assert.Equal([new(0, "x"), new(1, "y"), new(2, "z")], actual);
    }

    [Fact]
    public async Task QueryAsync_ColumnName�����g�p()
    {
        // Arange
        var table = new Table(
            ["col_a", "col_b"],
            [
                [0, 1, 2],
                ["x", "y", "z"],
            ]
        );
        ((StubDbHelper)helper_).SetTable(table);
        var sql = "SELECT * FROM T";
        var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

        // Act
        var actual = await helper_.QueryAsync<Entity>(sql, parameters);

        //Assert
        Assert.Equal([new(0, "x"), new(1, "y"), new(2, "z")], actual);
    }

    [Fact]
    public async Task ExecuteAsync()
    {
        // Arange
        var sql = "UPDATE T SET col_a = 1 WHERE col_b = :b";
        var parameters = new List<DbParameter> { helper_.GetParameter("b", 1) };

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
                _ => throw new ArgumentException($"�o�^����Ă��Ȃ��p�����[�^�[���ł��B name:{name}", nameof(name))
            }
        );

        //Assert
    }

    private record Entity(
        [property: ColumnName("col_a")] int A,
        [property: ColumnName("col_b")] string B
    );
}

// Arange

// Act

//Assert
