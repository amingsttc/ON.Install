using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Mercury.Service.Database;
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
        services.AddGrpcHttpApi();
        services.AddLogging();
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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<ChannelService>();
            endpoints.MapGrpcService<RoleService>();
            endpoints.MapGrpcService<MemberService>();
            endpoints.MapHub<EventHub>("/api/v1/hub");
        });
    }
}