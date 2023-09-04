using FluentValidation;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Caching;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Database.Repositories;
using ON.Mercury.Service.Hubs;
using ON.Mercury.Service.Middleware;
using ON.Mercury.Service.Models.Channels;
using ON.Mercury.Service.Services;
using ON.Mercury.Service.Validators;

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
                builder.WithOrigins("http://127.0.0.1:5173", "http://127.0.0.1:5173/", "http://localhost:5173")
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
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddScoped<IValidator<CreateOrUpdateChannel>, CreateOrUpdateChannelValidator>();
        services.AddJwtAuthentication();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("mercury", new OpenApiInfo { Title = "Mercury API" });
        });
        services.AddSignalR().AddNewtonsoftJsonProtocol(opts =>
        {
            opts.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        services.AddDbContext<PostgresContext>(opts =>
        {
            opts.UseNpgsql(Configuration.GetConnectionString("Postgres"));
            opts.EnableDetailedErrors();
            opts.EnableSensitiveDataLogging();
        });
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "api/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/api/mercury/swagger.json", "Mercury API");
            c.RoutePrefix = "api/mercury";
        });
        if (env.IsDevelopment())
            Program.IsDevelopment = true;
        app.UseRouting();
        app.UseCors();
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseJwtAuthentication();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ChatHub>("/api/mercury/hub");
            endpoints.MapGrpcService<ChatService>();
            endpoints.MapGrpcService<ClaimsService>();
            endpoints.MapGrpcService<AuditLogService>();
            endpoints.MapControllers();
        });
    }
}