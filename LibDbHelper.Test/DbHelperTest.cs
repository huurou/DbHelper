using LibDbHelper.Attributes;
using LibDbHelper.Test.Items;
using LibDbHelper.Test.Stubs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LibDbHelper.Test
{
    public class DbHelperTest
    {
        private readonly DbHelper helper_;

        public DbHelperTest(ITestOutputHelper testOutputHelper)
        {
            helper_ = new StubDbHelper(testOutputHelper);
        }

        [Fact]
        public async Task QueryAsync_ColumnName属性不使用()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" },
                { 1, "y" },
                { 2, "z" },
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryAsync(
                sql,
                r => new Entity(r.GetInt("col_a"), r.GetString("col_b")),
                parameters
            );

            // Assert
            var expected = new List<Entity>
            {
                new Entity(0, "x"),
                new Entity(1, "y"),
                new Entity(2, "z"),
            }.AsReadOnly();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryAsync_ColumnName属性使用()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" },
                { 1, "y" },
                { 2, "z" },
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryAsync<Entity>(sql, parameters);

            // Assert
            var expected = new List<Entity>
            {
                new Entity(0, "x"),
                new Entity(1, "y"),
                new Entity(2, "z"),
            }.AsReadOnly();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryAsync_int返す()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" },
                { 1, "y" },
                { 2, "z" },
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryAsync(sql, r => r.GetInt("col_a"), parameters);

            // Assert
            var expected = new List<int> { 0, 1, 2 }.AsReadOnly();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryFirstAsync_結果セット0件のとき失敗()
        {
            // Arange
            var table = new Table("col_a", "col_b");
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var exception = await Record.ExceptionAsync(async () => _ = await helper_.QueryFirstAsync<Entity>(sql, parameters));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal("レコードが存在しません。", exception.Message);
        }

        [Fact]
        public async Task QueryFirstAsync_結果セット1件()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" }
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryFirstAsync<Entity>(sql, parameters);

            // Assert
            var expected = new Entity(0, "x");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryFirstAsync_結果セット1件_int返す()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" }
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryFirstAsync(sql, r => r.GetInt("col_a"), parameters);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task QuerySingleAsync_結果セット0件のとき失敗()
        {
            // Arange
            var table = new Table("col_a", "col_b");
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var exception = await Record.ExceptionAsync(async () => _ = await helper_.QuerySingleAsync<Entity>(sql, parameters));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal("レコードが存在しません。", exception.Message);
        }

        [Fact]
        public async Task QuerySingleAsync_結果セット1件()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" }
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QuerySingleAsync<Entity>(sql, parameters);

            // Assert
            var expected = new Entity(0, "x");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QuerySingleAsync_結果セット1件_int返す()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" }
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QuerySingleAsync(sql, r => r.GetInt("col_a"), parameters);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task QuerySingleAsync_結果セット2件のとき失敗()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" },
                { 1, "y" }
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var exception = await Record.ExceptionAsync(async () => _ = await helper_.QuerySingleAsync<Entity>(sql, parameters));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal("複数のレコードが存在します。", exception.Message);
        }

        [Fact]
        public async Task ExecuteScalarAsync_結果セット0件()
        {
            // Arange
            var table = new Table("col_a", "col_b");
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";

            // Act
            var actual = await helper_.ExecuteScalarAsync<int?>(sql);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task ExecuteScalarAsync_結果セット1件()
        {
            // Arange
            var table = new Table("col_a", "col_b")
            {
                { 0, "x" }
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";

            // Act
            var actual = await helper_.ExecuteScalarAsync<int>(sql);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            // Arange
            var sql = "UPDATE T SET col_a = 1 WHERE col_b = :b";
            var parameters = new List<DbParameter> { helper_.GetParameter("b", 1) };

            // Act
            await helper_.ExecuteAsync(sql, parameters);

            // Assert
        }

        [Fact]
        public async Task ExecuteAsyncWithConnection()
        {
            var sql1 = "UPDATE T SET col_a = 1 WHERE col_b = :b";
            var sql2 = "UPDATE T SET col_b = 2 WHERE col_c = :c";
            var parameters1 = new List<DbParameter> { helper_.GetParameter("b", 1) };
            var parameters2 = new List<DbParameter> { helper_.GetParameter("c", 2) };
            using (var connection = helper_.GetConnection())
            {
                await connection.OpenAsync();
                var transaction = connection.BeginTransaction();
                try
                {
                    await helper_.ExecuteWithConnectionAsync(sql1, parameters1, connection);
                    await helper_.ExecuteWithConnectionAsync(sql2, parameters2, connection);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        [Fact]
        public async Task BulkInsertAsync()
        {
            // Arange
            var table = "Schema.Table";
            var columns = new List<string> { "col_a", "col_b", "col_c" };
            var values = new List<string> { ":a", ":b", "c" };
            var entities = new List<Entity>
            {
                new Entity(0, "x"),
                new Entity(1, "y"),
                new Entity(2, "z"),
            };

            // Act
            await helper_.BulkInsertAsync(
                table, columns, values, entities,
                (name, x) =>
                {
                    switch (name)
                    {
                        case "a":
                            return x.A;

                        case "b":
                            return x.B;

                        default:
                            throw new ArgumentException($"登録されていないパラメーター名です。 name:{name}", nameof(name));
                    }
                }
            );

            // Assert
        }

        private class Entity : IEquatable<Entity>
        {
            [ColumnName("col_a")]
            public int A { get; set; }

            [ColumnName("col_b")]
            public string B { get; set; }

            public Entity(int a, string b)
            {
                A = a;
                B = b;
            }

            public bool Equals(Entity other)
            {
                return other != null &&
                    A == other.A &&
                    B == other.B;
            }
        }
    }

    // Arange

    // Act

    // Assert
}
