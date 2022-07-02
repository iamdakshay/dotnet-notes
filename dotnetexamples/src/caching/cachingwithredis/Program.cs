using cachingwithredis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "dotnet5-0001-002.dotnet5.wrx7qm.use1.cache.amazonaws.com:6380,ssl=true,user=akshay,password=SWAMI@DATTAC3479";
});
//builder.Services.AddScoped<IDistributedCache>(c =>
//{
//    var options = c.GetRequiredService<IOptions<RedisCacheOptions>>();

//    options.Value.Configuration = "dotnet5-0001-002.dotnet5.wrx7qm.use1.cache.amazonaws.com,ssl=true,user=akshay,password=SWAMI@DATTAC3479";

//    return new RedisDistributedCache(options);
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
