using Answer.Application.Enum;

namespace Answer.Application.Entity;

/// <summary>
/// 系统用户表
/// </summary>
[SugarTable(null, "系统用户表")]
public class User : EntityBase
{
    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnDescription = "账号", Length = 32)]
    [Required, MaxLength(32), MinLength(6)]
    public string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [SugarColumn(ColumnDescription = "密码", Length = 512)]
    [MaxLength(512)]
    public string Password { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 32, IsNullable = true)]
    [MaxLength(32)]
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(ColumnDescription = "头像", Length = 512, IsNullable = true)]
    [MaxLength(512)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别-男_1、女_2
    /// </summary>
    [SugarColumn(ColumnDescription = "性别")]
    public GenderEnum Sex { get; set; } = GenderEnum.Male;

    /// <summary>
    /// 年龄
    /// </summary>
    [SugarColumn(ColumnDescription = "年龄", IsNullable = true)]
    public int Age { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnDescription = "出生日期", IsNullable = true)]
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 民族
    /// </summary>
    [SugarColumn(ColumnDescription = "民族", Length = 32, IsNullable = true)]
    [MaxLength(32)]
    public string? Nation { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [SugarColumn(ColumnDescription = "手机号码", Length = 16, IsNullable = true)]
    [MaxLength(16)]
    public string? Phone { get; set; }


    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "邮箱", Length = 64, IsNullable = true)]
    [MaxLength(64)]
    public string? Email { get; set; }


    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;


    /// <summary>
    /// 个人简介
    /// </summary>
    [SugarColumn(ColumnDescription = "个人简介", Length = 512, IsNullable = true)]
    [MaxLength(512)]
    public string? Introduction { get; set; }


    /// <summary>
    /// 账号类型
    /// </summary>
    [SugarColumn(ColumnDescription = "账号类型")]
    public UserTypeEnum AccountType { get; set; } = UserTypeEnum.NormalUser;


    /// <summary>
    /// 最新登录Ip
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录Ip", Length = 256, IsNullable = true)]
    [MaxLength(256)]
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 最新登录地点
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录地点", Length = 128, IsNullable = true)]
    [MaxLength(128)]
    public string? LastLoginAddress { get; set; }

    /// <summary>
    /// 最新登录时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录时间", IsNullable = true)]
    public DateTime? LastLoginTime { get; set; }
}