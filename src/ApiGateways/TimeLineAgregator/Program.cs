using TimeLineAgregator.Model;
using TimeLineAgregator.Services.Abstractions;
using TimelineService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var clientService = new ConfigClientService();
builder.Configuration.GetSection("ClientService").Bind(clientService);
builder.Services
    .AddScoped<ITimelineService, TimeLineAgregator.Services.TimelineService>()
    .AddGrpcClient<TweetsTimeline.TweetsTimelineClient>((services, options) =>
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
