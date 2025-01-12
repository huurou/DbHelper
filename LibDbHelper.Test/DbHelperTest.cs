using LibDbHelper.Attributes;
using LibDbHelper.Test.Items;
using LibDbHelper.Test.Stubs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibDbHelper.Test
{
    public class DbHelperTest
    {
        private readonly DbHelper helper_ = new StubDbHelper();

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
                new Entity(2, "z")
            };
            ((StubDbHelper)helper_).SqlExecuted += (s, e) => AssertSql(e.Sql, e.Time);
            ((StubDbHelper)helper_).ParametersCreated += (s, e) => AssertParameters(e.Parameters, e.Time);

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
            // Stubで発行したイベントでAssertを行います。
            void AssertSql(string sql, int time)
            {
                // 改行コードの違いでテストに失敗しないようにStringBuilderを使用します。
                var sb = new StringBuilder();
                sb.AppendLine("INSERT ALL");
                sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                sb.AppendLine("VALUES (:a0,:b0,c)");
                sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                sb.AppendLine("VALUES (:a1,:b1,c)");
                sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                sb.AppendLine("VALUES (:a2,:b2,c)");
                sb.Append("SELECT * FROM DUAL");
                var expected = sb.ToString();
                Assert.Equal(0, time);
                Assert.Equal(expected, sql);
            }

            void AssertParameters(List<DbParameter> parameters, int time)
            {
                var expected = new Dictionary<string, object>
                {
                    { "a0", 0 },
                    { "b0", "x" },
                    { "a1", 1 },
                    { "b1", "y" },
                    { "a2", 2 },
                    { "b2", "z" }
                };
                Assert.Equal(0, time);
                Assert.Equal(expected, parameters.ToDictionary(x => x.ParameterName, x => x.Value));
            }
        }

        [Fact]
        public async Task BulkInsertAsync_chunkSize()
        {
            // Arange
            var table = "Schema.Table";
            var columns = new List<string> { "col_a", "col_b", "col_c" };
            var values = new List<string> { ":a", ":b", "c" };
            var entities = new List<Entity>
            {
                new Entity(0, "a"),
                new Entity(1, "b"),
                new Entity(2, "c"),
                new Entity(3, "d"),
                new Entity(4, "e"),
                new Entity(5, "f"),
                new Entity(6, "g"),
                new Entity(7, "h"),
                new Entity(8, "i"),
                new Entity(9, "j"),
                new Entity(10, "k"),
                new Entity(11, "l"),
                new Entity(12, "m"),
                new Entity(13, "n"),
                new Entity(14, "o"),
                new Entity(15, "p"),
                new Entity(16, "q"),
                new Entity(17, "r"),
                new Entity(18, "s"),
                new Entity(19, "t"),
                new Entity(20, "u"),
                new Entity(21, "v"),
                new Entity(22, "w"),
                new Entity(23, "x"),
                new Entity(24, "y"),
                new Entity(25, "z")
            };
            var chunkSize = 10;
            ((StubDbHelper)helper_).SqlExecuted += (s, e) => AssertSql(e.Sql, e.Time);
            ((StubDbHelper)helper_).ParametersCreated += (s, e) => AssertParameters(e.Parameters, e.Time);

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
                },
                chunkSize
            );

            // Assert
            // Stubで発行したイベントでAssertを行います。
            void AssertSql(string sql, int time)
            {
                // 改行コードの違いでテストに失敗しないようにStringBuilderを使用します。
                var sb = new StringBuilder();
                switch (time)
                {
                    case 0:
                        sb.AppendLine("INSERT ALL");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a0,:b0,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a1,:b1,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a2,:b2,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a3,:b3,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a4,:b4,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a5,:b5,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a6,:b6,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a7,:b7,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a8,:b8,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a9,:b9,c)");
                        sb.Append("SELECT * FROM DUAL");
                        break;

                    case 1:
                        sb.AppendLine("INSERT ALL");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a0,:b0,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a1,:b1,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a2,:b2,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a3,:b3,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a4,:b4,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a5,:b5,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a6,:b6,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a7,:b7,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a8,:b8,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a9,:b9,c)");
                        sb.Append("SELECT * FROM DUAL");
                        break;

                    case 2:
                        sb.AppendLine("INSERT ALL");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a0,:b0,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a1,:b1,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a2,:b2,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a3,:b3,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a4,:b4,c)");
                        sb.AppendLine("INTO Schema.Table (col_a,col_b,col_c)");
                        sb.AppendLine("VALUES (:a5,:b5,c)");
                        sb.Append("SELECT * FROM DUAL");
                        break;

                    default:
                        break;
                }
                var expected = sb.ToString();
                Assert.Equal(expected, sql);
            }

            void AssertParameters(List<DbParameter> parameters, int time)
            {
                Dictionary<string, object> expected = null;
                switch (time)
                {
                    case 0:
                        expected = new Dictionary<string, object>
                        {
                            { "a0", 0 },
                            { "b0", "a" },
                            { "a1", 1 },
                            { "b1", "b" },
                            { "a2", 2 },
                            { "b2", "c" },
                            { "a3", 3 },
                            { "b3", "d" },
                            { "a4", 4 },
                            { "b4", "e" },
                            { "a5", 5 },
                            { "b5", "f" },
                            { "a6", 6 },
                            { "b6", "g" },
                            { "a7", 7 },
                            { "b7", "h" },
                            { "a8", 8 },
                            { "b8", "i" },
                            { "a9", 9 },
                            { "b9", "j" },
                        };
                        break;

                    case 1:
                        expected = new Dictionary<string, object>
                        {
                            { "a0", 10 },
                            { "b0", "k" },
                            { "a1", 11 },
                            { "b1", "l" },
                            { "a2", 12 },
                            { "b2", "m" },
                            { "a3", 13 },
                            { "b3", "n" },
                            { "a4", 14 },
                            { "b4", "o" },
                            { "a5", 15 },
                            { "b5", "p" },
                            { "a6", 16 },
                            { "b6", "q" },
                            { "a7", 17 },
                            { "b7", "r" },
                            { "a8", 18 },
                            { "b8", "s" },
                            { "a9", 19 },
                            { "b9", "t" }
                        };
                        break;

                    case 2:
                        expected = new Dictionary<string, object>
                        {
                            { "a0", 20 },
                            { "b0", "u" },
                            { "a1", 21 },
                            { "b1", "v" },
                            { "a2", 22 },
                            { "b2", "w" },
                            { "a3", 23 },
                            { "b3", "x" },
                            { "a4", 24 },
                            { "b4", "y" },
                            { "a5", 25 },
                            { "b5", "z" }
                        };
                        break;

                    default: break;
                }
                Assert.Equal(expected, parameters.ToDictionary(x => x.ParameterName, x => x.Value));
            }
        }

        [Fact]
        public async Task BulkInsertAsync_ChunkSize0なら失敗()
        {
            // Arange
            var table = "Schema.Table";
            var columns = new List<string> { "col_a", "col_b", "col_c" };
            var values = new List<string> { ":a", ":b", "c" };
            var entities = new List<Entity>
            {
                new Entity(0, "x"),
                new Entity(1, "y"),
                new Entity(2, "z")
            };

            // Act
            var ex = await Record.ExceptionAsync(async () =>
                await helper_.BulkInsertAsync(
                    table, columns, values, entities,
                    (name, x) => throw new NotImplementedException(),
                    0
                )
            );

            // Assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task QueryAsync_ColumnName属性使用Guid列あり()
        {
            // Arange
            var guid0 = Guid.NewGuid();
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var table = new Table("col_a", "col_b", "col_c")
            {
                { 0, "x" , guid0 },
                { 1, "y" , guid1 },
                { 2, "z" , guid2 },
            };
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryAsync<Entity2>(sql, parameters);

            // Assert
            var expected = new List<Entity2>
            {
                new Entity2(0, "x", guid0 ),
                new Entity2(1, "y", guid1 ),
                new Entity2(2, "z", guid2 ),
            }.AsReadOnly();
            Assert.Equal(expected, actual);
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

        private class Entity2 : IEquatable<Entity2>
        {
            [ColumnName("col_a")]
            public int A { get; set; }

            [ColumnName("col_b")]
            public string B { get; set; }

            [ColumnName("col_c")]
            public Guid C { get; set; }

            public Entity2(int a, string b, Guid c)
            {
                A = a;
                B = b;
                C = c;
            }

            public bool Equals(Entity2 other)
            {
                return other != null &&
                    A == other.A &&
                    B == other.B &&
                    C == other.C;
            }
        }
    }

    // Arange

    // Act

    // Assert
}
