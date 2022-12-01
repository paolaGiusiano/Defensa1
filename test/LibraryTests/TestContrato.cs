using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase Contrato
    /// </summary>
    [TestFixture]
    public class TestContrato
    {
        GestorContratos gestor = GestorContratos.getInstance();
        /// <summary>
        /// Test que verifica que el contrato se cree correctamente con sus parametros correspondientes
        /// </summary>
        [Test]
        public void TestCrearContrato()
        {   
            Categoria categoria = new Categoria("Jardineria");
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test","0");
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","1");
            Servicio servicio= new Servicio(104,"Servicio de jardineria",Payment.Debito,Costo.Costo_total_servicio,trabajador,categoria, 100);
            Contrato contrato= gestor.CrearContrato(empleador,servicio);
            Empleador Empleadoresperado=empleador;
            Servicio Servicioesperado=servicio;
            Assert.AreEqual(Servicioesperado,contrato.Servicio);
            Assert.AreEqual(Empleadoresperado,contrato.Empleador);
            
        }
        /// <summary>
        /// Testea que se le pueda calificar al empleador en el contrato
        /// </summary>
        [Test]
        public void TestCalificarContratoEmpleador()
        {
            Categoria categoria=new Categoria("Jardineria");
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test","0");
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","1");
            Servicio servicio= new Servicio(103,"Servicio de jardineria",Payment.Debito,Costo.Costo_total_servicio,trabajador,categoria, 100);
            Contrato contrato= new Contrato(empleador,servicio);
            Calificacion calificacion= new Calificacion();
            calificacion.AgregarCalificacion(5);
            contrato.CalificarContratoEmpleador(empleador,calificacion);
            
            int calificacionEsperada=5;
            Assert.AreEqual(calificacionEsperada,contrato.CalificacionEmpleador.Valor);
            Assert.AreEqual(calificacionEsperada,calificacion.Valor);
        }
        
        /// <summary>
        /// Testea que se le pueda calificar al empleador en el contrato
        /// </summary>
        [Test]
        public void TestCalificarContratoTrabajador()
        {
            Categoria categoria = new Categoria("Jardineria");
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test","0");
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","1");
            Servicio servicio= new Servicio(102,"Servicio de jardineria",Payment.Debito,Costo.Costo_total_servicio,trabajador,categoria, 100);
            Contrato contrato= new Contrato(empleador,servicio);
            Calificacion calificacion= new Calificacion();
            calificacion.AgregarCalificacion(5);
            contrato.CalificarContratoTrabajador(trabajador,calificacion);
            
            int calificacionEsperada=5;
            Assert.AreEqual(calificacionEsperada,contrato.CalificacionTrabajador.Valor);
            Assert.AreEqual(calificacionEsperada,calificacion.Valor);
        }
        /// <summary>
        /// Test que prueba la creacion de un contrato por hora
        /// </summary>
        [Test]
        public void TestCrearContratoHora()
        {
            Categoria categoria = new Categoria("Jardineria");
            Trabajador trabajador=new Trabajador("Test","Test","test","099999999","Test","Test","0");
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","1");
            Servicio servicio= new Servicio(101,"Servicio de jardineria",Payment.Debito,Costo.Costo_por_hora,trabajador,categoria, 100);
            decimal hora = 10;
            Contrato contrato= gestor.CrearContratoHora(empleador,servicio, hora);
            Empleador Empleadoresperado=empleador;
            Servicio Servicioesperado=servicio;
            Assert.AreEqual(Servicioesperado,contrato.Servicio);
            Assert.AreEqual(Empleadoresperado,contrato.Empleador);
            Assert.AreEqual(1000, contrato.CostoTotal);
        }

        
    
    }
}