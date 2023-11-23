using Serilog;
using Serilog.Events;
using System.Reflection;
using WebAPISample.Applications;
using WebAPISample.Services;
using WebAPISample;
using WebAPISample.Models;
using UBCP_WebAPISample.Middlewares;
using WebAPISample.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Serilog.Debugging.SelfLog.Enable(msg =>
{
    Console.WriteLine("Serilog �o�Ϳ��~:" + msg);
});

try
{
    // Add services to the container.
    builder.Services.AddScoped<IIDCreateService, IDCreateService>();
    builder.Services.AddSingleton<ISignatureService, SignatureService>();
    builder.Services.AddScoped<ISignatureApplication, SignatureApplication>();
    builder.Services.AddScoped<IResponseService, ResponseService>();

    builder.Services.AddHostedService<LogRestoreBackgroundService>();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<LogMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

