



namespace Answer.Application.Entity;

public class EntityBase
{

    [SugarColumn(ColumnDescription = "主键Id", IsPrimaryKey = true, IsIdentity = false)]
    public long Id { get; set; }

    [SugarColumn(InsertServerTime = true, ColumnDescription = "创建时间", IsOnlyIgnoreUpdate = true)]
    public DateTime CreateTime { get; set; }

    [SugarColumn(UpdateServerTime = true, ColumnDescription = "更新时间")]
    public DateTime UpdateTime { get; set; }
}