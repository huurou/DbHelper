using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace LibDbHelper.Test.Stubs
{
    public class StubDbParameterCollection : DbParameterCollection
    {
        private readonly List<StubDbParameter> parameters_ = new List<StubDbParameter>();

        public override int Count => parameters_.Count;
        public override object SyncRoot => ((IList)parameters_).SyncRoot;

        public override int Add(object value)
        {
            return ((IList)parameters_).Add(value);
        }

        public override void AddRange(Array values)
        {
            foreach (var item in values)
            {
                Add(item);
            }
        }

        public override void Clear()
        {
            parameters_.Clear();
        }

        public override bool Contains(object value)
        {
            return parameters_.Select(x => x.Value).Contains(value);
        }

        public override bool Contains(string parameterName)
        {
            return parameters_.Select(x => x.ParameterName).Contains(parameterName);
        }

        public override void CopyTo(Array array, int index)
        {
            ((IList)parameters_).CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return parameters_.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            return parameters_.Select(x => x.Value).ToList().IndexOf(value);
        }

        public override int IndexOf(string parameterName)
        {
            return parameters_.Select(x => x.ParameterName).ToList().IndexOf(parameterName);
        }

        public override void Insert(int index, object value)
        {
            ((IList)parameters_).Insert(index, value);
        }

        public override void Remove(object value)
        {
            ((IList)parameters_).Remove(value);
        }

        public override void RemoveAt(int index)
        {
            ((IList)parameters_).RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            parameters_.RemoveAt(IndexOf(parameterName));
        }

        protected override DbParameter GetParameter(int index)
        {
            return parameters_[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return parameters_.First(x => x.ParameterName == parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            parameters_[index] = (StubDbParameter)value;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var index = IndexOf(parameterName);
            parameters_[index] = (StubDbParameter)value;
        }
    }
}
