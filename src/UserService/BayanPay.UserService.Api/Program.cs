using System.Security.Claims;
using BayanPay.UserService.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Clerk", options =>
{
    // Authority is your Clerk instance (e.g. https://your-app.clerk.accounts.dev or https://clerk.yourdomain.com)
    options.Authority = builder.Configuration["Clerk:Authority"];
    // Clerk access tokens often don't require audience validation for your API; set to false unless you configured an audience.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Service API",
        Version = "v1"
    });
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/me", async (ClaimsPrincipal user, UserDbContext db) =>
{
    // get Clerk user id from token claim "sub" (or "clerk_user_id" if you’ve mapped it)
    var clerkUserId = user.FindFirst("sub")?.Value;
    if (string.IsNullOrEmpty(clerkUserId)) return Results.Unauthorized();

    var me = await db.Users.FirstOrDefaultAsync(u => u.ClerkUserId == clerkUserId);
    return me is null ? Results.NotFound() : Results.Ok(me);
})
.RequireAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

