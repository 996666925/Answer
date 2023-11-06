namespace Answer.Application.Utils;

public static class KeyUtils
{
    public static string GetKey(string otherId)
    {
        var myId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));

        return $"{myId}_{otherId}";
    }

    public static string GetKey(long otherId)
    {
        var myId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));

        return $"{myId}_{Convert.ToString(otherId)}";
    }
}