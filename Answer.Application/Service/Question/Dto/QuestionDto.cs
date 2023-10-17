using System.Text.Json.Serialization;

namespace Answer.Application.Service.Question.Dto;

public class QuestionDto
{
    public long Id { get; set; }

    [Required] public string Title { get; set; }

    [Required] public string Content { get; set; }

    public List<string> Types { get; set; } = new();

    [JsonIgnore] public long UserId { get; set; }
}