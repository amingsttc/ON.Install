using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Mercury.Service.Caching;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Database.Repositories;
using ON.Mercury.Service.Hubs;
using ON.Mercury.Service.Services;

namespace ON.Mercury.Service;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddLogging();
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.UseCamelCasing(false);
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5173")
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .AllowCredentials();
            });
        });
        services.AddDistributedMemoryCache();
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ICachingService, CachingService>();
        services.AddScoped<MemberRepository>();
        services.AddScoped<RoleRepository>();
        services.AddScoped<ChannelRepository>();
        services.AddJwtAuthentication();
        services.AddSignalR().AddNewtonsoftJsonProtocol(opts =>
        {
            opts.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        services.AddDbContext<PostgresContext>(opts =>
        {
            opts.UseNpgsql(Configuration.GetConnectionString("Postgres"));
        });
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            Program.IsDevelopment = true;
        app.UseRouting();
        app.UseJwtAuthentication();
        app.UseCors();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<ChannelService>();
            endpoints.MapGrpcService<RoleService>();
            endpoints.MapGrpcService<MemberService>();
            endpoints.MapHub<EventHub>("/api/hub");
            endpoints.MapControllers();
        });
    }
}