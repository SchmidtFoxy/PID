using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using PID.Data;
using PID.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuração do Identity
builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Protege controllers com filtro global
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

// Protege todas Razor Pages, exceto algumas
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");

    // ⛔ Libera páginas do Identity que precisam estar acessíveis sem login
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Register");
    options.Conventions.AllowAnonymousToPage("/Account/ForgotPassword");
    options.Conventions.AllowAnonymousToPage("/Account/ResetPassword");
    options.Conventions.AllowAnonymousToPage("/Account/ConfirmEmail");
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "controllerOnly",
    pattern: "{controller}",
    defaults: new { action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
