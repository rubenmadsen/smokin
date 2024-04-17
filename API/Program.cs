using API.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StopSmoking",
                                         Version = "v1",
                                         Description = "This ASP.NET Core Web API template serves as a foundation for retrieving smoking-related data. It's designed to give an introduction to building APIs using .NET Core.\r\n\r\nYour task is to implement the missing endpoints within this template. We encourage you to add and modify functions as they see fit to enhance the API's functionality.\r\n\r\nThe ultimate objective is to utilize this API as a backend for the web application you will build using Vue.js."
    });
});

var config = builder.Configuration;

//Custom logger
builder.Services.AddLogging(builder =>
{
    builder.AddProvider(new DatabaseLoggerProvider((category, level) => level >= LogLevel.Information, config.GetConnectionString("DBcon")));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
    options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var app = builder.Build();

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
