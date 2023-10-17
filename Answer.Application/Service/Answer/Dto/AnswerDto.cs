using System.Text.Json.Serialization;

namespace Answer.Application.Service.Answer.Dto;

public class AnswerDto
{
    [Required]
    public long ParentId { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [JsonIgnore]
    public long UserId { get; set; }
}