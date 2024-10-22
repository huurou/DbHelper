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
            var table = new Table(
                new List<string> { "col_a", "col_b" },
                new List<IEnumerable<object>>
                {
                    new List<object>{ 0, 1, 2 },
                    new List<object>{ "x", "y", "z" },
                }
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
            var table = new Table(
                new List<string> { "col_a", "col_b" },
                new List<IEnumerable<object>>
                {
                    new List<object>{ 0, 1, 2 },
                    new List<object>{ "x", "y", "z" },
                }
            );
            ((StubDbHelper)helper_).SetTable(table);
            var sql = "SELECT * FROM T";
            var parameters = new List<DbParameter> { helper_.GetParameter("n", 1) };

            // Act
            var actual = await helper_.QueryAsync<Entity>(sql, parameters);

            //Assert
            var expected = new List<Entity>
            {
                new Entity(0, "x"),
                new Entity(1, "y"),
                new Entity(2, "z"),
            }.AsReadOnly();
            Assert.Equal(expected, actual);
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

            //Assert
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

    //Assert
}
