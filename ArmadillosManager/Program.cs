using ArmadillosManager.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.LoginPath = "/Login/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });
builder.Services.AddDbContext<Sistem21ClubdeportivoContext>(
    optionsBuilder => optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_clubdeportivo;user=sistem21_Test;password=sistemas20_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb")));
builder.Services.AddMvc();
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
app.UseFileServer();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.MapControllers();
app.UseEndpoints(x =>
{
    x.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}");
    x.MapDefaultControllerRoute();
});
app.Run();