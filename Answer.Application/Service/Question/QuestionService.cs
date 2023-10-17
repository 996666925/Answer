using System.Security.Claims;
using Answer.Application.Entity;
using Answer.Application.Enum;
using Answer.Application.Service.Question.Dto;
using Answer.Application.Service.User.Dto;

namespace Answer.Application.Service.Question;

/// <summary>
/// 问题服务
/// </summary>
public class QuestionService
(Repository<Entity.Question> questionRepository, Repository<Entity.Answer> answerRepository,
    Repository<UQMapping> mappingRepository) : IDynamicApiController
{
    /// <summary>
    /// 创建问题接口
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public async Task<long> Post([Required] QuestionDto question)
    {
        question.UserId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));
        var result = await questionRepository.InsertReturnSnowflakeIdAsync(question.Adapt<Entity.Question>());
        return result;
    }

    /// <summary>
    /// 查询问题接口
    /// </summary>
    /// <param name="questionId"></param>
    [Authorize]
    public async Task<Entity.Question> Get([Required] long questionId)
    {
        var question = await questionRepository.AsQueryable()
            .FirstAsync(u => u.Id == questionId);

        _ = question ?? throw Oops.Oh(ErrorCodeEnum.Q0001);

        var answers = await answerRepository.AsQueryable().ToTreeAsync(a => a.Answers, a => a.ParentId, question.Id);
        question.Answers = answers;
        return question;
    }


    /// <summary>
    /// 修改问题内容接口
    /// </summary>
    /// <param name="questionDto"></param>
    /// <returns></returns>
    /// <exception cref="AppFriendlyException"></exception>
    [Authorize]
    public async Task<string> Put([Required] QuestionDto questionDto)
    {
        var userId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));
        var question = await questionRepository.AsQueryable()
            .FirstAsync(u => u.Id == questionDto.Id && u.UserId == userId);


        _ = question ?? throw Oops.Oh(ErrorCodeEnum.Q0001);

        var result = await questionRepository.AsUpdateable(questionDto.Adapt<Entity.Question>())
            .IgnoreColumns(u => new
            {
                u.Id, u.UserId, u.CreateTime, u.Voters
            })
            .Where(u => u.Id == question.Id)
            .ExecuteCommandAsync();


        return result > 0 ? "修改成功" : "修改失败";
    }


    /// <summary>
    /// 随机获取n条问题接口,默认是10条
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<List<Entity.Question>> GetRandom(int count = 10)
    {
        var questionList = await questionRepository.AsQueryable().Take(count).OrderBy(st => SqlFunc.GetRandom())
            .ToListAsync();

        return questionList;
    }


    /// <summary>
    /// 模糊查询问题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<QuestionVo> GetSearch([Required, FromQuery] QuestionInput input)
    {
        RefAsync<int> totalCount = 0;
        var questions = await questionRepository.AsQueryable()
            .Where(it => it.Title.Contains(input.Content))
            .ToPageListAsync(input.PageIndex, input.PageSize, totalCount);

        return new()
        {
            Questions = questions,
            Total = totalCount
        };
    }


    /// <summary>
    /// 通过分类查询问题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<QuestionVo> GetSearchByType([Required, FromQuery] QuestionInput input)
    {
        RefAsync<int> totalCount = 0;
        var questions = await questionRepository.AsQueryable()
            .Where(it => SqlFunc.JsonArrayAny(it.Types, input.Content))
            .ToPageListAsync(input.PageIndex, input.PageSize, totalCount);

        return new()
        {
            Questions = questions,
            Total = totalCount
        };
    }


    /// <summary>
    /// 收藏问题接口
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    /// <exception cref="AppFriendlyException"></exception>
    [Authorize]
    public async Task<string> PostFollow(long questionId)
    {
        var userId = Convert.ToInt64(App.User.FindFirstValue(ClaimConst.UserId));
        var question = await questionRepository.AsQueryable()
            .FirstAsync(u => u.Id == questionId);

        _ = question ?? throw Oops.Oh(ErrorCodeEnum.Q0001);

        var result = await mappingRepository.InsertAsync(new() { UserId = userId, QuestionId = question.Id });

        return result ? "收藏成功" : "收藏失败";
    }
}