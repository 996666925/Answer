namespace Answer.Application.Service.Question.Dto;

public class QuestionInput
{
    public string Content { get; set; }

    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}