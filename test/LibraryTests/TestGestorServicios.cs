using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase GestorUsuario
    /// </summary>
    [TestFixture]
    public class TestGestorServicio
    {
        /// <summary>
        /// Instancia del gestor de usuario singleton
        /// </summary>
        public GestorServicios gestor = GestorServicios.getInstance();
        
        /// <summary>
        /// Comprueba que el servicio creado se agrega a la lista de TodoServicios
        /// </summary>
        [Test]
        public void TestCrearServicio()
        {
            string DescripcionServicio = "Servicio de jardineria";
            Categoria categoria = new Categoria("Jardineria");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "1");
            Servicio servicio=gestor.CrearServicio(DescripcionServicio,pago,costo,trabajador,categoria, 100);
            gestor.TodoServicios.Find(nuevoServicio=>nuevoServicio.DescripcionServicio=="Servicio de jardineria");
            Assert.AreEqual(gestor.TodoServicios.Find(nuevoServicio=>nuevoServicio.DescripcionServicio=="Servicio de jardineria").DescripcionServicio,servicio.DescripcionServicio);
            
        }
        /// <summary>
        /// Se prueba que al Agregar un servicio, este se agregue con los parametros esperados
        /// </summary>
        [Test]
        public void TestAgregarServicio()
        {
            string DescripcionServicio = "Servicio de jardineria";
            Categoria categoria = new Categoria("Jardineria");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "1");
            string DescripcionServicioesperada = "Servicio de jardineria";
            Servicio nuevoServicio= gestor.CrearServicio(DescripcionServicio,pago,costo,trabajador,categoria, 100);
            Categoria categoriaesperada = new Categoria("Jardineria");
            Payment pagoesperado = Payment.Debito;
            Costo costoesperado= Costo.Costo_total_servicio;
            Trabajador trabajadoresperado=new Trabajador("Test","Test","test","099999999","Test","Test", "2");
            Assert.AreEqual(nuevoServicio.DescripcionServicio, DescripcionServicioesperada);
            Assert.AreEqual(nuevoServicio.Pago, pagoesperado);
            Assert.AreEqual(nuevoServicio.Cost, costoesperado);
            Assert.AreEqual(nuevoServicio.Categoria.Nombre, categoriaesperada.Nombre);
            Assert.AreEqual(nuevoServicio.Trabajador.Nombre, trabajadoresperado.Nombre);
        }
        /// <summary>
        /// Prueba que los servicios sean removidos con éxito de la lista TodoServicios
        /// </summary>
        [Test]
        public void TestRemoverServicio()
        {
            Administrador admin = new Administrador();
            string DescripcionServicio = "Servicio de conduccion";
            Categoria categoria = new Categoria("Conduccion");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "1");
            Servicio servicio=gestor.CrearServicio(DescripcionServicio,pago,costo,trabajador,categoria, 100);
            gestor.TodoServicios.Find(nuevoServicio=>nuevoServicio.DescripcionServicio=="Servicio de conduccion");
            gestor.RemoverServicio(servicio, admin, "Servicio inadecuado");
            Assert.AreEqual(gestor.TodoServicios.Find(nuevoServicio=>nuevoServicio.DescripcionServicio=="Servicio de conduccion"),null);
            
        }




    }

}