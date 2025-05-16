using FollowWorker.Aplication.Abstractions;
using FollowWorker.Aplication.Consumers;
using FollowWorker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TweeterDB.Context;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TweeterDBConnection");

builder.Services.AddHostedService<FollowConsumer>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddDbContext<TweetContext>(options => options.UseNpgsql(connectionString));

var host = builder.Build();
host.Run();