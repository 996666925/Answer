namespace Answer.Application.Entity;

public class Answer : EntityBase
{
    [SugarColumn(ColumnDescription = "父类Id(问题Id或评论Id)")]
    public long ParentId { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<Answer> Answers { get; set; }
    
    [SugarColumn(ColumnDescription = "用户Id")]
    public long UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public User User { get; set; }

    [SugarColumn(ColumnDescription = "评论内容")]
    public string Content { get; set; }
    
    [SugarColumn(ColumnDescription = "点赞数量")]

    public int Voters { get; set; } = 0;
    
}