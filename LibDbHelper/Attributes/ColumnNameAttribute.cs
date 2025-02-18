using System;

namespace LibDbHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ColumnNameAttribute : Attribute
    {
        public string ColumnName { get; }

        public ColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PascalToSnakeAttribute : Attribute
    {
        public bool Capital { get; }

        public PascalToSnakeAttribute(bool capital = false)
        {
            Capital = capital;
        }
    }
}
