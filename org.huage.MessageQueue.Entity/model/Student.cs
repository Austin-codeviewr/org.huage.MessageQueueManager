using SqlSugar;

namespace org.huage.MessageQueue.Entity.model;

[SugarTable("t_user")]
public class Student
{
    public Student()
    {
    }

    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //是主键, 还是标识列
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }
}