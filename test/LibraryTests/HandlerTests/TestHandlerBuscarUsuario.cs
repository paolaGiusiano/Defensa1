using Telegram.Bot.Types;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Clase que prueba el handler de buscar ususario
    /// </summary>
    [TestFixture]
    public class TestHandlerBuscarUsuario
    {
        /// <summary>
        /// Gestor singleton de usuarios
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Mensaje enviado por el usuario empleador
        /// </summary>
        public Message messageempleador;
        /// <summary>
        /// Mensaje enviado por el usuario trabajador
        /// </summary>
        public Message messagetrabajador;
        /// <summary>
        /// Instancia del handler
        /// </summary>
        public BuscarUsuarioHandler handler;
        /// <summary>
        /// Un empleador para datos de prueba
        /// </summary>
        public Empleador empleador1;
        /// <summary>
        /// Un trabajador para datos de prueba
        /// </summary>
        public Trabajador trabajador1;
        /// <summary>
        /// Setup para los datos de prueba
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            empleador1 = gestorUsuario.AgregarEmpleador("Test", "Test", "TestEmpleador@test.edu.uy", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "801");
            trabajador1 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador@test.edu.uy", "09999999", "Montevideo, Francisco Miranda 4270", "12345678", "802");
            handler = new BuscarUsuarioHandler(null);
            messageempleador = new Message();
            messageempleador.From = new User();
            messageempleador.From.Id = int.Parse(empleador1.ID);
        }
        /// <summary>
        /// Prueba que se puede buscar un empleador por ID
        /// </summary>
        [Test]
        public void TestBuscaEmpleadorID()
        {
            messageempleador.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BusquedaPrompt));
            Assert.That(response, Is.EqualTo("Quiere buscar por ID del usuario o por su email?"));

            messageempleador.Text = "id";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.IDPrompt));
            Assert.That(response, Is.EqualTo("Inserte ID del usuario que quiere buscar."));

            messageempleador.Text = "801";
            IHandler result2 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BuscarUsuarioID));
            Assert.That(response, Is.EqualTo("Buscando usuario. Digame cuando quiere continuar."));

            messageempleador.Text = "next";
            IHandler result3 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.Start));
            Assert.That(response, Is.EqualTo(empleador1.ToString()));
        }
        /// <summary>
        /// Prueba que se puede buscar un trabajador por ID
        /// </summary>
        [Test]
        public void TestBuscaTrabajadorID()
        {
            messageempleador.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BusquedaPrompt));
            Assert.That(response, Is.EqualTo("Quiere buscar por ID del usuario o por su email?"));

            messageempleador.Text = "id";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.IDPrompt));
            Assert.That(response, Is.EqualTo("Inserte ID del usuario que quiere buscar."));

            messageempleador.Text = "802";
            IHandler result2 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BuscarUsuarioID));
            Assert.That(response, Is.EqualTo("Buscando usuario. Digame cuando quiere continuar."));

            messageempleador.Text = "next";
            IHandler result3 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.Start));
            Assert.That(response, Is.EqualTo(trabajador1.ToString()));

        }
        /// <summary>
        /// Prueba que se puede buscar un empleador por email
        /// </summary>
        [Test]
        public void TestBuscaEmpleadorEmail()
        {
            messageempleador.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BusquedaPrompt));
            Assert.That(response, Is.EqualTo("Quiere buscar por ID del usuario o por su email?"));

            messageempleador.Text = "email";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Inserte email del usuario que quiere buscar."));

            messageempleador.Text = "TestEmpleador@test.edu.uy";
            IHandler result2 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BuscarUsuarioEmail));
            Assert.That(response, Is.EqualTo("Buscando usuario. Digame cuando quiere continuar."));

            messageempleador.Text = "next";
            IHandler result3 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.Start));
            Assert.That(response, Is.EqualTo(empleador1.ToString()));
        }
        /// <summary>
        /// Prueba que se puede buscar un trabajador por email
        /// </summary>
        [Test]
        public void TestBuscaTrabajadorEmail()
        {
            messageempleador.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BusquedaPrompt));
            Assert.That(response, Is.EqualTo("Quiere buscar por ID del usuario o por su email?"));

            messageempleador.Text = "email";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Inserte email del usuario que quiere buscar."));

            messageempleador.Text = "TestTrabajador@test.edu.uy";
            IHandler result2 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.BuscarUsuarioEmail));
            Assert.That(response, Is.EqualTo("Buscando usuario. Digame cuando quiere continuar."));

            messageempleador.Text = "next";
            IHandler result3 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarUsuarioHandler.BuscarUsuarioState.Start));
            Assert.That(response, Is.EqualTo(trabajador1.ToString()));
        }
    }
}