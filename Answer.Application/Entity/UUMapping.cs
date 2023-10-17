namespace Answer.Application.Entity;


/// <summary>
/// 用户和用户之间的中间表,为了实现关注用户(是A关注了B)
/// </summary>
public class UUMapping
{
    [SugarColumn(IsPrimaryKey = true)]
    public long UserIdA { get; set; }
    [SugarColumn(IsPrimaryKey = true)]
    public long UserIdB { get; set; }
}