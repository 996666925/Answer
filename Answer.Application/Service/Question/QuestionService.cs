using System.Security.Claims;
using Answer.Application.Entity;
using Answer.Application.Enum;
using Answer.Application.Service.Question.Dto;
using Answer.Application.Service.User.Dto;
using Furion.LinqBuilder;

namespace Answer.Application.Service.Question;

/// <summary>
/// 问题相关接口
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
    /// 查询问题详情接口
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
        await questionRepository.AsUpdateable().Where(p => p.Id == questionId)
            .SetColumns((q) => q.Count == q.Count + 1).ExecuteCommandAsync();

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
    /// 查询问题列表，0是根据活跃，1是根据发布时间，2是根据浏览数量，3是未回答，默认是10条
    /// </summary>
    /// <param name="order"></param>   
    /// <param name="count"></param>
    /// <param name="pageIndex"></param>
    /// <returns></returns>
    public async Task<Object> GetList([Required] OrderEnum order, int count = 10, int pageIndex = 0)
    {
        RefAsync<int> totalCount = 0;

        var questionList = order switch
        {
            OrderEnum.Active =>
                await questionRepository.AsQueryable().OrderBy(st => st.AnswerCount, OrderByType.Desc)
                    .ToPageListAsync(pageIndex, count, totalCount),

            OrderEnum.Latest =>
                await questionRepository.AsQueryable().OrderBy(st => st.UpdateTime, OrderByType.Desc)
                    .ToPageListAsync(pageIndex, count, totalCount),

            OrderEnum.Frequent =>
                await questionRepository.AsQueryable().OrderBy(st => st.Count, OrderByType.Desc)
                    .ToPageListAsync(pageIndex, count, totalCount),

            OrderEnum.Unanswered =>
                await questionRepository.AsQueryable().Where(q => q.AnswerCount <= 0)
                    .ToPageListAsync(pageIndex, count, totalCount),

            _ => new()
        };


        return new
        {
            Questions = questionList,
            Total = totalCount
        };
    }


    /// <summary>
    /// 模糊查询问题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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
    /// 通过标签查询问题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<QuestionVo> GetSearchByType([Required, FromQuery] QuestionInput input)
    {
        RefAsync<int> totalCount = 0;
        List<Entity.Question> questions = await questionRepository.AsQueryable()
            .WhereIF(!input.Content.IsNullOrEmpty(), it => SqlFunc.JsonArrayAny(it.Types, input.Content))
            .WhereIF(input.Content.IsNullOrEmpty(), it => !SqlFunc.JsonLike(it.Types, "[]"))
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

    /// <summary>
    /// 点赞接口
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [Authorize]
    public async Task<string> PutGood(long questionId)
    {
        await questionRepository.AsUpdateable().Where(p => p.Id == questionId)
            .SetColumns((q) => q.Voters == q.Voters + 1).ExecuteCommandAsync();

        return "点赞成功";
    }
}