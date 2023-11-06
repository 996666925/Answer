using Answer.Application.Service.Answer.Dto;
using Answer.Application.Utils;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Answer.Application.Service.Answer;

/// <summary>
/// 评论服务
/// </summary>
public class AnswerService
(Repository<Entity.Answer> answerRepository, Repository<Entity.Question> questionRepository,
    IRedisDatabase redisDatabase) : IDynamicApiController
{
    /// <summary>
    /// 创建评论接口
    /// </summary>
    /// <param name="answerDto">parent填问题Id或评论Id,写那个就是评论哪个</param>
    /// <returns></returns>
    public async Task<long> Post([Required] AnswerDto answerDto)
    {
        answerDto.UserId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));

        var id = await answerRepository.InsertReturnSnowflakeIdAsync(answerDto.Adapt<Entity.Answer>());
        await questionRepository.AsUpdateable().Where(p => p.Id == answerDto.ParentId)
            .SetColumns((q) => q.AnswerCount == q.AnswerCount + 1).ExecuteCommandAsync();

        return id;
    }

    /// <summary>
    /// 删除评论接口
    /// </summary>
    /// <param name="answerId"></param>
    /// <returns></returns>
    /// <exception cref="AppFriendlyException"></exception>
    public async Task<string> Delete(long answerId)
    {
        var userId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));
        var answer = await answerRepository.AsQueryable().FirstAsync(a => a.Id == answerId && a.UserId == userId);

        _ = answer ?? throw Oops.Oh(ErrorCodeEnum.Q0001);

        var answers = await answerRepository.AsQueryable().ToChildListAsync(a => a.ParentId, answer.Id);


        var result = await answerRepository.AsDeleteable().In(answers.Select(a => a.Id)).ExecuteCommandAsync();
        return result > 0 ? "删除成功" : "删除失败";
    }

    /// <summary>
    /// 点赞接口
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<string> PutGood(long id)
    {
        await answerRepository.AsUpdateable().Where(p => p.Id == id)
            .SetColumns((q) => q.Voters == q.Voters + 1).ExecuteCommandAsync();

        string key = KeyUtils.GetKey(id);

        var isGood = await redisDatabase.HashGetAsync<bool>("AnswerGoods", key);
        if (isGood)
        {
            throw Oops.Oh(ErrorCodeEnum.Q0003);
        }

        await redisDatabase.HashSetAsync("AnswerGoods", key, true);
        return "点赞成功";
    }

    /// <summary>
    /// 检查是否已经点赞
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<Object> GetCheckGood(long id)
    {
        string key = KeyUtils.GetKey(id);

        var isGood = await redisDatabase.HashGetAsync<bool>("AnswerGoods", key);

        return new
        {
            isGood
        };
    }

    /// <summary>
    /// 取消点赞
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<string> PutCancelGood(long id)
    {
        string key = KeyUtils.GetKey(id);

        var isGood = await redisDatabase.HashGetAsync<bool>("AnswerGoods", key);

        if (isGood)
        {
            await redisDatabase.HashSetAsync("AnswerGoods", key, false);

            await answerRepository.AsUpdateable().Where(p => p.Id == id)
                .SetColumns((q) => q.Voters == q.Voters - 1).ExecuteCommandAsync();
        }


        return "取消点赞成功";
    }
}