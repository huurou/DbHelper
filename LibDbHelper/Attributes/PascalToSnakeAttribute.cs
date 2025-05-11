using System;

namespace LibDbHelper.Attributes
{
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
