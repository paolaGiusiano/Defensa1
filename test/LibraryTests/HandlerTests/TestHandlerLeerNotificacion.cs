using NUnit.Framework;
using Telegram.Bot.Types;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de ContratarServicio
    /// </summary>
    [TestFixture]
    public class TestHandlerLeerNotificacion
    {
        /// <summary>
        /// Gestor singleton de usuarios
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Un mensaje enviado por el empleador
        /// </summary>
        public Message messageempleador;
        /// <summary>
        /// Un mensaje enviado por el trabajador
        /// </summary>
        public Message messagetrabajador;
        /// <summary>
        /// Un mensaje enviado por el administrador
        /// </summary>
        public Message messageadministrador;
        /// <summary>
        /// Instancia del handler
        /// </summary>
        public LeerNotificacionHandler handler;
        /// <summary>
        /// Un empleador para datos de prueba
        /// </summary>
        public Empleador empleador1;
        /// <summary>
        /// Un trabajador para datos de prueba
        /// </summary>
        public Trabajador trabajador1;
        /// <summary>
        /// Un administrador para datos de prueba
        /// </summary>
        public Administrador administrador1;
        /// <summary>
        /// Una notificacion del empleador
        /// </summary>
        Notificacion notificacionempleador1;
        /// <summary>
        /// Otra notificacion del empleador
        /// </summary>
        Notificacion notificacionempelador2;
        /// <summary>
        /// Una notificacion del trabajador
        /// </summary>
        Notificacion notificaciontrabajador1;
        /// <summary>
        /// Otra notificacion del trabajador
        /// </summary>
        Notificacion notificaciontrabajador2;
        /// <summary>
        /// Una notificacion del administrador
        /// </summary>
        Notificacion notificacionadministrador1;
        /// <summary>
        /// Otra notificacion del administrador
        /// </summary>
        Notificacion notificacionadminsitrador2;


        /// <summary>
        /// Setup de los datos de prueba
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            empleador1 = gestorUsuario.AgregarEmpleador("Test", "Test", "TestEmpleador1", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "401");
            trabajador1 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador1", "09999999", "Montevideo, Francisco Miranda 4270", "12345678", "402");
            administrador1 = gestorUsuario.AgregarAdmin("Test", "Test", "TestTrabajador1", "12345678", "403");
            handler = new LeerNotificacionHandler(null);
            messageempleador = new Message();
            messageempleador.From = new User();
            messageempleador.From.Id = int.Parse(empleador1.ID);
            messagetrabajador = new Message();
            messagetrabajador.From = new User();
            messagetrabajador.From.Id = int.Parse(trabajador1.ID);
            messageadministrador = new Message();
            messageadministrador.From = new User();
            messageadministrador.From.Id = int.Parse(administrador1.ID);
        }
        /// <summary>
        /// Setup que se hace todos los tests para reiniciar el estado de las notificaciones
        /// </summary>
        [SetUp]
        public void RepetitiveSetup()
        {
            empleador1.Notificaciones = new List<Notificacion>();
            trabajador1.Notificaciones = new List<Notificacion>();
            administrador1.Notificaciones = new List<Notificacion>();

            notificacionempleador1 = new Notificacion("Notificacion sin leer del empleador");
            notificacionempelador2 = new Notificacion("Notificacion leida del empleador");
            notificacionempelador2.MarcarLeido();
            notificaciontrabajador1 = new Notificacion("Notificacion sin leer del trabajador");
            notificaciontrabajador2 = new Notificacion("Notificacion leida del trabajador");
            notificaciontrabajador2.MarcarLeido();
            notificacionadministrador1 = new Notificacion("Notificacion sin leer del admin");
            notificacionadminsitrador2 = new Notificacion("Notificacion leida del admin");
            notificacionadminsitrador2.MarcarLeido();

            empleador1.Notificaciones.Add(notificacionempleador1);
            empleador1.Notificaciones.Add(notificacionempelador2);
            trabajador1.Notificaciones.Add(notificaciontrabajador1);
            trabajador1.Notificaciones.Add(notificaciontrabajador2);
            administrador1.Notificaciones.Add(notificacionadministrador1);
            administrador1.Notificaciones.Add(notificacionadminsitrador2);
        }
        /// <summary>
        /// Test que prueba que el empleador puede leer sus notificaciones leidas
        /// </summary>
        [Test]
        public void TestEmpleadorRead()
        {
            messageempleador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messageempleador.Text = "Leidas";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadDisplay));
            Assert.That(response, Is.EqualTo("Buscando sus notificaciones leidas. Digame cuando quiera seguir."));

            messageempleador.Text = "Next";
            IHandler result2 = this.handler.Handle(messageempleador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificacionempelador2.ToString()));
        }
        /// <summary>
        /// Test que prueba que el empleador puede leer sus notificaciones no leidas
        /// </summary>
        [Test]
        public void TestEmpleadorUnread()
        {
            messageempleador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messageempleador.Text = "No leidas";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.UnreadDisplay));
            Assert.That(response, Is.EqualTo("Buscando sus notificaciones sin leer. Digame cuando quiera seguir."));

            messageempleador.Text = "Next";
            IHandler result2 = this.handler.Handle(messageempleador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificacionempleador1.ToString()));
        }
        /// <summary>
        /// Test que prueba que el empleador puede leer todas sus notificaciones
        /// </summary>
        [Test]
        public void TestEmpleadorAll()
        {
            messageempleador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messageempleador.Text = "Todas";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.AllDisplay));
            Assert.That(response, Is.EqualTo("Buscando todas sus notificaciones. Digame cuando quiera seguir."));

            messageempleador.Text = "Next";
            IHandler result2 = this.handler.Handle(messageempleador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificacionempleador1.ToString() + "\r\n" + notificacionempelador2.ToString()));
        }
        /// <summary>
        /// Test que prueba que el trabajador puede leer sus notificaciones leidas
        /// </summary>
        [Test]
        public void TestTrabajadorRead()
        {
            messagetrabajador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messagetrabajador.Text = "Leidas";
            IHandler result1 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadDisplay));
            Assert.That(response, Is.EqualTo("Buscando sus notificaciones leidas. Digame cuando quiera seguir."));

            messagetrabajador.Text = "Next";
            IHandler result2 = this.handler.Handle(messagetrabajador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificaciontrabajador2.ToString()));
        }
        /// <summary>
        /// Test que prueba que el trabajador puede leer sus notificaciones no leidas
        /// </summary>
        [Test]
        public void TestTrabajadorUnread()
        {
            messagetrabajador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messagetrabajador.Text = "No leidas";
            IHandler result1 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.UnreadDisplay));
            Assert.That(response, Is.EqualTo("Buscando sus notificaciones sin leer. Digame cuando quiera seguir."));

            messagetrabajador.Text = "Next";
            IHandler result2 = this.handler.Handle(messagetrabajador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificaciontrabajador1.ToString()));
        }
        /// <summary>
        /// Test que prueba que el trabajador puede leer todas sus notificaciones
        /// </summary>
        [Test]
        public void TestTrabajadorAll()
        {
            messagetrabajador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messagetrabajador.Text = "Todas";
            IHandler result1 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.AllDisplay));
            Assert.That(response, Is.EqualTo("Buscando todas sus notificaciones. Digame cuando quiera seguir."));

            messagetrabajador.Text = "Next";
            IHandler result2 = this.handler.Handle(messagetrabajador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificaciontrabajador1.ToString() + "\r\n" + notificaciontrabajador2.ToString()));
        }
        /// <summary>
        /// Test que prueba que el admin puede leer sus notificaciones leidas
        /// </summary>
        [Test]
        public void TestAdminRead()
        {
            messageadministrador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messageadministrador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messageadministrador.Text = "Leidas";
            IHandler result1 = this.handler.Handle(messageadministrador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadDisplay));
            Assert.That(response, Is.EqualTo("Buscando sus notificaciones leidas. Digame cuando quiera seguir."));

            messageadministrador.Text = "Next";
            IHandler result2 = this.handler.Handle(messageadministrador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificacionadminsitrador2.ToString()));
        }
        /// <summary>
        /// Test que prueba que el admin puede leer sus notificaciones no leidas
        /// </summary>
        [Test]
        public void TestAdminUnread()
        {
            messageadministrador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messageadministrador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messageadministrador.Text = "No leidas";
            IHandler result1 = this.handler.Handle(messageadministrador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.UnreadDisplay));
            Assert.That(response, Is.EqualTo("Buscando sus notificaciones sin leer. Digame cuando quiera seguir."));

            messageadministrador.Text = "Next";
            IHandler result2 = this.handler.Handle(messageadministrador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificacionadministrador1.ToString()));
        }
        /// <summary>
        /// Test que prueba que el admin puede leer todas sus notificaciones
        /// </summary>
        [Test]
        public void TestAdminAll()
        {
            messageadministrador.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(messageadministrador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.ReadPrompt));
            Assert.That(response, Is.EqualTo("Cuales notificaciones quiere ver? Leidas, No leidas o Todas"));

            messageadministrador.Text = "Todas";
            IHandler result1 = this.handler.Handle(messageadministrador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.AllDisplay));
            Assert.That(response, Is.EqualTo("Buscando todas sus notificaciones. Digame cuando quiera seguir."));

            messageadministrador.Text = "Next";
            IHandler result2 = this.handler.Handle(messageadministrador, out response);
            
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(LeerNotificacionHandler.LeerNotificacionesState.Start));
            Assert.That(response, Is.EqualTo(notificacionadministrador1.ToString() + "\r\n" + notificacionadminsitrador2.ToString()));
        }
    }
}