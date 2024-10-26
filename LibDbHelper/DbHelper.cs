using LibDbHelper.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
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

        public abstract DbConnection GetConnection();

        protected abstract DbCommand GetCommand(string sql, DbConnection connection);

        public abstract DbParameter GetParameter(string parameterName, object value);

        /// <summary>
        /// Queryの非同期バージョンです。
        /// クエリを実行し結果セットを返します。
        /// ColumnName属性に基づいてEntityを作成します。
        /// </summary>
        /// <typeparam name="T">結果セットのアイテムの型</typeparam>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        public async Task<ReadOnlyCollection<T>> QueryAsync<T>(string sql, IEnumerable<DbParameter> parameters = default, CancellationToken cancellationToken = default) where T : class
        {
            return await QueryAsync(sql, CreateEntity<T>, parameters, cancellationToken);
        }

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
            using (var connection = GetConnection())
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

        /// <summary>
        /// QueryFirstの非同期バージョンです。
        /// クエリを実行し結果セットの最初の行を返します。
        /// 他のすべての行は無視されます。
        /// 結果セットが0件のとき例外をスローします。
        /// ColumnName属性に基づいてEntityを作成します。
        /// </summary>
        /// <typeparam name="T">結果セットのアイテムの型</typeparam>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        /// <returns>結果セット</returns>
        public async Task<T> QueryFirstAsync<T>(string sql, IEnumerable<DbParameter> parameters = default, CancellationToken cancellationToken = default) where T : class
        {
            return await QueryFirstAsync(sql, CreateEntity<T>, parameters, cancellationToken);
        }

        /// <summary>
        /// QueryFirstの非同期バージョンです。
        /// クエリを実行し結果セットの最初の行を返します。
        /// 他のすべての行は無視されます。
        /// 結果セットが0件のとき例外をスローします。
        /// </summary>
        /// <typeparam name="T">結果セットのアイテムの型</typeparam>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="createEntity">実行結果から結果セットのアイテムを作成するデリゲート</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        /// <returns>結果セット</returns>
        public async Task<T> QueryFirstAsync<T>(string sql, Func<DbDataReader, T> createEntity, IEnumerable<DbParameter> parameters = default, CancellationToken cancellationToken = default)
        {
            using (var connection = GetConnection())
            using (var command = GetCommand(sql, connection))
            {
                await connection.OpenAsync(cancellationToken);
                if (parameters != null && parameters.Any())
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return createEntity(reader);
                    }
                    throw new InvalidOperationException("レコードが存在しません。");
                }
            }
        }

        /// <summary>
        /// QuerySingleの非同期バージョンです。
        /// クエリを実行し結果セットを返します。
        /// 結果セットが1件でないとき例外をスローします。
        /// ColumnName属性に基づいてEntityを作成します。
        /// </summary>
        /// <typeparam name="T">結果セットのアイテムの型</typeparam>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        /// <returns>結果セット</returns>
        public async Task<T> QuerySingleAsync<T>(string sql, IEnumerable<DbParameter> parameters = default, CancellationToken cancellationToken = default) where T : class
        {
            return await QuerySingleAsync(sql, CreateEntity<T>, parameters, cancellationToken);
        }

        /// <summary>
        /// QuerySingleの非同期バージョンです。
        /// クエリを実行し結果セットを返します。
        /// 結果セットが1件でないとき例外をスローします。
        /// </summary>
        /// <typeparam name="T">結果セットのアイテムの型</typeparam>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="createEntity">実行結果から結果セットのアイテムを作成するデリゲート</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        /// <returns>結果セット</returns>
        public async Task<T> QuerySingleAsync<T>(string sql, Func<DbDataReader, T> createEntity, IEnumerable<DbParameter> parameters = default, CancellationToken cancellationToken = default)
        {
            using (var connection = GetConnection())
            using (var command = GetCommand(sql, connection))
            {
                await connection.OpenAsync(cancellationToken);
                if (parameters != null && parameters.Any())
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    var result = default(T);
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        result = createEntity(reader);
                    }
                    else
                    {
                        throw new InvalidOperationException("レコードが存在しません。");
                    }
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        throw new InvalidOperationException("複数のレコードが存在します。");
                    }
                    else
                    {
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// トランザクション内でSQLステートメントを実行します。
        /// </summary>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="isolationLevel">トランザクションロック動作のレベル</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        public async Task ExecuteAsync(string sql, IEnumerable<DbParameter> parameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            using (var connection = GetConnection())
            using (var command = GetCommand(sql, connection))
            {
                await connection.OpenAsync(cancellationToken);
                var transaction = connection.BeginTransaction(isolationLevel);
                try
                {
                    if (parameters != null && parameters.Any())
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    await command.ExecuteNonQueryAsync(cancellationToken);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// トランザクション内でSQLステートメントを実行します。
        /// 事前に作成したコネクションを使用します。
        /// トランザクションを使用する場合は事前にBeginする必要があります。
        /// </summary>
        /// <param name="sql">実行するSQL</param>
        /// <param name="parameters">パラメーターのコレクション</param>
        /// <param name="connection">コネクション</param>
        /// <param name="isolationLevel">トランザクションロック動作のレベル</param>
        /// <param name="cancellationToken">キャンセル要求を監視するためのトークン</param>
        /// <returns></returns>
        public async Task ExecuteWithConnectionAsync(string sql, IEnumerable<DbParameter> parameters, DbConnection connection, CancellationToken cancellationToken = default)
        {
            using (var command = GetCommand(sql, connection))
            {
                if (parameters != null && parameters.Any())
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public abstract Task BulkInsertAsync<T>(string table, IEnumerable<string> columns, IEnumerable<string> values, IEnumerable<T> entities, Func<string, T, object> getParameterValue, char placeHolderSymbol = ':');

        // ColumnNameAttributeを元にして結果セットを作成する
        private T CreateEntity<T>(DbDataReader reader) where T : class
        {
            var entity = FormatterServices.GetUninitializedObject(typeof(T)) as T;
            foreach (var propInfo in typeof(T).GetProperties())
            {
                if (!(Attribute.GetCustomAttribute(propInfo, typeof(ColumnNameAttribute)) is ColumnNameAttribute columnNameAttribute)) { continue; }
                var propType = propInfo.PropertyType;

                var columnIndex = reader.GetOrdinal(columnNameAttribute.ColumnName);
                // プロパティがNull許容値型の場合の基の型 Null許容値型でない場合はnull
                var underlyingType = Nullable.GetUnderlyingType(propType);
                if (reader.IsDBNull(columnIndex))
                {
                    // string or Null許容値型
                    if (propType == typeof(string) || underlyingType != null)
                    {
                        propInfo.SetValue(entity, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Nullやで\nプロパティの型：{propType}");
                    }
                }
                else
                {
                    var typeCode = Type.GetTypeCode(underlyingType ?? propType);
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            propInfo.SetValue(entity, reader.GetString(columnIndex));
                            break;

                        case TypeCode.Char:
                            propInfo.SetValue(entity, reader.GetChar(columnIndex));
                            break;

                        case TypeCode.SByte:
                            propInfo.SetValue(entity, (sbyte)reader.GetByte(columnIndex));
                            break;

                        case TypeCode.Byte:
                            propInfo.SetValue(entity, reader.GetByte(columnIndex));
                            break;

                        case TypeCode.Int16:
                            propInfo.SetValue(entity, reader.GetInt16(columnIndex));
                            break;

                        case TypeCode.UInt16:
                            propInfo.SetValue(entity, (ushort)reader.GetInt16(columnIndex));
                            break;

                        case TypeCode.Int32:
                            propInfo.SetValue(entity, reader.GetInt32(columnIndex));
                            break;

                        case TypeCode.UInt32:
                            propInfo.SetValue(entity, (uint)reader.GetInt32(columnIndex));
                            break;

                        case TypeCode.Int64:
                            propInfo.SetValue(entity, reader.GetInt64(columnIndex));
                            break;

                        case TypeCode.UInt64:
                            propInfo.SetValue(entity, (ulong)reader.GetInt64(columnIndex));
                            break;

                        case TypeCode.Single:
                            propInfo.SetValue(entity, reader.GetFloat(columnIndex));
                            break;

                        case TypeCode.Double:
                            propInfo.SetValue(entity, reader.GetDouble(columnIndex));
                            break;

                        case TypeCode.Decimal:
                            propInfo.SetValue(entity, reader.GetDecimal(columnIndex));
                            break;

                        case TypeCode.DateTime:
                            propInfo.SetValue(entity, reader.GetDateTime(columnIndex));
                            break;

                        case TypeCode.String:
                            propInfo.SetValue(entity, reader.GetString(columnIndex));
                            break;

                        case TypeCode.Empty:
                        case TypeCode.Object:
                        case TypeCode.DBNull:
                        default: throw new NotSupportedException($"TypeCode:{typeCode} propType:{propType} propName:{propInfo.Name}");
                    }
                }
            }
            return entity;
        }
    }
}
