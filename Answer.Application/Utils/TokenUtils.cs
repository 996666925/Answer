using Answer.Application.Entity;
using Answer.Application.Service.Auth.Dto;
using Answer.Application.Service.User.Dto;

namespace Answer.Application.Utils;

public static class TokenUtils
{
    public static Object GetToken(this Entity.User user)
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

        return new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public static Object GetToken(this UserInput user)
    {
        return user.Adapt<User>().GetToken();
    }
}