using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.BusinessLayer.Concrete;
using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using OneDose.FirstProject.DataAccessLayer.EntityFramework;
using OneDose.FirstProject.WebAPI.Caching;
using OneDose.FirstProject.WebAPI.Middlewares;
using OneDose.FirstProject.WebAPI.Model;
using OneDose.FirstProject.WebAPI.Security;
using OneDose.FirstProject.WebAPI.Security.Abstract;
using OneDose.FirstProject.WebAPI.ServiceRegistirations;
using StackExchange.Redis;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomServices();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.AddSingleton<IRedisCacheService,RedisCacheService>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
   
    var redis = new RedisCacheService( redisSettings.Host, redisSettings.Port, builder.Configuration);
    redis.Connect();
    return redis;
});
builder.Services.AddSingleton<ITokenBlackListService, TokenBlackListService>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

    var redis = new TokenBlackListService(redisSettings.Host, redisSettings.Port);
    redis.Connect();
    return redis;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Person";
   
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,//Uygulamada izin verilecek sitelerin denetlenmesini etkiler
        ValidateIssuer = true,//hangi sitenin denetleyip denetlemeyeceðini izin verir
        ValidateLifetime = true, //tokenin yaþam süresi olsun mu
        ValidateIssuerSigningKey = true,  //tokenin kullanýcýya ait olup olmadýðýný kontrol eden bir parametre
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
        ClockSkew = TimeSpan.Zero


    };
});
builder.Services.AddDistributedMemoryCache();
/*builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("SessionPolicy", policy =>
    {
        policy.Requirements.Add(new SessionRequirement("X-Session-Id","Token"));
    });

});*/
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();
//app.UseMiddleware<ApiResponseMiddleware>();
app.UseMiddleware<BlacklistTokenMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
