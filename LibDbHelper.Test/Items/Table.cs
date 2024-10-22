using System.Collections.Generic;
using System.Linq;

namespace LibDbHelper.Test.Items
{
    public class Table
    {
        public List<string> ColumnNames { get; }
        public List<Column> Columns { get; }

        public Column this[int index] => Columns[index];
        public Column this[string name] => Columns[ColumnNames.IndexOf(name)];

        public Table(IEnumerable<string> columnNames, IEnumerable<IEnumerable<object>> columnValues)
        {
            ColumnNames = columnNames.ToList();
            Columns = columnNames.Select((x, i) => new Column(x, columnValues.ElementAt(i))).ToList();
        }
    }
}
