
using SqlSugar;
using Answer.Application.Entity;

SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        ConnectionString = "Server=localhost;Database=answer;Uid=root;Pwd=1152207863;",
        DbType = DbType.MySql,
        IsAutoCloseConnection = true
    },
    db => { });


db.DbMaintenance.CreateDatabase();
db.CodeFirst.InitTables<User>();