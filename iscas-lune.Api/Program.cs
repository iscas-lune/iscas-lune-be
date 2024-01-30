using dotenv.net;
using iscas_lune_api.CrossCutting;
using iscas_lune_api.Workers;
using iscaslune.Api;
using iscaslune.Api.CrossCutting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InjectDomain();
builder.Services.InjectInfrastructure();
builder.Services.InjectApplication();
builder.Services.InjectJwt();
builder.Services.InjectCors();
builder.Services.InjectHttpClient();
builder.Services.AddHostedService<PedidosEmAbertoWorker>();

var app = builder.Build();

if (EnvironmentVariable.GetVariable("AMBIENTE").Equals("develop"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
