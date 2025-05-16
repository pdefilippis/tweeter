using CacheService.Abstractions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TimelineService.Aplication.Abstraction;
using TimelineService.Infrastructure;
using TimelineService.Models;
using TweeterDB.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<ITweetRepository, TweetRepository>();
builder.Services.AddScoped<ICacheService, CacheService.CacheService>();

#region Reddis Connection
var redisSettings = new RedisConnectionSettings();
builder.Configuration.GetSection("RedisConnection").Bind(redisSettings);

var configurationOptions = new ConfigurationOptions {
    User = redisSettings.User,
    Password = redisSettings.Password,
    AbortOnConnectFail = redisSettings.AbortOnConnectFail
};

configurationOptions.EndPoints.Add(redisSettings.Host, redisSettings.Port);
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configurationOptions));
#endregion

#region Db Connection
var connectionString = builder.Configuration.GetConnectionString("TweeterDBConnection");
builder.Services.AddDbContext<TweetContext>(options =>
    options.UseNpgsql(connectionString));
#endregion

var app = builder.Build();
app.MapGrpcService<TimelineService.Aplication.Service.TimelineService>();
app.MapGet("/", () => "TimelineService Iniciado");

app.Run();
