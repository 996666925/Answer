using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Answer.Application.Service.Chat;



public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}


public class ChatHub(IRedisDatabase redisDatabase) :Hub<IChatClient>
{
    
    
    
    
    
}