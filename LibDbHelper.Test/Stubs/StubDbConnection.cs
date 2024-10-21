using System.Data;
using System.Data.Common;
using LibDbHelper.Test.Items;

namespace LibDbHelper.Test.Stubs;

public class StubDbConnection(Table table) : DbConnection
{
    public override string ConnectionString { get; set; }
    public override string Database { get; }
    public override string DataSource { get; }
    public override string ServerVersion { get; }
    public override ConnectionState State { get; }

    public override void ChangeDatabase(string databaseName)
    {
    }

    public override void Open()
    {
    }

    public override void Close()
    {
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        return new StubDbTransaction(this, isolationLevel);
    }

    protected override DbCommand CreateDbCommand()
    {
        return new StubDbCommand(table);
    }
}
