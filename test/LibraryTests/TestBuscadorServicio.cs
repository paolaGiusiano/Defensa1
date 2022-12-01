using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase Buscador
    /// </summary>
    [TestFixture]
    public class TestBuscadorServicio
    {
        /// <summary>
        /// Variable global que representa una categoria muy usada
        /// </summary>
        public Categoria categoria1 = new Categoria("Jardineria");
        /// <summary>
        /// Variable global que representa una categoria no muy usada
        /// </summary>
        public Categoria categoria2 = new Categoria("Otros");
        /// <summary>
        /// Variable global que representa un servicio
        /// </summary>
        public Servicio servicio1 = new Servicio();
        /// <summary>
        /// Variable global que representa un servicio
        /// </summary>
        public Servicio servicio2 = new Servicio();
        /// <summary>
        /// Variable global que representa un servicio
        /// </summary>
        public Servicio servicio3 = new Servicio();
        /// <summary>
        /// Variable global que representa un servicio
        /// </summary>
        public Servicio servicio4 = new Servicio();
        /// <summary>
        /// Variable global que representa a un empleador usando el sistema
        /// </summary>
        public Empleador empleador1 = new Empleador();
        /// <summary>
        /// Variable global que define trabajador para servicio1
        /// </summary>
        public Trabajador trabajador1 = new Trabajador("Juan", "Rodriguez", "1111@ucu.edu.com", "099000000", "18 de Julio 1824","1234","0");
        /// <summary>
        /// Variable global que define trabajador para servicio2
        /// </summary>
        public Trabajador trabajador2 = new Trabajador("Roberto", "Torres", "2222@ucu.edu.com", "099111111", "18 de Julio 1825","1234","1");
        /// <summary>
        /// Variable global que define trabajador para servicio3
        /// </summary>
        public Trabajador trabajador3 = new Trabajador("Gabriel", "Esteban", "3333@ucu.edu.com", "099333333", "18 de Julio 1826","1234","2");
        /// <summary>
        /// Gestor de servicios que contiene todos los servicios
        /// </summary>
        public GestorServicios gestor = GestorServicios.getInstance();
        /// <summary>
        /// Gestor de categorias que contiene todas las categorias
        /// </summary>
        public GestorCategorias gestorCategorias = GestorCategorias.getInstance();

        /// <summary>
        /// Prepara los datos de prueba para el testing
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            gestorCategorias.TodaCategoria.Add(categoria1);
            gestorCategorias.TodaCategoria.Add(categoria2);
            servicio1.Trabajador = trabajador1;
            servicio1.Categoria = categoria2;
            servicio2.Trabajador = trabajador2;
            servicio2.Categoria = categoria1;
            servicio3.Trabajador = trabajador3;
            servicio3.Categoria = categoria1;
            servicio4.Trabajador = trabajador1;
            servicio4.Categoria = categoria1;
            gestor.TodoServicios.Add(servicio1);
            gestor.TodoServicios.Add(servicio2);
            gestor.TodoServicios.Add(servicio3);
            gestor.TodoServicios.Add(servicio4);
        }

        /// <summary>
        /// Prueba que el Buscador devuelve una lista basada en una categoria enviada como parametro
        /// </summary>
        [Test]
        public void TestBuscarPorCategoria()
        {
            string keyword = "Jardineria";
            BuscadorServicios buscador = BuscadorServicios.getInstance();
            List<Servicio> resultado = buscador.BuscarOrdenCalificacion(keyword, empleador1);
            foreach(Servicio servicio in resultado)
            {
                Assert.AreEqual(servicio.Categoria.Nombre, keyword);
            }
        }
        /// <summary>
        /// Prueba que el Buscador devuelve una lista ordendada de forma descendente por la calificacion del trabajador del servicio
        /// </summary>
        [Test]
        public void TestBuscarOrdenadoCalificacion()
        {
            string keyword = "Jardineria";
            BuscadorServicios buscador = BuscadorServicios.getInstance();
            List<Servicio> resultado = buscador.BuscarOrdenCalificacion(keyword, empleador1);
            List<Servicio> auxiliar = new List<Servicio>();
            foreach(Servicio servicio in gestor.TodoServicios)
            {
                if(servicio.Categoria.Nombre == keyword)
                {
                    auxiliar.Add(servicio);
                }
            }
            auxiliar.OrderByDescending(Servicio=>Servicio.Trabajador.CalificacionTotal);
            Assert.AreEqual(auxiliar,resultado);
        }
        /// <summary>
        /// Prueba que el Buscador devuelve una lista ordenada de forma ascendente por la direccion del trabajador del servicio
        /// Se requiere el API Geolocator para confirmar la distancia de dos puntos
        /// </summary>
        [Test]
        public void TestBuscarOrdenadoDistancia()
        {
            string keyword = "Jardineria";
            BuscadorServicios buscador = BuscadorServicios.getInstance();
            Empleador empleador = new Empleador("Test","Test","Email@email.com","0999999","Montevideo, Francisco Miranda 4270", "12345","3");
            List<Servicio> resultado = buscador.BuscarOrdenDistancia(keyword,empleador);
            List<Servicio> auxiliar = new List<Servicio>();
            foreach(Servicio servicio in gestor.TodoServicios)
            {
                if(servicio.Categoria.Nombre == keyword)
                {
                    auxiliar.Add(servicio);
                    Console.WriteLine($"Trabajador: {servicio.Trabajador.Nombre}, Direccion: {servicio.Trabajador.Direccion}, Categoria: {servicio.Categoria}");
                }
            }
            auxiliar.Sort((a, b) => buscador.calculator.CalculateDistance(a.Trabajador.Direccion,empleador.Direccion).Distance.CompareTo(buscador.calculator.CalculateDistance(b.Trabajador.Direccion,empleador.Direccion).Distance));
            Assert.AreEqual(auxiliar,resultado);
        }
    }
}