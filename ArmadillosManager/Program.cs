using ArmadillosManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Sistem21ClubdeportivoContext>(
    optionsBuilder => optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_clubdeportivo;user=sistem21_Test;password=sistemas20_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb")));
builder.Services.AddMvc();
var app = builder.Build();
app.UseFileServer();
app.UseRouting();
app.MapControllers();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.Run();