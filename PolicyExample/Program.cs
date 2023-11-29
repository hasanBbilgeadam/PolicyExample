using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PolicyExample.AppPolicies;
using PolicyExample.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(a =>
{
    a.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlCon"));
});


builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();




builder.Services.AddAuthentication(opt =>
{

    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt => {

    opt.RequireHttpsMetadata = false;

    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = "http://localhost",
        ValidAudience = "http://localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GucluBirSifreTokenSifresi")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorizationHandler,FreeAccesRequirmentHander>();
builder.Services.AddAuthorization(opt =>
{

    opt.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ANKARA");
    });

    opt.AddPolicy("FreeAccessPolicy", policy =>
    {
        policy.AddRequirements(new FreeAccesRequirement());
    });


});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    await RoleManager.CreateAsync(new() { Name = "appAdmin" });

    await userManager.CreateAsync(new AppUser()
    {
        UserName = "AdminAdmin",
        Email = "admin@gmail.com",

    },"Hasan.123");


   var adminUser =   await userManager.FindByEmailAsync("admin@gmail.com");
   await userManager.AddToRoleAsync(adminUser, "appAdmin");
}

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
