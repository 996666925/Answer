using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
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
        services.AddJwt<JwtHandler>(
            jwtBearerConfigure:(options =>
            {

                options.Events = new()
                {
                    OnMessageReceived = context =>
                    {

                        var assessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(assessToken) &&
                            (path.StartsWithSegments("/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = assessToken;
                            
                        }

                        return Task.CompletedTask;
                    }
                };
            }));
        services.AddSqlsugar();
        services.AddSingleton<IUserIdProvider, UserIdProvider>();
        services.AddSignalR();
        services.AddCorsAccessor();

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
            endpoints.MapHubs();
            endpoints.MapControllers();
        });
    }
}