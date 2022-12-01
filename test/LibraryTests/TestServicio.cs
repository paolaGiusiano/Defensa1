using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase Buscador
    /// </summary>
    [TestFixture]
    public class TestServicio
    {
        /// <summary>
        /// Prueba que un Servicio es creado con dados parametros
        /// </summary>
        [Test]
        public void TestCrearServicio()
        {
            string DescripcionServicio = "Servicio de jardineria";
            Categoria categoria = new Categoria("Jardineria");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "0");
            Servicio nuevoServicio=new Servicio(110,DescripcionServicio,pago,costo,trabajador,categoria, 100);
            Assert.AreEqual(nuevoServicio.DescripcionServicio, DescripcionServicio);
            Assert.AreEqual(nuevoServicio.Categoria, categoria);
            Assert.AreEqual(nuevoServicio.Pago, pago);
            Assert.AreEqual(nuevoServicio.Cost, costo);
            Assert.AreEqual(nuevoServicio.Trabajador, trabajador);
        }
        /// <summary>
        /// Prueba que el servicio se actualice de forma correcta
        /// </summary>
        [Test]
        public void TestActualizarServicio()
        {
            string DescripcionServicio = "Servicio de jardineria";
            Categoria categoria = new Categoria("Jardineria");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "0");
            Servicio nuevoServicio=new Servicio(109,DescripcionServicio,pago,costo,trabajador,categoria, 100);
            Categoria nuevacategoria=new Categoria("Conductor");
            Payment nuevopago = Payment.Credito;
            Costo nuevocosto= Costo.Costo_por_hora;
            nuevoServicio.ActualizarServicio(nuevacategoria,nuevopago,nuevocosto);
            Assert.AreEqual(Payment.Credito,nuevoServicio.Pago);
            Assert.AreEqual(Costo.Costo_por_hora,nuevoServicio.Cost);
            Assert.AreEqual("Conductor",nuevoServicio.Categoria.Nombre);
        }
        /// <summary>
        /// Prueba que la categoría modificada sea la esperada en el servicio que recibe este cambio
        /// </summary>
        [Test]
        public void TestModificarCategoria()
        {
            string DescripcionServicio = "Servicio de jardineria";
            Categoria categoria = new Categoria("Jardineria");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "0");
            Servicio nuevoServicio=new Servicio(108,DescripcionServicio,pago,costo,trabajador,categoria, 100);
            Categoria nuevacategoria=new Categoria("Conductor");
            nuevoServicio.ModificarCategoria(nuevacategoria);
            Assert.AreEqual("Conductor",nuevoServicio.Categoria.Nombre);
        }
        /// <summary>
        /// Testea que funcione correctamente el agregar un servicio a un trabajador
        /// </summary>
        [Test]
        public void TestAgregarServicio()
        {
            string DescripcionServicio = "Servicio de jardineria";
            Categoria categoria = new Categoria("Jardineria");
            Payment pago = Payment.Debito;
            Costo costo= Costo.Costo_total_servicio;
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test", "0");
            Servicio nuevoServicio=new Servicio(107,DescripcionServicio,pago,costo,trabajador,categoria, 100);
            Categoria nuevacategoria=new Categoria("Conductor");
            nuevoServicio.AgregarServicio(nuevoServicio,trabajador);
            trabajador.Servicios.Find(servicio=>servicio.DescripcionServicio=="Servicio de jardineria");
            Assert.AreEqual(trabajador.Servicios.Find(nuevoServicio=>nuevoServicio.DescripcionServicio=="Servicio de jardineria"),nuevoServicio);
        }





    }
}