using System;
using System.Data.Common;

namespace LibDbHelper
{
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// 指定された列名の値を文字列として取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static string GetString(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? throw new InvalidOperationException($"値がDBNullでした columnName:{columnName}") : reader.GetString(ordinal);
        }

        /// <summary>
        /// 指定された列名の値を文字列として取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns>指定の列名の値</returns>
        public static string GetStringNullable(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetStringNullable(ordinal);
        }

        /// <summary>
        /// 指定された列名の値を文字列として取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="ordinal">列のインデックス</param>
        /// <returns>指定の列名の値</returns>
        public static string GetStringNullable(this DbDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        /// <summary>
        /// 指定された列名の値をdecimalとして取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static decimal GetDecimal(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? throw new InvalidOperationException($"値がDBNullでした columnName:{columnName}") : reader.GetDecimal(ordinal);
        }

        /// <summary>
        /// 指定された列名の値をdecimal?として取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static decimal? GetDecimalNullable(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (decimal?)null : reader.GetDecimal(ordinal);
        }

        /// <summary>
        /// 指定された列名の値をintとして取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static int GetInt(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? throw new InvalidOperationException($"値がDBNullでした columnName:{columnName}") : reader.GetInt32(ordinal);
        }

        /// <summary>
        /// 指定された列名の値をint?として取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static int? GetIntNullable(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
        }

        /// <summary>
        /// 指定された列名の値をDateTimeとして取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static DateTime GetDateTime(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? throw new InvalidOperationException($"値がDBNullでした columnName:{columnName}") : reader.GetDateTime(ordinal);
        }

        /// <summary>
        /// 指定された列名の値をDateTime?として取得します。
        /// </summary>
        /// <param name="reader">DbDataReader継承クラスのインスタンス</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static DateTime? GetDateTimeNullable(this DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
        }
    }
}
