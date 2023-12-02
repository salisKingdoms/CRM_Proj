

using System.Text.Json.Serialization;
using WS_CRM.Helper;
using AutoMapper;
using WS_CRM.Feature.Activity.dao;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure strongly typed settings object
    services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
   // var appConfig = configh
    // configure DI for application services
    services.AddSingleton<DataContext>();
    services.AddScoped<IActivityRepo, ActivityRepo>();
}

var app = builder.Build();

// ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

// configure HTTP request pipeline
//{
//    // global cors policy
//    app.UseCors(x => x
//        .AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader());

//    // global error handler
//    app.UseMiddleware<ErrorHandlerMiddleware>();

//    app.MapControllers();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.Run();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();