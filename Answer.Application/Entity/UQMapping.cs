namespace Answer.Application.Entity;

/// <summary>
/// 用户和问题之间的中间表,为了实现收藏问题
/// </summary>
public class UQMapping
{
    [SugarColumn(IsPrimaryKey = true)]
    public long UserId { get; set; }
    [SugarColumn(IsPrimaryKey = true)]
    public long QuestionId { get; set; }
}