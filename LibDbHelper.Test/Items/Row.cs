using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LibDbHelper.Test.Items
{
    public class Row : IEnumerable<object>
    {
        public object this[int ordinal] => values_[ordinal];
        public object this[string columnName] => values_[columnNames_.IndexOf(columnName)];

        private readonly List<string> columnNames_;
        private readonly List<object> values_;

        public Row(IEnumerable<string> columnNames, IEnumerable<object> values)
        {
            if (columnNames is null) { throw new ArgumentNullException(nameof(columnNames)); }
            if (values is null) { throw new ArgumentNullException(nameof(values)); }
            columnNames_ = columnNames.ToList();
            values_ = values.ToList();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return values_.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
