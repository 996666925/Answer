using Answer.Application.Const;
using Answer.Application.Enum;
using Answer.Application.Service.Auth.Dto;
using Answer.Core;

namespace Answer.Application.Service.Auth;

/// <summary>
/// 系统登录授权服务
/// </summary>
public class AuthService
    (Repository<Entity.User> userRepository, IHttpContextAccessor httpContextAccessor) : IDynamicApiController
{
    /// <summary>
    /// 登录接口
    /// </summary>
    /// <returns></returns>
    public async Task<LoginOutput> PostLogin([Required] LoginInput input)
    {
        var user = await userRepository.AsQueryable().FirstAsync(u => u.Account == input.Account);

        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0001);


        // 账号是否被冻结
        if (user.Status == StatusEnum.Disable)
        {
            throw Oops.Oh(ErrorCodeEnum.D0002);
        }

        // 密码是否正确

        if (user.Password != MD5Encryption.Encrypt(input.Password))
            throw Oops.Oh(ErrorCodeEnum.D0003);

        var result = CreateToken(user);

        httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(result.AccessToken, result.RefreshToken);

        return result;
    }


    /// <summary>
    /// 生成Token令牌
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [NonAction]
    public static LoginOutput CreateToken(Entity.User user)
    {
        // 生成Token令牌
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            { ClaimConst.UserId, Convert.ToString(user.Id) },
            { ClaimConst.Account, user.Account },
            { ClaimConst.AccountType, user.AccountType },
        });

        // 生成刷新Token令牌
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);
        
        return new LoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}