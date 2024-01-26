using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using apiNew.Models;
using apiNew.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "dotnet-user-jwts",
        ValidAudiences = new[] { "http://localhost:4200", "http://localhost:5075", "http://localhost:5075", "https://localhost:7178" },
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aSIn2Pdd7BPGpiMIFFwyEZ+RGC2nqFYcGOkDpYntP4Q="))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* The line `builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));` is configuring
the application to use Entity Framework Core for database access. */
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

/* The line `builder.Services.AddScoped<IDriverRepository, DriverRepository>();` is registering a
scoped dependency injection for the `IDriverRepository` interface and its implementation
`DriverRepository`. */
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<apiNew.Services.AuthService>();
var app = builder.Build();
app.UseAuthentication();
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
