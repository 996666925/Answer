﻿using System.Text.Json.Serialization;
using Answer.Application.Enum;

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