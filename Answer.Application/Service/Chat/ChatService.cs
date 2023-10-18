using Furion.ClayObject;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Answer.Application.Service.Chat;

public class ChatService(IRedisDatabase redisDatabase) : IDynamicApiController
{
    public async void Put(long userId)
    {
        var key = ChatUtils.GetKey(userId);


        await redisDatabase.HashSetAsync(key, "count", 0);
    }


    public async Task<Object> Get(long userId)
    {
        var key = ChatUtils.GetKey(userId);


        var count = await redisDatabase.HashGetAsync<int>(key, "count");

        var list = await redisDatabase.HashGetAsync<List<ChatInfo>>(key, "list");

        return new
        {
            Count = count,
            Value = list
        };
    }
    
    
}