namespace Answer.Application.Enum;


/// <summary>
/// 系统错误码
/// </summary>
[ErrorCodeType]
[Description("系统错误码")]
public enum ErrorCodeEnum
{
    /// <summary>
    /// 用户名不存在
    /// </summary>
    [ErrorCodeItemMetadata("用户名不存在")]
    D0001,
    /// <summary>
    /// 账号已被冻结
    /// </summary>
    [ErrorCodeItemMetadata("账号已被冻结")]
    D0002,
    
    /// <summary>
    /// 密码不正确
    /// </summary>
    [ErrorCodeItemMetadata("密码不正确")]
    D0003,
    
    /// <summary>
    /// 用户名已存在
    /// </summary>
    [ErrorCodeItemMetadata("用户名已存在")]
    D0004,
    
    /// <summary>
    /// 问题不存在
    /// </summary>
    [ErrorCodeItemMetadata("问题不存在")]
    Q0001,
    
    /// <summary>
    /// 评论不存在
    /// </summary>
    [ErrorCodeItemMetadata("评论不存在")]
    Q0002,
}