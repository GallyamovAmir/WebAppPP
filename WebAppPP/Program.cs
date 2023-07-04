using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;
using System;
using WebAppPP.Models;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<VarkaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VarkaDb")));

builder.Services.AddAuthentication("Cookies").AddCookie(options =>
{
    options.LoginPath = "/SignLog/Login";
    options.LogoutPath = "/SignLog/Logout";
});



builder.Services.AddAuthentication();

var app = builder.Build();

//ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//ILogger logger = loggerFactory.CreateLogger<Program>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignLog}/{action=Login}/{id?}");

app.Run();
