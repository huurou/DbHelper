namespace LibDbHelper.Test.Items;

public class Column(string name, IEnumerable<object> values)
{
    public string Name { get; set; } = name;
    public List<object> Values { get; set; } = [.. values];
    public object this[int index] => Values[index];
}
