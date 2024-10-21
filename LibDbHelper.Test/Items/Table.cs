namespace LibDbHelper.Test.Items;

public class Table(IEnumerable<string> columnNames, IEnumerable<IEnumerable<object>> columnValues)
{
    public List<string> ColumnNames { get; } = [.. columnNames];
    public List<Column> Columns { get; } = [.. columnNames.Select((x, i) => new Column(x, columnValues.ElementAt(i)))];
    public Column this[int index] => Columns[index];
    public Column this[string name] => Columns[ColumnNames.IndexOf(name)];
}
