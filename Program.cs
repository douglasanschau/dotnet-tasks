using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using tarefaUsuariosDotnet.Models;
using tarefaUsuariosDotnet.Data;
using tarefaUsuariosDotnet.DependencyInjection;
using tarefaUsuariosDotnet.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Database>();

// 🍪 Cookie config
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Usuario/Login";
    options.AccessDeniedPath = "/Usuario/AcessoNegado";
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationServices();

//Sessions
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseSession();
app.UseAuthentication();   // 👈 LOGIN FUNCIONA AQUI
app.UseMiddleware<AuthMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
