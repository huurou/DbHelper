using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibDbHelper
{
    public abstract class DbHelper
    {
        public string ConnectionString { get; }

        protected DbHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected abstract DbConnection GetConnection(string connectionString);

        protected abstract DbCommand GetCommand(string sql, DbConnection connection);

        protected abstract DbParameter GetParameter(string parameterName, object value);

        /// <summary>
        /// Queryの非同期バージョンです。
        /// クエリを実行し結果セットを返します。
        /// </summary>
        /// <typeparam name="T">結果セットのアイテムの型</typeparam>
        /// <param name="sql">実行するSQL</param>
        /// <param name="createEntity">実行結果から結果セットのアイテムを作成するデリゲート</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        /// <returns>結果セット</returns>
        public async Task<ReadOnlyCollection<T>> QueryAsync<T>(string sql, Func<DbDataReader, T> createEntity, IEnumerable<DbParameter> parameters = default, CancellationToken cancellationToken = default)
        {
            using (var connection = GetConnection(ConnectionString))
            using (var command = GetCommand(sql, connection))
            {
                await connection.OpenAsync(cancellationToken);
                if (parameters != null && parameters.Any())
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    var result = new List<T>();
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        result.Add(createEntity(reader));
                    }
                    return result.AsReadOnly();
                }
            }
        }
    }
}
