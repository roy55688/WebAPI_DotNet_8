using Serilog;
using System.Reflection;
using WebAPISample.Applications;
using WebAPISample.Services;
using UBCP_WebAPISample.Middlewares;
using WebAPISample.BackgroundServices;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPISample;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Serilog.Debugging.SelfLog.Enable(msg =>
{
    Console.WriteLine("Serilog 發生錯誤:" + msg);
});

try
{
    // Add services to the container.
    builder.Services.AddSingleton<ISignatureService, SignatureService>();
    builder.Services.AddSingleton<ICheckService, CheckService>();
    builder.Services.AddScoped<IIDCreateService, IDCreateService>();
    builder.Services.AddScoped<IResponseService, ResponseService>();

    // Add applications to the container.
    builder.Services.AddScoped<IAccountApplication, AccountApplication>();
    builder.Services.AddScoped<ISignatureApplication, SignatureApplication>();

    // Add backgroundServices to the container.
    builder.Services.AddHostedService<LogRestoreBackgroundService>();

    //Add SwaggerOptions
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    //Add Version
    //Learn more : https://www.milanjovanovic.tech/blog/api-versioning-in-aspnetcore
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
        .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });


    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    });

    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            //增加不同版本的Describe
            var descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
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

