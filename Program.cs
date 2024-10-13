using System.Text;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    (options) =>
    {
        options.AddPolicy(
            "DevCORS",
            (corsBuilder) =>
            {
                corsBuilder
                    .WithOrigins(
                        "http://localhost:4200",
                        "http://localhost:3000",
                        "http://localhost:8000"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
        );

        options.AddPolicy(
            "ProdCORS",
            (corsBuilder) =>
            {
                corsBuilder
                    .WithOrigins("https://myPRodsite.Com")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
        );
    }
);

builder.Services.AddScoped<IUserRepository, UserRepository>();

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : "")
);

TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false,
};

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = tokenValidationParameters;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCORS");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCORS");
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// app.MapGet("/weatherforecast", () => { }).WithName("GetWeatherForecast").WithOpenApi();

app.Run();
