using System.Text.Json.Serialization;
using Answer.Application.Enum;

namespace Answer.Application.Service.User.Dto;

public class UserOutput
{
    /// <summary>
    /// 账号
    /// </summary>
    /// <example>admin</example>
    public string Account { get; set; }
    

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别-男_1、女_2
    /// </summary>
    public int Sex { get; set; } = 1;

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 民族
    /// </summary>
    public string? Nation { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? Phone { get; set; }


    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }


    /// <summary>
    /// 个人简介
    /// </summary>
    public string? Introduction { get; set; }
    
    public List<Entity.Question> FollowQuestions { get; set; }
    
    public List<UserOutput> FollowUsers { get; set; }
}