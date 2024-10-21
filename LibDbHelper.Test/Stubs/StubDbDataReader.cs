using System.Collections;
using System.Data.Common;
using LibDbHelper.Test.Items;

namespace LibDbHelper.Test.Stubs;

public class StubDbDataReader(Table table) : DbDataReader
{
    public override object this[int ordinal] => table[ordinal][rowNum_];
    public override object this[string name] => table[name][rowNum_];

    public override int Depth { get; }
    public override int FieldCount { get; }
    public override bool HasRows { get; }
    public override bool IsClosed { get; }
    public override int RecordsAffected { get; }

    private int rowNum_ = -1;

    public override bool GetBoolean(int ordinal)
    {
        return (int)table[ordinal][rowNum_] != 0;
    }

    public override byte GetByte(int ordinal)
    {
        return (byte)table[ordinal][rowNum_];
    }

    public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
    {
        throw new NotImplementedException();
    }

    public override char GetChar(int ordinal)
    {
        return (char)table[ordinal][rowNum_];
    }

    public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
    {
        throw new NotImplementedException();
    }

    public override string GetDataTypeName(int ordinal)
    {
        return table[ordinal][rowNum_].GetType().Name;
    }

    public override DateTime GetDateTime(int ordinal)
    {
        return (DateTime)table[ordinal][rowNum_];
    }

    public override decimal GetDecimal(int ordinal)
    {
        return (decimal)table[ordinal][rowNum_];
    }

    public override double GetDouble(int ordinal)
    {
        return (double)table[ordinal][rowNum_];
    }

    public override IEnumerator GetEnumerator()
    {
        return table.Columns.Select(x => x[rowNum_]).GetEnumerator();
    }

    public override Type GetFieldType(int ordinal)
    {
        return table[ordinal][rowNum_].GetType();
    }

    public override float GetFloat(int ordinal)
    {
        return (float)table[ordinal][rowNum_];
    }

    public override Guid GetGuid(int ordinal)
    {
        return (Guid)table[ordinal][rowNum_];
    }

    public override short GetInt16(int ordinal)
    {
        return (short)table[ordinal][rowNum_];
    }

    public override int GetInt32(int ordinal)
    {
        return (int)table[ordinal][rowNum_];
    }

    public override long GetInt64(int ordinal)
    {
        return (long)table[ordinal][rowNum_];
    }

    public override string GetName(int ordinal)
    {
        return table[ordinal].Name;
    }

    public override int GetOrdinal(string name)
    {
        return table.ColumnNames.IndexOf(name);
    }

    public override string GetString(int ordinal)
    {
        return (string)table[ordinal][rowNum_];
    }

    public override object GetValue(int ordinal)
    {
        return table[ordinal][rowNum_];
    }

    public override int GetValues(object[] values)
    {
        var array = table.ColumnNames.Select(x => x[rowNum_]).ToArray();
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
        return 0 <= rowNum_ && rowNum_ < table.Columns[0].Values.Count;
    }
}