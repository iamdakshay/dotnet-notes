using AWS.Logger;
using AWS.Logger.SeriLog;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add HttpContext which will be used to access client ip, client id, correlation id
builder.Services.AddHttpContextAccessor();

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Information);
});

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    // add additional information using Enrichers
    loggerConfiguration
    .Enrich.WithClientIp()
    .Enrich.WithClientAgent()
    .Enrich.WithCorrelationId()
    // add if correlation id will be based on specified header value
    // .Enrich.WithCorrelationIdHeader(headerKey:"x-correlation-id")
    .Enrich.WithSensitiveDataMasking();

    // add console sink
    loggerConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3} {ClientIp} {ClientAgent}] {Message:lj}{NewLine}{Exception}");

    // add cloudwatch sink
    loggerConfiguration.WriteTo.AWSSeriLog(
        new AWSLoggerConfig()
        {
            Region = Amazon.RegionEndpoint.USEast1.SystemName,
            LogGroup = "loggingexample.api"
        });

    // add s3 sink
    loggerConfiguration.WriteTo.AmazonS3(
        "log.txt",
        "aksdnc-b1",
        Amazon.RegionEndpoint.USEast1,
        LogEventLevel.Information,
        outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3} {ClientIp} {ClientAgent}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: Serilog.Sinks.AmazonS3.RollingInterval.Minute);


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// add http request logging
app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
