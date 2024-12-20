﻿using LibDbHelper.Test.Items;
using System.Data;
using System.Data.Common;

namespace LibDbHelper.Test.Stubs
{
    public class StubDbCommand : DbCommand
    {
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection { get; } = new StubDbParameterCollection();
        protected override DbTransaction DbTransaction { get; set; }

        private readonly Table table_;

        public StubDbCommand(Table table)
        {
            table_ = table;
        }

        public override void Cancel()
        {
        }

        public override int ExecuteNonQuery()
        {
            return 1;
        }

        public override object ExecuteScalar()
        {
            return table_.Count > 0 ? table_[0][0] : null;
        }

        public override void Prepare()
        {
        }

        protected override DbParameter CreateDbParameter()
        {
            return new StubDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new StubDbDataReader(table_);
        }
    }
}
