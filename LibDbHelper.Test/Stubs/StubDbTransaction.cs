using System.Data;
using System.Data.Common;

namespace LibDbHelper.Test.Stubs;

public class StubDbTransaction(StubDbConnection connection, IsolationLevel isolationLevel) : DbTransaction
{
    protected override DbConnection DbConnection { get; } = connection;
    public override IsolationLevel IsolationLevel { get; } = isolationLevel;

    public override void Commit()
    {
    }

    public override void Rollback()
    {
    }
}
