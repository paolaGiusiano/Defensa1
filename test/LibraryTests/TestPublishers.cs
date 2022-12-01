using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de las clases de Publisher que implementan Observer
    /// </summary>
    [TestFixture]
    public class TestPublishers
    {
        /// <summary>
        /// Gestor de servicios singleton
        /// </summary>
        public GestorServicios gestorServicios = GestorServicios.getInstance();
        /// <summary>
        /// Gestor de usuarios singleton
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Gestor de categorias singleton
        /// </summary>
        public GestorCategorias gestorCategorias = GestorCategorias.getInstance();
        /// <summary>
        /// Gestor de contratos singleton
        /// </summary>
        public GestorContratos gestorContratos = GestorContratos.getInstance();
        /// <summary>
        /// Un trabajador
        /// </summary>
        public Trabajador trabajador1;
        /// <summary>
        /// Un empleador
        /// </summary>
        public Empleador empleador1;
        /// <summary>
        /// Un admin
        /// </summary>
        public Administrador admin1;
        /// <summary>
        /// Una categoria
        /// </summary>
        public Categoria categoria1;
        /// <summary>
        /// Un servicio cargado al sistema
        /// </summary>
        public Servicio servicio1;
        /// <summary>
        /// Un contrato cargado al sistema
        /// </summary>
        public Contrato contrato1;
        
        /// <summary>
        /// Setup que ocurre solo una vez, definiendo los usuarios y categoria
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            trabajador1 = gestorUsuario.AgregarTrabajador("Trabajador", "Test", "Test@test.com", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "701");
            empleador1 = gestorUsuario.AgregarEmpleador("Empleador", "Test", "Test@test.com", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "702");
            admin1 = gestorUsuario.AgregarAdmin("Admin", "Test", "Test@test.com", "12345678", "703");
            categoria1 = gestorCategorias.CrearCategoria("Categoria", admin1);
        }
        /// <summary>
        /// Setup a repetirse cada iteracion, cargando los datos del servicio y el contrato
        /// </summary>
        [SetUp]
        public void RepetitiveSetup()
        {
            servicio1 = gestorServicios.CrearServicio("Servicio ya existente", Payment.Debito, Costo.Costo_total_servicio, trabajador1, categoria1, 110);
            contrato1 = gestorContratos.CrearContrato(empleador1, servicio1);
        }
        
        /// <summary>
        /// Prueba que cuando se crea un servicio, se subscribe el trabajador a su publisher
        /// </summary>
        [Test]
        public void TestSubscribirServicio()
        {
            Servicio nuevoServicio = gestorServicios.CrearServicio("Nuevo servicio", Payment.Transferencia, Costo.Costo_por_hora, trabajador1, categoria1, 100);
            Publisher publisher = gestorServicios.TodoPublisherServicios.Find(Publisher => Publisher.Servicio == nuevoServicio);
            Assert.Contains(trabajador1, publisher.Subscriptores);
        }
        /// <summary>
        /// Prueba que cuando se crea un contrato, el trabajador y empleador del mismo se subscriben a su publisher
        /// </summary>
        [Test]
        public void TestSubscribirContrato()
        {
            Contrato nuevoContrato = gestorContratos.CrearContrato(empleador1, servicio1);
            Publisher publisher = gestorContratos.TodoPublisherContrato.Find(Publisher => Publisher.Contrato == nuevoContrato);
            Assert.Contains(trabajador1, publisher.Subscriptores);
            Assert.Contains(empleador1, publisher.Subscriptores);
        }
        /// <summary>
        /// Prueba que cuando se remueve un servicio, se envia notificacion a los usuarios subscritos al publisher de ese servicio
        /// </summary>
        [Test]
        public void TestEnviarNotificacionServicio()
        {
            string motivo = "Servicio no cumple con las normas de la plataforma";
            string mensaje = $"El servicio {servicio1.DescripcionServicio} de ID {servicio1.Id} ha sido removido del sistema por el siguiente motivo: {motivo}. Por cualquier duda, contacte a nuestros administradores.";
            gestorServicios.RemoverServicio(servicio1, admin1, motivo);
            Notificacion notif = trabajador1.Notificaciones.Find(notif => notif.Mensaje == mensaje);
            Assert.AreEqual(mensaje, notif.Mensaje);
        }
        /// <summary>
        /// Prueba que si se evalua un contrato, se envia una notificacion a los usuarios subscritos al publisher de ese contrato
        /// </summary>
        [Test]
        public void TestEnviarNotificacionContrato()
        {
            string mensaje1 = $"El usuario {trabajador1.Nombre} {trabajador1.Apellido} ha calificado el contrato {contrato1.ID} por el servicio {contrato1.Servicio.DescripcionServicio}.";
            string mensaje2 = $"El usuario {empleador1.Nombre} {empleador1.Apellido} ha calificado el contrato {contrato1.ID} por el servicio {contrato1.Servicio.DescripcionServicio}.";
            gestorContratos.EvaluarContratoEmpleador(5, contrato1, trabajador1);
            gestorContratos.EvaluarContratoTrabajdor(1, contrato1, empleador1);
            Assert.AreEqual(mensaje1, trabajador1.Notificaciones.Find(notif => notif.Mensaje == mensaje1).Mensaje);
            Assert.AreEqual(mensaje2, trabajador1.Notificaciones.Find(notif => notif.Mensaje == mensaje2).Mensaje);
            Assert.AreEqual(mensaje1, empleador1.Notificaciones.Find(notif => notif.Mensaje == mensaje1).Mensaje);
            Assert.AreEqual(mensaje2, empleador1.Notificaciones.Find(notif => notif.Mensaje == mensaje2).Mensaje);
        }
    }
}