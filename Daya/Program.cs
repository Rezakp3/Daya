using Daya.Jwt;
using Domain;
using Jwt;
using LocationService.Interface;
using LocationService.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ReserveService.Interface;
using ReserveService.Repository;
using UserService.Interface;
using UserService.Repository;

var builder = WebApplication.CreateBuilder(args);

#region Di

builder.Services.AddScoped<IDbCon, DbCon>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ILocationTypeRepo, LocationTypeRepo>();
builder.Services.AddScoped<ILocationRepo, LocationRepo>();
builder.Services.AddScoped<IReserveRepo, ReserveRepo>();
builder.Services.AddScoped<IAuthorizationHandler, JustActiveAuth>();

#endregion  

// Add services to the container.

#region Jwt

builder.Services.AddScoped<JwtTokenCreator>();
builder.Services.AddSwaggerService();
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("JustActive", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("IsActive", "true");
        policy.Requirements.Add(new IsActiveRequirement());
    });
});

#endregion

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
