﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ArmadillosManager.Models;
using ArmadillosManager.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ArmadillosManager.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ArmadillosManager.Areas.Responsables.Controllers
{
    [Authorize(Roles = "Responsables")]
    [Area("Responsables")]
    public class HomeController : Controller
    {
        private readonly Sistem21ClubdeportivoContext context;
        private readonly Repository<Responsable> repositoryResponsable;
        private readonly Repository<Jugador> repositoryJugador;
        private readonly Repository<Movimientos> repositoryMovimientos;

        public HomeController(Sistem21ClubdeportivoContext cx)
        {
            context = cx;
            repositoryResponsable = new Repository<Responsable>(context);
            repositoryJugador = new Repository<Jugador>(context);
            repositoryMovimientos = new Repository<Movimientos>(context);
        }
        [Route("/GestionarPrincipal")]
        public IActionResult GestionarPrincipal()
        {
            string? test = HttpContext.Session.GetString("NombreResponsable");
            if (test != null)
            {
                ResponsableViewModel vm = new ResponsableViewModel();
                vm.ResponsableInfo = new Responsable();
                vm.Jugadores = repositoryJugador.GetAll().Include(x => x.CategoriaNavigation).Where(x => x.IdResponsableNavigation.Correo == test);
                vm.ResponsableInfo.Nombre = repositoryResponsable.GetAll().Where(x => x.Correo.ToLower() == test).Select(x => x.Nombre).FirstOrDefault();
                vm.ResponsableInfo.Direccion = repositoryResponsable.GetAll().Where(x => x.Correo.ToLower() == test).Select(x => x.Direccion).FirstOrDefault();
                vm.ResponsableInfo.Telefono = repositoryResponsable.GetAll().Where(x => x.Correo == test).Select(x => x.Telefono).FirstOrDefault();
                vm.Movimientos = context.Movimientos.Include(x => x.IdPagoNavigation).Include(x => x.IdPagoNavigation.IdResponsableNavigation)
                    .Where(x => x.IdPagoNavigation.IdResponsableNavigation.Nombre == vm.ResponsableInfo.Nombre)
                    .Select(x => new ResponsableHelpViewModel
                    {
                        MovimientoHelp = new Movimientos { Concepto = x.Concepto, Monto = x.Monto },
                        JugadorHelp = new Jugador { Nombre = x.IdPagoNavigation.IdJugadorNavigation.Nombre, Direccion = x.IdPagoNavigation.IdJugadorNavigation.Direccion, Telefono = x.IdPagoNavigation.IdJugadorNavigation.Telefono }
                    });
                return View(vm);
            }
            else
                return BadRequest("Si esto sale algo esta mal, literal no hay forma de que esto salga");
        }
        [Route("/ResponsablePrincipal/Movimientos")]
        public IActionResult MovimientosJugadores(Responsable responsable)
        {
            var jugadoresresponsable = repositoryJugador.GetAll().Where(x => x.IdResponsableNavigation.Nombre == responsable.Nombre);
            return Ok(jugadoresresponsable);
        }
        [HttpGet("/EditarJugador/Jugador/{id}")]
        public IActionResult EditarJugador(int id)
        {
            var a = repositoryJugador.GetAll().Include(x => x.IdResponsableNavigation).Where(x => x.Id == id).FirstOrDefault();
            if (a == null)
                return RedirectToAction("GestionarPrincipal");
            if (a.IdResponsableNavigation.Correo != HttpContext.Session.GetString("NombreResponsable"))
                return RedirectToAction("GestionarPrincipal");
            AgregarJugadorViewModel vm = new AgregarJugadorViewModel();
            vm.Jugador = a;
            vm.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            vm.Ligas = context.Liga.OrderBy(x => x.Liga1);
            vm.Responsables = context.Responsable.OrderBy(x => x.Nombre);
            if (!string.IsNullOrWhiteSpace(a.IdResponsable.ToString()))
                vm.RFC = repositoryResponsable.GetAll().Where(x => x.Id == a.IdResponsable).Select(x => x.Rfc).FirstOrDefault();
            return View(vm);
        }
        [HttpPost("/Home/EditarJugador")]
        public IActionResult EditarJugador(AgregarJugadorViewModel juga)
        {
            var a = repositoryJugador.GetAll().Include(x => x.IdResponsableNavigation).Where(x => x.Nombre == juga.Jugador.Nombre).FirstOrDefault();
            if (a == null)
                return NotFound("El jugador que buscas editar puede que haya sido eliminado/dado de baja por el admnistrador, favor de comunicarse al local");
            if (a.IdResponsableNavigation.Correo != HttpContext.Session.GetString("NombreResponsable"))
                return RedirectToAction("GestionarPrincipal");
            if (!string.IsNullOrWhiteSpace(juga.RFC))
                a.IdResponsable = repositoryResponsable.GetAll().Where(x => x.Rfc == juga.RFC).Select(x => x.Id).FirstOrDefault();
            if (a.IdResponsable == 0)
                return BadRequest("Favor de agregar el RFC del Patrocinador/Responsable");
            a.Telefono = juga.Jugador.Telefono;
            a.Direccion = juga.Jugador.Direccion;
            a.Categoria = juga.Jugador.Categoria;
            a.Liga = juga.Jugador.Liga;
            repositoryJugador.Update(a);
            return Ok();
        }
    }
}