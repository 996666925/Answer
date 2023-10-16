namespace Answer.Application.Entity;

public class EntityBase
{
    [SugarColumn(ColumnName = "Id", ColumnDescription = "主键Id", IsPrimaryKey = true, IsIdentity = false)]
    public  long Id { get; set; }
}