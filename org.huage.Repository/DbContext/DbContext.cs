using SqlSugar;

namespace org.huage.Repository.DbContext;

public class DbContext <T> where T : class, new()
{
    /*public DbContext()
    {
        Db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "server=.;uid=sa;pwd=sa123;database=test",
            DbType = DbType.MySql,
            InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
            IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了

        });
    }
    //注意：不能写成静态的
    public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
    public SimpleClient<T> SimpleDb { get { return new SimpleClient<T>(Db); } }//用来处理t表的常用操作*/

}