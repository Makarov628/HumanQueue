using Microsoft.AspNetCore.Authentication.JwtBearer;
using MediatR;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using HQ.UseCases.Auth.Queries.GetUser;
using HQ.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using HQ.Infrastructure;
using HQ.Application;
using HQ.UseCases;
using HQ.Domain.Common.ValueObjects;
using HQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using HQ.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

AvailableCultures.AppendCulture("kk");
AvailableCultures.AppendCulture("ru");
AvailableCultures.AppendCulture("en");

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddAuthorization();
builder.Services.AddCors();

builder.Services.AddUseCases();
builder.Services.AddMediatR(typeof(RequestStatusChangedEventHandler).Assembly, typeof(HQ.UseCases.DependencyInjection).Assembly);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HQ", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();



app.UseStaticFiles();
app.UseRouting();
app.UseCors(cors =>
             cors
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowAnyOrigin()
            );

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<HQHub>("/hub").RequireCors(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .WithMethods("GET", "POST");
    });
});
app.MapFallbackToFile("index.html");

app.UseSwagger();
app.UseSwaggerUI();

// Init Migrations
using (var scope = app.Services.CreateScope())
{
    HQDbContext dbContext = scope.ServiceProvider.GetRequiredService<HQDbContext>();
    dbContext.Database.Migrate();
    DbInitializer.Initialize(dbContext);
}


app.Run();
