using System.Collections.Generic;
using System.Linq;

namespace LibDbHelper.Test.Items
{
    public class Column
    {
        public string Name { get; set; }
        public List<object> Values { get; set; }

        public object this[int index] => Values[index];

        public Column(string name, IEnumerable<object> values)
        {
            Name = name;
            Values = values.ToList();
        }
    }
}
