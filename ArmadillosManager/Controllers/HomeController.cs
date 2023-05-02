using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ArmadillosManager.Models;
using ArmadillosManager.Repositories;

namespace ArmadillosManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly Sistem21ClubdeportivoContext context;
        private readonly Repository<Responsable> repositoryResponsable;

        public HomeController(Sistem21ClubdeportivoContext cx)
        {
            this.context = cx;
            repositoryResponsable = new Repository<Responsable>(context);
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IniciarSesion(Login login)
        {
            var datos = repositoryResponsable.GetAll().Select(x => x.Nombre);
            if (login.UserName.ToLower() == "prueba" && login.Password == "Admina1")
            {
                var listaclaims = new List<Claim>();
                foreach (var claim in datos)
                {
                    listaclaims.Add(new Claim(ClaimTypes.Name, claim));
                }
                //var listaclaims = new List<Claim>()
                //{
                //    new Claim(ClaimTypes.Name,"Guillermo Saúl Andrade Silos"),
                //    new Claim(ClaimTypes.Role,"Administrador")
                //};
                var identidad = new ClaimsIdentity(listaclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad),
                    new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTimeOffset.Now.AddMinutes(5),
                        IsPersistent = true
                    });
                return RedirectToAction("Index", "Home", new { Area = "Administrador" });
            }
            {
                ModelState.AddModelError("", "Nombre de usuario o contraseña incorrecta");
                return View(login);
            }
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
