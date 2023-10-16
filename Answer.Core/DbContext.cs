using Furion;
using SqlSugar;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Answer.Core;

/// <summary>
/// 数据库上下文对象
/// </summary>
public static class DbContext
{
    /// <summary>
    /// SqlSugar 数据库实例
    /// </summary>
    public static readonly SqlSugarScope Instance = new(
        // 读取 appsettings.json 中的 ConnectionConfigs 配置节点
        App.GetConfig<List<ConnectionConfig>>("ConnectionConfigs")
        , db =>
        {
            
            
            // 这里配置全局事件，比如拦截执行 SQL
        });
    
    public static void AddSqlsugar(this IServiceCollection services)
    {
        services.AddSingleton<ISqlSugarClient>(Instance);
        services.AddScoped(typeof(Repository<>));
    }
}

public class Repository<T> : SimpleClient<T> where T : class, new()
{
    public Repository(ISqlSugarClient context) : base(context)
    {
        base.Context = context;
    }
}