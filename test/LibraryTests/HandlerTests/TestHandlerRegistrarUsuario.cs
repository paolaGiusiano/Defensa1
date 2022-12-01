using NUnit.Framework;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de RegistrarUsuario
    /// </summary>
    [TestFixture]
    public class TestHandlerRegistrarUsuario
    {
        /// <summary>
        /// Buscador de usuarios singleton
        /// </summary>
        /// <returns></returns>
        public BuscadorUsuarios buscadorUsuarios = new BuscadorUsuarios();
        /// <summary>
        /// Handler de RegistrarUsuario
        /// </summary>
        public RegistrarUsuarioHandler handler;
        /// <summary>
        /// Mensaje enviado por un prospecto empleador
        /// </summary>
        /// <returns></returns>
        public Message messageempleador = new Message();
        /// <summary>
        /// Mensaje enviado por un prospecto trabajador
        /// </summary>
        /// <returns></returns>
        public Message messagetrabajador = new Message();
        /// <summary>
        /// Setup que ocurre solo una vez
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            messageempleador.From = new User();
            messageempleador.From.Id = int.Parse("501");
            messagetrabajador.From = new User();
            messagetrabajador.From.Id = int.Parse("502");
            handler = new RegistrarUsuarioHandler(null);
        }
        /// <summary>
        /// Prueba que un usuario de telegram puede registrarse como empleador
        /// </summary>
        [Test]
        public void TestRegistrarEmpleador()
        {
            messageempleador.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.ProfilePrompt));
            Assert.That(response, Is.EqualTo("Por favor defina que tipo de usuario quiere registrar: empleador o trabajador"));

            messageempleador.Text = "empleador";
            IHandler result1 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.NombrePrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el nombre del usuario a registrar."));

            messageempleador.Text = "Andres";
            IHandler result2 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.ApellidoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el apellido del usuario a registrar."));

            messageempleador.Text = "Arancio";
            IHandler result3 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el email del usuario a registrar."));

            messageempleador.Text = "test@test.com";
            IHandler result4 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.PasswordPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte la contraseña del usuario a registrar. Debe ser al menos 8 caracteres."));

            messageempleador.Text = "12345678";
            IHandler result5 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.DireccionPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte la direccion del usuario a registrar en el siguiente formato: Departamento, Calle numero de edificio"));

            messageempleador.Text = "Montevideo, Francisco Miranda 4270";
            IHandler result6 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.TelefonoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el telefono de contacto del usuario a registrar."));

            messageempleador.Text = "099999999";
            IHandler result7 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.RegistrarUsuario));
            Assert.That(response, Is.EqualTo("Creando usuario. Cuando quiera continuar, digamelo."));

            messageempleador.Text = "Next";
            IHandler result8 = this.handler.Handle(messageempleador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.Start));
            Assert.That(response, Is.EqualTo($"Se ha creado el empleador Andres Arancio con el ID 501."));
            Assert.IsTrue(buscadorUsuarios.GetPersona("501") is Empleador);
        }
        /// <summary>
        /// Prueba que un usuario de telegram puede registrarse como trabajador
        /// </summary>
        [Test]
        public void TestRegistrarTrabajador()
        {
            messagetrabajador.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.ProfilePrompt));
            Assert.That(response, Is.EqualTo("Por favor defina que tipo de usuario quiere registrar: empleador o trabajador"));

            messagetrabajador.Text = "trabajador";
            IHandler result1 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.NombrePrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el nombre del usuario a registrar."));

            messagetrabajador.Text = "Andres";
            IHandler result2 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.ApellidoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el apellido del usuario a registrar."));

            messagetrabajador.Text = "Arancio";
            IHandler result3 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el email del usuario a registrar."));

            messagetrabajador.Text = "test@test.com";
            IHandler result4 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.PasswordPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte la contraseña del usuario a registrar. Debe ser al menos 8 caracteres."));

            messagetrabajador.Text = "12345678";
            IHandler result5 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.DireccionPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte la direccion del usuario a registrar en el siguiente formato: Departamento, Calle numero de edificio"));

            messagetrabajador.Text = "Montevideo, Francisco Miranda 4270";
            IHandler result6 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.TelefonoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, por favor inserte el telefono de contacto del usuario a registrar."));

            messagetrabajador.Text = "099999999";
            IHandler result7 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.RegistrarUsuario));
            Assert.That(response, Is.EqualTo("Creando usuario. Cuando quiera continuar, digamelo."));

            messagetrabajador.Text = "Next";
            IHandler result8 = this.handler.Handle(messagetrabajador, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(RegistrarUsuarioHandler.RegistrarUsuarioState.Start));
            Assert.That(response, Is.EqualTo($"Se ha creado el trabajador Andres Arancio con el ID 502."));
            Assert.IsTrue(buscadorUsuarios.GetPersona("502") is Trabajador);
        }
    }
}