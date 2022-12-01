using System;
using System.Collections.Generic;
using NUnit.Framework;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de ContratarServicio
    /// </summary>
    [TestFixture]
    public class TestHandlerRemoverServicio
    {
        /// <summary>
        /// Gestor singleton de usuarios
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Gestor singleton de servicios
        /// </summary>
        public GestorServicios gestorServicios = GestorServicios.getInstance();
        /// <summary>
        /// Gestor singleton de categorias
        /// </summary>
        public GestorCategorias gestorCategorias = GestorCategorias.getInstance();
        /// <summary>
        /// Mensaje enviado por el usuario
        /// </summary>
        public Message message;
        /// <summary>
        /// Instancia del handler
        /// </summary>
        public RemoverServicioHandler handler;
        /// <summary>
        /// Servicio que cobra en base de hora
        /// </summary>
        public Servicio servicioHora;
        /// <summary>
        /// Servicio que cobra a termino
        /// </summary>
        public Servicio servicioTermino;
        /// <summary>
        /// Administrador del sistema
        /// </summary>
        public Administrador administrador;
        /// <summary>
        /// Un trabajador del sistema
        /// </summary>
        public Trabajador trabajador1;
        /// <summary>
        /// Un empleador del sistema
        /// </summary>
        public Empleador empleador1;
        /// <summary>
        /// Una categoria del sistema
        /// </summary>
        public Categoria categoria1;
        /// <summary>
        /// Buscador de servicios singleton
        /// </summary>
        public BuscadorServicios buscadorServicios;
        /// <summary>
        /// Setup de data de testing
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            empleador1 = gestorUsuario.AgregarEmpleador("Test", "Test", "TestEmpleador", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "901");
            trabajador1 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador", "09999999", "Montevideo, Francisco Miranda 4270", "12345678", "902");
            administrador = gestorUsuario.AgregarAdmin("test", "test", "test", "12345678", "903");
            categoria1 = gestorCategorias.CrearCategoria("Categoria", administrador);
            servicioHora = gestorServicios.CrearServicio("Servicio por hora", Payment.Debito, Costo.Costo_por_hora, trabajador1, categoria1, 100);
            servicioTermino = gestorServicios.CrearServicio("Servicio con pago a termino", Payment.Debito, Costo.Costo_total_servicio, trabajador1, categoria1, 20);
            handler = new RemoverServicioHandler(null);
            message = new Message();
            message.From = new User();
            message.From.Id = int.Parse(administrador.ID);

        }
        /// <summary>
        /// Prueba que un usuario puede contratar un servicio por hora simplemente sabiendo el ID del mismo
        /// </summary>
        /// <value></value>
        [Test]
        public void TestRemoverServicio()
        {
            message.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RemoverServicioHandler.RemoverServicioState.IDServicioPrompt));
            Assert.That(response, Is.EqualTo("Por favor, elija el ID del servicio que desea remover."));

            message.Text = servicioHora.Id.ToString();
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RemoverServicioHandler.RemoverServicioState.MotivoPrompt));
            Assert.That(response, Is.EqualTo("Por favor, explique el motivo por el que se esta removiendo el servicio."));

            message.Text = "Servicio invalido";
            IHandler result2 = this.handler.Handle(message, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RemoverServicioHandler.RemoverServicioState.Start));
            Assert.That(response, Is.EqualTo("El servicio ha sido removido con exito."));
        }
    }
}