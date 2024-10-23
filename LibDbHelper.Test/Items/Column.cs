using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibDbHelper.Test.Items
{
    public class Column : IEnumerable<object>
    {
        public string Name { get; }
        public object this[int index] => values_[index];

        private readonly ReadOnlyCollection<object> values_;

        public Column(string name, IEnumerable<object> values)
        {
            if (name is null) { throw new ArgumentNullException(nameof(name)); }
            if (values is null) { throw new ArgumentNullException(nameof(values)); }
            Name = name;
            values_ = values.ToList().AsReadOnly();
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
