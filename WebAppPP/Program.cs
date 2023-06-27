using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using WebAppPP.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VarkaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddAuthentication("Cookies").AddCookie(options => options.LoginPath = "/SignLog/Login");

builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapGet("/login", async (HttpContext context) =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    // html-форма дл€ ввода логина/парол€
//    string loginForm = @"<!DOCTYPE html>
//    <html>
//    <head>
//        <meta charset='utf-8' />
//        <title>METANIT.COM</title>
//    </head>
//    <body>
//        <h2>Login Form</h2>
//        <form method='post'>
//            <p>
//                <label>Phone</label><br />
//                <input type='tel' name='phone' />
//            </p>
//            <p>
//                <label>Password</label><br />
//                <input type='password' name='password' />
//            </p>
//            <input type='submit' value='Login' />
//        </form>
//    </body>
//    </html>";
//    await context.Response.WriteAsync(loginForm);
//});

//app.MapPost("/SignLog/Login", async (string? returnUrl, HttpContext context) =>
//{
//    // получаем из формы phone и пароль
//    var form = context.Request.Form;
//    // если phone и/или пароль не установлены, посылаем статусный код ошибки 400
//    if (!form.ContainsKey("phone") || !form.ContainsKey("password"))
//        return Results.BadRequest("Phone и/или пароль не установлены");

//    string phone = form["phone"];
//    string password = form["password"];

//    VarkaDbContext db = new VarkaDbContext();
//    // находим пользовател€ 
//    var user = db.Users.FirstOrDefault(p => p.Phone == phone && p.Password == password && p.RoleId == 1);
//    // если пользователь не найден, отправл€ем статусный код 401
//    if (user is null) {
//         Results.Unauthorized();
//         Results.BadRequest("Ќедостаточно прав");
//    }



//    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Phone) };
//    // создаем объект ClaimsIdentity
//    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
//    // установка аутентификационных куки
//    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
//    return Results.Redirect(returnUrl ?? "/");
//});

//app.MapGet("/logout", async (HttpContext context) =>
//{
//    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//    return Results.Redirect("/SignLog/login");
//});

//app.Map("/", [Authorize] () => Results.Redirect("/SignLog/login"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignLog}/{action=Login}/{id?}");

app.Run();
