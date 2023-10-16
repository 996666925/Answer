namespace Answer.Application.Service.User.Dto;

public class UserInput
{
    /// <summary>
    /// 账号
    /// </summary>
    /// <example>admin</example>
    [Required(ErrorMessage = "账号不能为空"), MinLength(5, ErrorMessage = "账号不能少于5个字符")]
    public string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <example>123456</example>
    [Required(ErrorMessage = "密码不能为空"), MinLength(5, ErrorMessage = "密码不能少于5个字符")]
    public string Password { get; set; }

    
}