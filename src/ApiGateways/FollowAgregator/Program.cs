using FollowAgregator.Model;
using FollowAgregator.Services;
using FollowAgregator.Services.Abstractions;
using FollowService.Protos.Follow;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var clientService = new ClientService();
builder.Configuration.GetSection("ClientService").Bind(clientService);
builder.Services.AddScoped<IFollowService, FollowerService>()
    .AddGrpcClient<Follower.FollowerClient>((services, options) =>
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
