using Answer.Application.Service.Chat.Dto;
using Furion.InstantMessaging;
using Furion.Logging.Extensions;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Answer.Application.Service.Chat;

public interface IChatClient
{
    Task ReceiveMessage(ChatInfo info);
}

[MapHub("/chat")]
[Authorize]
public class ChatHub(IRedisDatabase redisDatabase) : Hub<IChatClient>
{

    public async Task SendMessage(string userId, string message)
    {
        var key = ChatUtils.GetKey(userId);


        var list = await redisDatabase.HashGetAsync<List<ChatInfo>>(key, "list") ?? new List<ChatInfo>();


        var chatInfo = new ChatInfo()
        {
            Content = message,
            Time = DateTime.Now,
            UserId = userId
        };
        list.Add(chatInfo);

        await redisDatabase.HashSetAsync(key, "list", list);

        await Clients.User(userId).ReceiveMessage(chatInfo);
    }
}