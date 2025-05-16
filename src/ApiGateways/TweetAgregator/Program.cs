using TweetAgregator.Models;
using TweetAgregator.Service;
using TweetAgregator.Services.Abstractions;
using TweetService;

var clientService = new ConfigClientService();
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.GetSection("ClientService").Bind(clientService);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IPublicationService, PublicationService>()
    .AddGrpcClient<Publication.PublicationClient>((services, options) =>
    {
        options.Address = new Uri($"{clientService.Address}:{clientService.Port.ToString()}");
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
