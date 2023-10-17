using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Answer.Application.Service.Chat;

public class ChatService(IRedisDatabase redisDatabase) : IDynamicApiController
{
    public async void Put(long userId)
    {
        var key = $"{userId}_{App.User.FindFirstValue(ClaimConst.UserId)}";


        await redisDatabase.HashSetAsync(key, "count", 123213);
    }
}