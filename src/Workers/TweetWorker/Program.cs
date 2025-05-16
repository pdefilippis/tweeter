using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TweeterDB.Context;
using TweetWorker.Aplication.Abstractions;
using TweetWorker.Aplication.Consumers;
using TweetWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TweeterDBConnection");

builder.Services.AddHostedService<TweetConsumer>();
builder.Services.AddScoped<ITweetRepository, TweetRepository>();
builder.Services.AddDbContext<TweetContext>(options => options.UseNpgsql(connectionString));

var host = builder.Build();
host.Run();