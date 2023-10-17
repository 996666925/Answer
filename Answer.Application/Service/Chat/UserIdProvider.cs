using Microsoft.AspNetCore.SignalR;

namespace Answer.Application.Service.Chat;

public class UserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirstValue(ClaimConst.UserId);
    }
}