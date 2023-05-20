using ArmadillosManager.Models;
using ArmadillosManager.Models.ViewModels;
using ArmadillosManager.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ArmadillosManager.Areas.Administrador.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Administrador")]
    public class ResponsableController : Controller
    {
        private readonly Sistem21ClubdeportivoContext context;
        private readonly Repository<Responsable> repositoryResponsable;
        private readonly Repository<Jugador> repositoryJugador;
        private readonly Repository<Temporada> repositoryTemporada;
        private readonly Repository<Pago> repositoryPago;

        public ResponsableController(Sistem21ClubdeportivoContext cx)
        {
            context = cx;
            repositoryResponsable = new Repository<Responsable>(context);
            repositoryJugador = new Repository<Jugador>(context);
            repositoryTemporada = new Repository<Temporada>(context);
            repositoryPago = new Repository<Pago>(context);
        }
        [Route("/GestionarResponsable")]
        public IActionResult GestionarResponsable(GestionarResponsablesViewModel jugavm)
        {
            var datos = repositoryResponsable.GetAll().ToList();
            GestionarResponsablesViewModel vm = new GestionarResponsablesViewModel { Responsables = datos };
            vm.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            return View(vm);
        }
        [Route("/GestionarJugadores")]
        public IActionResult GestionarJugadores(GestionarJugadoresViewModels jugavm)
        {
            GestionarJugadoresViewModels v = new GestionarJugadoresViewModels();
            var vm = repositoryJugador.GetAll().Include(x => x.IdResponsableNavigation).Include(x => x.CategoriaNavigation).Include(x => x.LigaNavigation)
                .Select(x => new Jugador
                {
                    CategoriaNavigation = x.CategoriaNavigation,
                    Direccion = x.Direccion,
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Telefono = x.Telefono,
                    IdResponsableNavigation = x.IdResponsableNavigation,
                    LigaNavigation = x.LigaNavigation
                });
            if (jugavm.Filtros?.Liga != "Todos" && jugavm.Filtros?.Liga != null)
                vm = vm.Where(x => x.LigaNavigation.Liga1 == jugavm.Filtros.Liga);
            if (jugavm.Filtros?.Categoria != "Todos" && jugavm.Filtros?.Categoria != null)
                vm = vm.Where(x => x.CategoriaNavigation.IdCategoria.ToString() == jugavm.Filtros.Categoria);
            v.Jugadores = vm;
            v.Filtros = jugavm.Filtros;
            v.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            return View(v);
        }
        public IActionResult AgregarResponsable()
        {
            AgregarResponsableViewModel vm = new AgregarResponsableViewModel();
            vm.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            vm.Ligas = context.Liga.OrderBy(x => x.Liga1);
            vm.Jugadores = repositoryJugador.GetAll().Select(x => new Jugador
            {
                Nombre = x.Nombre,
                CategoriaNavigation = x.CategoriaNavigation,
                LigaNavigation = x.LigaNavigation
            });
            return View(vm);
        }
        [HttpPost]
        public IActionResult AgregarResponsable(AgregarResponsableViewModel responsablevm)
        {
            if (responsablevm.Responsable != null)
            {
                byte errores = 0;
                if (string.IsNullOrWhiteSpace(responsablevm.Responsable.Nombre))
                {
                    ModelState.AddModelError("", "Favor de escribir el nombre del responsable");
                    errores++;
                }
                if (errores <= 0)
                    repositoryResponsable.Insert(responsablevm.Responsable);

                return RedirectToAction("GestionarJugadores");
            }
            return View(responsablevm);
        }
        [HttpGet("/Responsable/AgregarJugador")]
        public IActionResult AgregarJugador()
        {
            AgregarJugadorViewModel vm = new AgregarJugadorViewModel();
            vm.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            vm.Ligas = context.Liga.OrderBy(x => x.Liga1);
            vm.Responsables = context.Responsable.OrderBy(x => x.Nombre);
            vm.RFC = "";
            return View(vm);
        }
        [HttpPost]
        public IActionResult AgregarJugador(AgregarJugadorViewModel jugadorvm)
        {
            if (jugadorvm.Jugador.Nombre != null)
            {
                byte errores = 0;
                if (string.IsNullOrWhiteSpace(jugadorvm.Jugador.Nombre))
                {
                    ModelState.AddModelError("", "Favor de escribir el nombre del jugador");
                    errores++;
                }
                jugadorvm.Jugador.IdResponsable = repositoryResponsable.GetAll().Where(x => x.Rfc.ToLower() == jugadorvm.RFC.ToLower()).Select(x => x.Id).FirstOrDefault();
                if (errores <= 0)
                    repositoryJugador.Insert(jugadorvm.Jugador);

                return RedirectToAction("GestionarJugadores");
            }
            return View(jugadorvm);
        }
        [HttpGet("/GestionarPago")]
        public IActionResult GestionarPago()
        {
            GestionarPagoViewModel v = new GestionarPagoViewModel();
            v.Pagos = repositoryPago.GetAll().Include(x => x.IdResponsableNavigation).Include(x => x.IdJugadorNavigation).Include(x => x.IdJugadorNavigation.CategoriaNavigation)
            .Select(x => new Pago
            {
                Id = x.Id,
                IdResponsable = x.IdResponsable,
                IdJugador = x.IdJugador,
                Estado = x.Estado,
                IdJugadorNavigation = x.IdJugadorNavigation,
                IdResponsableNavigation = x.IdResponsableNavigation
            });
            v.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            return View(v);
        }
        [HttpPost]
        public IActionResult GestionarPago(GestionarPagoViewModel pagovm)
        {
            GestionarPagoViewModel v = new GestionarPagoViewModel();
            var vm = repositoryPago.GetAll().Include(x => x.IdResponsableNavigation).Include(x => x.IdJugadorNavigation).Include(x => x.IdJugadorNavigation.CategoriaNavigation)
            .Select(x => new Pago
            {
                Id = x.Id,
                IdResponsable = x.IdResponsable,
                IdJugador = x.IdJugador,
                Estado = x.Estado,
                IdJugadorNavigation = x.IdJugadorNavigation,
                IdResponsableNavigation = x.IdResponsableNavigation
            });
            if (pagovm.Filtros?.Liga != "Todos" && pagovm.Filtros?.Liga != null)
                vm = vm.Where(x => x.IdJugadorNavigation.LigaNavigation.Liga1 == pagovm.Filtros.Liga);
            if (pagovm.Filtros?.Categoria != "Todos" && pagovm.Filtros?.Categoria != null)
                vm = vm.Where(x => x.IdJugadorNavigation.Categoria.ToString() == pagovm.Filtros.Categoria);
            if (pagovm.Filtros?.Estado != "Todos" && pagovm.Filtros?.Estado != null)
                vm = vm.Where(x => x.Estado == pagovm.Filtros.Estado);
            v.Pagos = vm;
            v.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            return View(v);
        }
        [HttpGet("/GestionarTemporada")]
        public IActionResult GestionarTemporada()
        {
            GestionarTemporadaViewModel vm = new GestionarTemporadaViewModel();
            vm.Temporada = context.Temporada.OrderByDescending(x => x.Id).FirstOrDefault();
            vm.FinalLigaJuvenilFormateada = vm.Temporada.FinalLigaJuvenil.ToString("yyyy-MM-dd");
            vm.InicioLigaJuvenilFormateada = vm.Temporada.InicioLigaJuvenil.ToString("yyyy-MM-dd");
            vm.InicioLigaInfantilFormateada = vm.Temporada.InicioLigaInfantil.ToString("yyyy-MM-dd");
            vm.FinalLigaInfantilFormateada = vm.Temporada.FinalLigaInfantil.ToString("yyyy-MM-dd");
            return View(vm);
        }
        [HttpPost]
        public IActionResult GestionarTemporada(GestionarTemporadaViewModel temp)
        {
            if (string.IsNullOrWhiteSpace(temp.Temporada.CostoTemporadaJuvenil.ToString()))
                ModelState.AddModelError("", "El costo de la temporada Juvenil no puede ser menor o igual a 0");
            if (temp.Temporada.CostoTemporadaJuvenil <= 0)
                ModelState.AddModelError("", "El costo de la temporada Juvenil no puede ser menor o igual a 0");
            if (string.IsNullOrWhiteSpace(temp.Temporada.CostoTemporadaInfantil.ToString()))
                ModelState.AddModelError("", "El costo de la temporada Infantil no puede ser menor o igual a 0");
            if (temp.Temporada.CostoTemporadaInfantil <= 0)
                ModelState.AddModelError("", "El costo de la temporada Infantil no puede ser menor o igual a 0");
            if (DateOnly.Parse(temp.InicioLigaInfantilFormateada) >= DateOnly.Parse(temp.FinalLigaInfantilFormateada) || DateOnly.Parse(temp.InicioLigaInfantilFormateada) <= DateOnly.FromDateTime(DateTime.Now))
                ModelState.AddModelError("", "La fecha de inicio no puede ser la misma que la fecha final");
            if (DateOnly.Parse(temp.InicioLigaJuvenilFormateada) >= DateOnly.Parse(temp.FinalLigaJuvenilFormateada) || DateOnly.Parse(temp.InicioLigaJuvenilFormateada) <= DateOnly.FromDateTime(DateTime.UtcNow))
                ModelState.AddModelError("", "La fecha de inicio no puede ser la misma que la fecha final");
            temp.Temporada.InicioLigaInfantil = DateOnly.ParseExact(temp.InicioLigaJuvenilFormateada, "dd-MM-YYYY");
            temp.Temporada.FinalLigaInfantil = DateOnly.ParseExact(temp.FinalLigaInfantilFormateada, "dd-MM-YYYY");
            temp.Temporada.InicioLigaJuvenil = DateOnly.ParseExact(temp.InicioLigaJuvenilFormateada, "dd-MM-YYYY");
            temp.Temporada.FinalLigaJuvenil = DateOnly.ParseExact(temp.FinalLigaJuvenilFormateada, "dd-MM-YYYY");
            repositoryTemporada.Insert(temp.Temporada);
            return Ok();
        }
        public IActionResult EditarResponsable(int id)
        {
            var a = repositoryResponsable.GetById(id);
            if (a == null)
                return NotFound();
            AgregarResponsableViewModel vm = new AgregarResponsableViewModel();
            vm.Responsable = new Responsable
            {
                Nombre = a.Nombre,
                Correo = a.Correo,
                Rfc = a.Rfc,
                Telefono = a.Telefono,
                Direccion = a.Direccion
            };
            return View(vm);
        }
        [HttpPost("/EditarResponsable")]
        public IActionResult EditarResponsable(AgregarResponsableViewModel res)
        {
            var a = repositoryResponsable.GetAll().Where(x => x.Nombre == res.Responsable.Nombre).FirstOrDefault();
            if (a == null)
                return NotFound();
            a.Rfc = res.Responsable.Rfc;
            a.Telefono = res.Responsable.Telefono;
            a.Correo = res.Responsable.Correo;
            a.Direccion = res.Responsable.Direccion;
            repositoryResponsable.Update(a);
            return Ok();
        }
        public IActionResult EditarJugador(int id)
        {
            var a = repositoryJugador.GetById(id);
            if (a == null)
                return RedirectToAction("Index");
            AgregarJugadorViewModel vm = new AgregarJugadorViewModel();
            vm.Jugador = a;
            vm.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            vm.Ligas = context.Liga.OrderBy(x => x.Liga1);
            vm.Responsables = context.Responsable.OrderBy(x => x.Nombre);
            if (!string.IsNullOrWhiteSpace(a.IdResponsable.ToString()))
                vm.RFC = repositoryResponsable.GetAll().Where(x => x.Id == a.IdResponsable).Select(x => x.Rfc).FirstOrDefault();
            return View(vm);
        }
        [HttpPost("/EditarJugador")]
        public IActionResult EditarJugador(AgregarJugadorViewModel juga)
        {
            var a = repositoryJugador.GetAll().Where(x => x.Nombre == juga.Jugador.Nombre).FirstOrDefault();
            if (a == null)
                return NotFound();
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

        /*                                              Gracias BING
         Aquí hay un ejemplo de cómo puedes crear un filtro de búsqueda en tiempo real con React y JavaScript:

import React, { useState } from 'react';

function SearchFilter() {
  const [filter, setFilter] = useState('');
  const data = [
    { name: 'Juan' },
    { name: 'Pedro' },
    { name: 'Ana' },
    // ...
  ];

  const filteredData = data.filter(item => item.name.includes(filter));

  return (
    <>
      <input
        type="text"
        placeholder="Buscar..."
        onChange={e => setFilter(e.target.value)}
      />
      <table>
        <thead>
          <tr>
            <th>Nombre</th>
          </tr>
        </thead>
        <tbody>
          {filteredData.map(item => (
            <tr key={item.name}>
              <td>{item.name}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}
En este ejemplo, usamos el estado filter para almacenar el valor del filtro y el método setFilter para actualizarlo. 
        Cada vez que el usuario escribe en el elemento de entrada, el evento onChange se activa y actualiza el estado del 
        filtro con el valor actual del elemento de entrada.

Luego usamos el método filter de JavaScript para filtrar los datos de la tabla en función del valor del filtro. 
        Finalmente, mostramos los datos filtrados en la tabla.
         */
    }
}
