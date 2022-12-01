using NUnit.Framework;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de Agregar Admin
    /// </summary>
    [TestFixture]
    public class TestHandlerAgregarAdmin
    {
        /// <summary>
        /// Buscador de usuarios singleton
        /// </summary>
        /// <returns></returns>
        public BuscadorUsuarios buscadorUsuarios = new BuscadorUsuarios();
        /// <summary>
        /// Handler de RegistrarUsuario
        /// </summary>
        public AgregarAdminHandler handler;
        /// <summary>
        /// Mensaje enviado por un prospecto empleador
        /// </summary>
        /// <returns></returns>
        public Message message = new Message();
        /// <summary>
        /// Mensaje enviado por un prospecto trabajador
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            message.From = new User();
            message.From.Id = int.Parse("1001");
            handler = new AgregarAdminHandler(null);
        }
        /// <summary>
        /// Prueba que un usuario de telegram puede registrarse como empleador
        /// </summary>
        [Test]
        public void TestAgregarAdminPorSecret()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.SecretPrompt));
            Assert.That(response, Is.EqualTo("La creacion de administradores esta restringida a otros admins y miembros del equipo de desarrollo. Inserte master password o su password personal."));

            message.Text = "P2Bot";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.NombrePrompt));
            Assert.That(response, Is.EqualTo("Por favor introduzca el Nombre del Administrador"));

            message.Text = "Andres";
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.ApellidoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, introduzca el Apellido del Administrador"));

            message.Text = "Arancio";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, introduzca el Email del Administrador"));

            message.Text = "test@test.com";
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.PasswordPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, introduzca la password del Administrador. Recuerde que debe tener 8 o mas caracteres."));

            message.Text = "12345678";
            IHandler result5 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.CrearAdmin));
            Assert.That(response, Is.EqualTo("Creando Admin. Escriba Next o cualquier mensaje para continuar."));

            message.Text = "Next";
            IHandler result6 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AgregarAdminHandler.AgregarAdminState.Start));
            Assert.That(response, Is.EqualTo($"Admin Andres Arancio creado."));
            Assert.IsTrue(buscadorUsuarios.GetPersona("1001") is Administrador);
        }
    }
}