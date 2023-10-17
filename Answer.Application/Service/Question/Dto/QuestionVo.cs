using System.Text.Json.Serialization;

namespace Answer.Application.Service.Question.Dto;

public class QuestionVo
{
    public List<Entity.Question> Questions { get; set; }

    public int Total { get; set; }
}