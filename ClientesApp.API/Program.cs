using ClientesApp.API.Extensions;
using ClientesApp.Infra.Data.SqlServer.Extensions;
using ClientesApp.Domain.Extensions;
using ClientesApp.Application.Extensions;
using ClientesApp.Infra.Data.MongoDB.Extensions;
using ClientesApp.Infra.Messages.Extensions;
using ClientesApp.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(map => { map.LowercaseUrls = true; });
builder.Services.AddSwaggerConfig();
builder.Services.AddEntityFramework(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddMongoDB(builder.Configuration);
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddCorsConfig(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<NotFoundExceptionMiddleware>();

app.UseCorsConfig();
app.UseSwaggerConfig();
app.UseAuthorization();
app.MapControllers();

app.Run();
