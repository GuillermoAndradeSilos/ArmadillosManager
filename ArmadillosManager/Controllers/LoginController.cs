using ArmadillosManager.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ArmadillosManager.Repositories;

namespace ArmadillosManager.Controllers
{
    public class LoginController : Controller
    {
        private readonly Repository<Responsable> repositoryResponsable;
        public LoginController(Sistem21ClubdeportivoContext cx)
        {
            repositoryResponsable = new Repository<Responsable>(cx);
        }
        [Route("/")]
        [Route("/Index")]
        [Route("/IniciarSesion")]
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IniciarSesion(Login login)
        {
            var datos = repositoryResponsable.GetAll()
                .Where(x => x.Correo.ToLower() == login.UserName.ToLower())
                .Select(x => new Login { UserName = x.Correo, Password = x.Contraseña }).FirstOrDefault();
            if (datos != null && login.UserName.ToLower() == datos.UserName.ToLower() && login.Password == datos.Password)
            {
                //Esta linea es para registrar todos los usuarios
                var listaclaims = new List<List<Claim>>();
                var test = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, login.UserName),
                    };
                if (datos.Password == login.Password && login.Password == "Administrador")
                {
                    test.Add(new Claim(ClaimTypes.Role, "Administrador"));
                    listaclaims.Add(test);
                    var identidadadmin = new ClaimsIdentity(listaclaims[0], CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadadmin),
                        new AuthenticationProperties()
                        {
                            ExpiresUtc = DateTimeOffset.Now.AddMinutes(5),
                            IsPersistent = true
                        });
                    return RedirectToAction("GestionarJugadores", "Responsable", new { Area = "Administrador" });
                }
                else
                {
                    test.Add(new Claim(ClaimTypes.Role, "Responsables"));
                    listaclaims.Add(test);
                    var identidad = new ClaimsIdentity(listaclaims[0], CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad),
                        new AuthenticationProperties()
                        {
                            ExpiresUtc = DateTimeOffset.Now.AddMinutes(5),
                            IsPersistent = true
                        });
                    HttpContext.Session.SetString("NombreResponsable", datos.UserName.ToLower());
                    return RedirectToAction("GestionarPrincipal", "Home", new { Area = "Responsables" });
                }
            }
            else
            {
                ModelState.AddModelError("AAAA", "Correo o contraseña incorrecta");
                return View(login);
            }
            /*Falta agregar los recibos del responsable, es decir el recibo que genera el dadministrador que se envie al responsable 
                 para que se pueda ver un estado de cuenta*/
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("IniciarSesion");
        }
    }
}
