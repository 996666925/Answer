using System.Collections.Generic;
using System.Text.Json;
using Answer.Application.Service.Chat;
using Answer.Core;
using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace Answer.Web.Core;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConsoleFormatter();
        services.AddJwt<JwtHandler>();
        services.AddSqlsugar();
        services.AddSignalR();
        services.AddCorsAccessor();
        services.AddSingleton<IUserIdProvider, UserIdProvider>();
        services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(App.GetConfig<RedisConfiguration>("Redis"));
        services.AddControllers()
            .AddInjectWithUnifyResult().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.AddLongTypeConverters();
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // app.UseHttpsRedirection();

        app.UseRouting();
        app.UseRedisInformation();
        app.UseCorsAccessor();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseInject(string.Empty);

        app.UseEndpoints(endpoints =>
        {
        
            endpoints.MapControllers();
        });
    }
}