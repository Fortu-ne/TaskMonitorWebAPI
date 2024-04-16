using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Interface;
using TaskMonitorWebAPI.Repository;
using TaskMonitorWebAPI.Mapping;
using TaskMonitorWebAPI.Entities;
using System.Text.Json;
using TaskMonitorWebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddControllers(
//               //.AddJsonOptions(options =>
//               //{
//               //    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
//               //}
               
               
//               );
//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//{
//    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
//});

builder.Services.AddSwaggerGen(
    options =>
{

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,

    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
}
);

// Injection Pool
builder.Services.AddScoped<IUser, UserRep>();
builder.Services.AddTransient<ITask, TaskRep>();


builder.Services.AddDbContext<DataContext>(op =>
op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Singleton);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ProfileMapper));
builder.Services.AddCors();

//Background checker
builder.Services.AddHostedService<BackgroundReminder>();


builder.Services.AddAuthorization();

//builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<DataContext>();

//JWT Auth 
builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(x => {
    x.TokenValidationParameters = new TokenValidationParameters
    {

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        

    };
});

var app = builder.Build();

//app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
