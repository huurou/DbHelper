using System.Data;
using System.Data.Common;

namespace LibDbHelper.Test.Stubs
{
    public class StubDbTransaction : DbTransaction
    {
        protected override DbConnection DbConnection { get; }
        public override IsolationLevel IsolationLevel { get; }

        public StubDbTransaction(DbConnection dbConnection, IsolationLevel isolationLevel)
        {
            DbConnection = dbConnection;
            IsolationLevel = isolationLevel;
        }

        public override void Commit()
        {
        }

        public override void Rollback()
        {
        }
    }
}
