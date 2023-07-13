using System.Text;
using AuthArmor.Samples.AspNetCore.WebApi.Configuration;
using AuthArmor.Sdk.Infrastructure;
using AuthArmor.Sdk.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authTokenConfig = builder.Configuration
    .GetSection(AuthenticationTokenConfiguration.ConfigurationKey)
    .Get<AuthenticationTokenConfiguration>()!;

builder.Services.AddSingleton<AuthenticationTokenConfiguration>(authTokenConfig);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = authTokenConfig.Issuer,
        ValidAudience = authTokenConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authTokenConfig.IssuerSigningKey))
    };
});

builder.Services.Configure<AuthArmorConfiguration>(builder.Configuration.GetSection("AuthArmor"));
builder.Services.AddAuthArmorSdkServices();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
