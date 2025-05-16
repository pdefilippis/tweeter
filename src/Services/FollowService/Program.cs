using FollowService.Aplication.Abstractions;
using FollowService.Aplication.Services;
using FollowService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddScoped<IMessagePublisher, FollowPublisher>();

var app = builder.Build();
app.MapGrpcService<FollowServices>();
app.MapGet("/", () => "Follow Service Iniciado");

app.Run();
