using NUnit.Framework;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de Crear categoria 
    /// /// </summary>
    [TestFixture]
    public class TestHandlerCrearCategoria
    {
        /// <summary>
        /// Gestor singleton de usuarios
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
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
        public CrearCategoriaHandler handler;
        /// <summary>
        /// Administrador que crea categorias
        /// </summary>
        public Administrador admin1;
        /// <summary>
        /// Un empleador para datos de prueba
        /// </summary>
        public Empleador empleador1;
        /// <summary>
        /// Un trabajador para datos de prueba
        /// </summary>
        public Trabajador trabajador1;
        /// <summary>
        /// Una categoria para datos de prueba
        /// </summary>
        public Categoria categoria1;

        /// <summary>
        /// Setup para los datos de prueba
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            admin1 = gestorUsuario.AgregarAdmin("Test", "Test", "TestAdministrador", "12345678", "600");
            empleador1 = gestorUsuario.AgregarEmpleador("Test", "Test", "TestEmpleador1", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "601");
            trabajador1 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador1", "09999999", "Montevideo, Francisco Miranda 4270", "12345678", "602");
            categoria1 = gestorCategorias.CrearCategoria("Jardinería", admin1);
            handler = new CrearCategoriaHandler(null);
            message = new Message();
            message.From = new User();
            message.From.Id = int.Parse(admin1.ID);

        }
        /// <summary>
        /// Prueba que un administrador puede crear una categoría nueva
        /// </summary>
        [Test]
        public void TestCrearCategoriaInicial()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearCategoriaHandler.CrearCategoriaState.CategoriaNombrePrompt));
            Assert.That(response, Is.EqualTo("Por favor, elija el nombre de la categoria."));
            
            message.Text = "Conductor";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearCategoriaHandler.CrearCategoriaState.Start));
            Assert.That(response, Is.EqualTo($"Su categoria: {message.Text} ha sido creada con éxito."));

        }
        /// <summary>
        /// Prueba que un administrador no puede crear una categoria ya existente
        /// </summary>
        [Test]
        public void TestCrearCategoriaRepetida()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearCategoriaHandler.CrearCategoriaState.CategoriaNombrePrompt));
            Assert.That(response, Is.EqualTo("Por favor, elija el nombre de la categoria."));
            
            message.Text = "Jardinería";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearCategoriaHandler.CrearCategoriaState.CategoriaNombrePrompt));
            Assert.That(response, Is.EqualTo("La categoria ya existe."));

        }
        



    }
}