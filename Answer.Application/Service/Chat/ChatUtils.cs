namespace Answer.Application.Service.Chat;

public static class ChatUtils
{
    public static string GetKey(string otherId)
    {
        var userId = Convert.ToInt64(otherId);
        var myId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));

        return userId < myId ? $"{userId}_{myId}" : $"{myId}_{userId}";
    }
    
    public static string GetKey(long userId)
    { ;
        var myId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));

        return userId < myId ? $"{userId}_{myId}" : $"{myId}_{userId}";
    }
}