using System.Text.Json.Nodes;

namespace Answer.Application.Entity;

public class Question : EntityBase
{
    [SugarColumn(ColumnDescription = "标题", Length = 512)]
    public string Title { get; set; }

    [SugarColumn(ColumnDescription = "内容", Length = 2048)]
    public string Content { get; set; }

    [SugarColumn(ColumnDescription = "用户ID")]
    public long UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public User User { get; set; }

    [SugarColumn(IsJson = true)] public List<string> Types { get; set; } 
    [SugarColumn(IsIgnore = true)] public List<Answer> Answers { get; set; }

    [SugarColumn(ColumnDescription = "点赞数量")]
    public int Voters { get; set; }
}