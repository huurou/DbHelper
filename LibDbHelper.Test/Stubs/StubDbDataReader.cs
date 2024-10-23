using LibDbHelper.Test.Items;
using System;
using System.Collections;
using System.Data.Common;
using System.Linq;

namespace LibDbHelper.Test.Stubs
{
    public class StubDbDataReader : DbDataReader
    {
        public override object this[int ordinal] => table_[rowNum_][ordinal];
        public override object this[string name] => table_[rowNum_][name];

        public override int Depth { get; }
        public override int FieldCount { get; }
        public override bool HasRows { get; }
        public override bool IsClosed { get; }
        public override int RecordsAffected { get; }

        private readonly Table table_;
        private int rowNum_ = -1;

        public StubDbDataReader(Table table)
        {
            table_ = table;
        }

        public override bool GetBoolean(int ordinal)
        {
            return (int)table_[rowNum_][ordinal] != 0;
        }

        public override byte GetByte(int ordinal)
        {
            return (byte)table_[rowNum_][ordinal];
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            return (char)table_[rowNum_][ordinal];
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            return table_[rowNum_][ordinal].GetType().Name;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return (DateTime)table_[rowNum_][ordinal];
        }

        public override decimal GetDecimal(int ordinal)
        {
            return (decimal)table_[rowNum_][ordinal];
        }

        public override double GetDouble(int ordinal)
        {
            return (double)table_[rowNum_][ordinal];
        }

        public override IEnumerator GetEnumerator()
        {
            return table_.GetEnumerator();
        }

        public override Type GetFieldType(int ordinal)
        {
            return table_[rowNum_][ordinal].GetType();
        }

        public override float GetFloat(int ordinal)
        {
            return (float)table_[rowNum_][ordinal];
        }

        public override Guid GetGuid(int ordinal)
        {
            return (Guid)table_[rowNum_][ordinal];
        }

        public override short GetInt16(int ordinal)
        {
            return (short)table_[rowNum_][ordinal];
        }

        public override int GetInt32(int ordinal)
        {
            return (int)table_[rowNum_][ordinal];
        }

        public override long GetInt64(int ordinal)
        {
            return (long)table_[rowNum_][ordinal];
        }

        public override string GetName(int ordinal)
        {
            return table_.GetColumn(ordinal).Name;
        }

        public override int GetOrdinal(string name)
        {
            return table_.ColumnNames.IndexOf(name);
        }

        public override string GetString(int ordinal)
        {
            return (string)table_[rowNum_][ordinal];
        }

        public override object GetValue(int ordinal)
        {
            return table_[rowNum_][ordinal];
        }

        public override int GetValues(object[] values)
        {
            var array = table_.ColumnNames.Select(x => x[rowNum_]).ToArray();
            array.CopyTo(values, 0);
            return array.Length;
        }

        public override bool IsDBNull(int ordinal)
        {
            return this[ordinal] is DBNull;
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            rowNum_++;
            return 0 <= rowNum_ && rowNum_ < table_.Count;
        }
    }
}