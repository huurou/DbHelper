using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibDbHelper.Test.Items
{
    public class Table : IEnumerable<Row>
    {
        public int Count => rows_.Count;
        public ReadOnlyCollection<string> ColumnNames { get; }
        public Row this[int index] => rows_[index];
        public Column this[string name] => new Column(name, rows_.Select(x => x[name]));

        private readonly List<Row> rows_ = new List<Row>();

        public Table(IEnumerable<string> columnNames)
        {
            if (columnNames is null) { throw new ArgumentNullException(nameof(columnNames)); }
            ColumnNames = columnNames.ToList().AsReadOnly();
        }

        public Table(params string[] columnNames) : this(columnNames?.AsEnumerable())
        {
        }

        public void Add(params object[] rows)
        {
            Add(rows.AsEnumerable());
        }

        public void Add(IEnumerable<object> row)
        {
            if (row is null) { return; }
            rows_.Add(new Row(ColumnNames, row));
        }

        public Column GetColumn(int ordinal)
        {
            return new Column(ColumnNames[ordinal], rows_.Select(x => x[ordinal]));
        }

        public IEnumerator<Row> GetEnumerator()
        {
            return rows_.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
