using Answer.Application.Service.Answer.Dto;

namespace Answer.Application.Service.Answer;

/// <summary>
/// 评论服务
/// </summary>
public class AnswerService(Repository<Entity.Answer> answerRepository) : IDynamicApiController
{
    /// <summary>
    /// 创建评论接口
    /// </summary>
    /// <param name="answerDto">parent填问题Id或评论Id,写那个就是评论哪个</param>
    /// <returns></returns>
    public async Task<long> Post([Required] AnswerDto answerDto)
    {
        answerDto.UserId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));

        return await answerRepository.InsertReturnSnowflakeIdAsync(answerDto.Adapt<Entity.Answer>());
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
}