using FluentValidation;
using TweetService.Aplication.Abstractions;
using TweetService.Aplication.Services;
using TweetService.Aplication.Validations;
using TweetService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddScoped<IMessagePublisher, TweetPublisher>();
builder.Services.AddTransient<IValidator<TweetService.MessageRequest>, MessageRequestValidator>();

var app = builder.Build();
app.MapGrpcService<PublicationService>();
app.MapGet("/", () => "TweetService Iniciado");

app.Run();
