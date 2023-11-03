using Furion.ClayObject;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Answer.Application.Service.Chat;


/// <summary>
/// 用户通信相关接口
/// </summary>
/// <param name="redisDatabase"></param>
public class ChatService(IRedisDatabase redisDatabase) : IDynamicApiController
{
    /// <summary>
    /// 告诉服务器，用户已经查看最新消息
    /// </summary>
    /// <param name="userId"></param>
    public async void Put(long userId)
    {
        var key = ChatUtils.GetKey(userId);


        await redisDatabase.HashSetAsync(key, "count", 0);
    }


    /// <summary>
    /// 获取用户的消息记录，参数是对方的id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
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