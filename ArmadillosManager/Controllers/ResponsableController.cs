using ArmadillosManager.Models;
using ArmadillosManager.Models.ViewModels;
using ArmadillosManager.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArmadillosManager.Controllers
{
    public class ResponsableController : Controller
    {
        private readonly Sistem21ClubdeportivoContext context;
        private readonly Repository<Responsable> repositoryResponsable;
        private readonly Repository<Jugador> repositoryJugador;

        public ResponsableController(Sistem21ClubdeportivoContext cx)
        {
            this.context = cx;
            repositoryResponsable = new Repository<Responsable>(context);
            repositoryJugador = new Repository<Jugador>(context);
        }
        [Route("/GestionarResponsable")]
        public IActionResult GestionarResponsable()
        {
            var datos = repositoryResponsable.GetAll().ToList();
            GestionarResponsablesViewModel vm = new GestionarResponsablesViewModel { Responsables = datos };
            return View(vm);
        }
        [Route("/")]
        [Route("/Index")]
        [Route("/GestionarJugadores")]
        public IActionResult GestionarJugadores()
        {
            var jugadores = repositoryJugador.GetAll().Include(x => x.IdResponsableNavigation).Include(x => x.CategoriaNavigation).Select(x => new Jugador
            {
                CategoriaNavigation = x.CategoriaNavigation,
                Direccion = x.Direccion,
                Id = x.Id,
                Nombre = x.Nombre,
                Telefono = x.Telefono,
                IdResponsableNavigation = x.IdResponsableNavigation
            });
            GestionarJugadoresViewModels vm = new GestionarJugadoresViewModels() { Jugadores = jugadores };
            return View(vm);
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
        public IActionResult AgregarJugador()
        {
            AgregarJugadorViewModel vm = new AgregarJugadorViewModel();
            vm.Categorias = context.Categoria.OrderBy(x => x.Categoria1);
            vm.Ligas = context.Liga.OrderBy(x => x.Liga1);
            return View(vm);
        }
        [HttpPost]
        public IActionResult AgregarJugador(AgregarJugadorViewModel jugadorvm)
        {
            if (jugadorvm.Jugador != null)
            {
                byte errores = 0;
                if (string.IsNullOrWhiteSpace(jugadorvm.Jugador.Nombre))
                {
                    ModelState.AddModelError("", "Favor de escribir el nombre del jugador");
                    errores++;
                }
                if (errores<=0)
                    repositoryJugador.Insert(jugadorvm.Jugador);

                return RedirectToAction("GestionarJugadores");
            }
            return View(jugadorvm);
        }
        [HttpPost]
        public IActionResult EditarResponsable(Responsable res)
        {
            var a = repositoryResponsable.GetById(res.Id);
            if (a == null)
                return NotFound();
            a.Telefono = res.Telefono;
            a.Correo = res.Correo;
            a.Direccion = res.Direccion;
            repositoryResponsable.Update(res);
            return Ok();
        }
        [HttpPost]
        public IActionResult EditarJugador(Jugador juga)
        {
            var a = repositoryJugador.GetById(juga.Id);
            if (a == null)
                return NotFound();
            a.Telefono = juga.Telefono;
            a.Direccion = juga.Direccion;
            a.Categoria = juga.Categoria;
            a.Liga = juga.Liga;
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
