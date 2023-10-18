
using Furion.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Furion.DataEncryption;

namespace Answer.Web.Core;

public class JwtHandler : AppAuthorizeHandler
{
    
    public override async Task HandleAsync(AuthorizationHandlerContext context)
    {
        
        // 自动刷新 token
        if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
        {
            await AuthorizeHandleAsync(context);
        }
        else context.Fail();    // 授权失败
        
    }
    public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
    {
        // 这里写您的授权判断逻辑，授权通过返回 true，否则返回 false

        return Task.FromResult(true);
    }
}
