using System.Security.Claims;
using Answer.Application.Entity;
using Answer.Application.Enum;
using Answer.Application.Service.User.Dto;


namespace Answer.Application.Service.User;

/// <summary>
/// 用户服务
/// </summary>
public class UserService
(Repository<Entity.User> userRepository, Repository<Entity.Question> questionRepository,
    Repository<UUMapping> mappingRepository) : IDynamicApiController
{
    /// <summary>
    /// 注册接口
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="AppFriendlyException"></exception>
    public async Task<string> Post([Required] UserInput input)
    {
        var user = await userRepository.GetFirstAsync(u => u.Account == input.Account);

        if (user != null)
        {
            throw Oops.Oh(ErrorCodeEnum.D0004);
        }

        input.Password = MD5Encryption.Encrypt(input.Password);
        await userRepository.InsertReturnSnowflakeIdAsync(input.Adapt<Entity.User>());


        return UserConst.RegisterOk;
    }

    /// <summary>
    /// 修改用户数据接口
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<string> Put([Required] UserInput input)
    {
        var user = input.Adapt<Entity.User>();
        var account = App.User.FindFirstValue(ClaimConst.Account);

        var result = await userRepository.AsUpdateable(user)
            .IgnoreColumns(u => new
            {
                u.Account, u.Password, u.AccountType, u.Status, u.LastLoginAddress, u.LastLoginIp, u.LastLoginTime, u.Id
            })
            .Where(u => u.Account == account)
            .ExecuteCommandAsync();

        return result > 0 ? "修改成功" : "修改失败";
    }

    /// <summary>
    /// 获取用户数据接口
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<UserOutput> Get([Required] long userId)
    {
        var user = await userRepository.AsQueryable().FirstAsync(u => u.Id == userId);

        return user.Adapt<UserOutput>();
    }

    /// <summary>
    /// 获取自己的数据
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public async Task<UserOutput> GetMe()
    {
        var userId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));
        var user = await userRepository.AsQueryable()
            .Where(u => u.Id == userId).Includes(u => u.FollowUsers)
            .Includes(u => u.FollowQuestions)
            .FirstAsync();

        return user.Adapt<UserOutput>();
    }

    /// <summary>
    /// 获取用户的问题
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public async Task<List<Entity.Question>> GetQuestion(long userId)
    {
        var questions = await questionRepository.AsQueryable().Where(u => u.UserId == userId).ToListAsync();

        return questions;
    }

    /// <summary>
    /// 关注用户接口
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="AppFriendlyException"></exception>
    [Authorize]
    public async Task<string> PostFollow(long userId)
    {
        var user = await userRepository.AsQueryable().FirstAsync(u => u.Id == userId);
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0001);

        var userIdA = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));
        var result = await mappingRepository.InsertAsync(new() { UserIdA = userIdA, UserIdB = userId });

        return result ? "关注成功" : "关注失败";
    }
}