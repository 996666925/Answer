using System.Text.Json.Serialization;
using Answer.Application.Enum;

namespace Answer.Application.Service.User.Dto;

public class UserInfo
{
    /// <summary>
    /// 昵称
    /// </summary>
    [MaxLength(32)]
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [MaxLength(512)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别-男_1、女_2
    /// </summary>
    public GenderEnum Sex { get; set; } = GenderEnum.Male;

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
    [MaxLength(32)]
    public string? Nation { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [MaxLength(16)]
    public string? Phone { get; set; }


    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(64)]
    public string? Email { get; set; }


    /// <summary>
    /// 个人简介
    /// </summary>
    [MaxLength(512)]
    public string? Introduction { get; set; }
}