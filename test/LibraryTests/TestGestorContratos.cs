using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase GestorContratos
    /// </summary>
    [TestFixture]
    public class TestGestorContratos
    {
        GestorContratos gestor = GestorContratos.getInstance();

        /// <summary>
        /// Prueba que el gestor crea un contrato nuevo y lo agrega a su lista de todos los contratos
        /// </summary>
        [Test]
        public void TestCrearContrato()
        {
            Empleador empleador = new Empleador("Test","Test","test","099999999","Test","Test", "1");
            Trabajador trabajador = new Trabajador("Test","Test","test","099999999","Test","Test", "2");
            Categoria categoria = new Categoria("Test");
            Servicio servicio = new Servicio(105,"Test",Payment.Debito,Costo.Costo_por_hora,trabajador,categoria, 100);

            Contrato nuevoContrato = gestor.CrearContrato(empleador,servicio);

            Assert.AreEqual(nuevoContrato,gestor.TodoContratos.Find(Contrato=>Contrato.Empleador==empleador&&Contrato.Trabajador==trabajador));
        }
        /// <summary>
        /// Prueba que el gestor puede mostrar un contrato en particular con sus parametros
        /// </summary>
        [Test]
        public void TestMostrarContrato()
        {
            Empleador empleador = new Empleador("Test","Test","test","099999999","Test","Test", "1");
            Trabajador trabajador = new Trabajador("Test","Test","test","099999999","Test","Test", "2");
            Categoria categoria = new Categoria("Test");
            Servicio servicio = new Servicio(106,"Test",Payment.Debito,Costo.Costo_por_hora,trabajador,categoria, 100);

            Contrato nuevoContrato = gestor.CrearContrato(empleador,servicio);

            Contrato resultadoBusqueda = gestor.MostrarContrato(empleador,servicio);
            
            Assert.AreEqual(nuevoContrato,resultadoBusqueda);
        }
    }
}