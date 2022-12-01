using NUnit.Framework;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de ContratarServicio
    /// </summary>
    [TestFixture]
    public class TestHandlerCrearServicio
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
        public CrearServicioHandler handler;
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
            categoria1 = gestorCategorias.CrearCategoria("Categoria", admin1);
            handler = new CrearServicioHandler(null);
            message = new Message();
            message.From = new User();
            message.From.Id = int.Parse(trabajador1.ID);

        }
        /// <summary>
        /// Prueba que un usuario puede crear un servicio
        /// </summary>
        [Test]
        public void TestCrearServicio()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.DescripcionPrompt));
            Assert.That(response, Is.EqualTo("Por favor, inserte la descripcion del servicio."));

            message.Text = "Nuevo servicio.";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.CategoriaPrompt));
            Assert.That(response, Is.EqualTo($"Ahora, por favor, elija una categoria para el servicio de la siguiente lista: {this.gestorCategorias.AllNames()}."));

            message.Text = "Categoria";
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.PagoPrompt));
            Assert.That(response, Is.EqualTo($"Para continuar, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia"));

            message.Text = "Credito";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.CostPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, agregue como el costo del servicio debe ser calculado: Por hora, A termino."));

            message.Text = "A termino";
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.CostoPrompt));
            Assert.That(response, Is.EqualTo("Para terminar, agregue el costo del servicio total en pesos."));

            message.Text = 100.ToString();
            IHandler result5 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.CrearServicio));
            Assert.That(response, Is.EqualTo("El servicio esta siendo creado. Digame cuando quiere continuar."));

            message.Text = "Next";
            IHandler result6 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(CrearServicioHandler.CrearServicioState.Start));
            Assert.That(response, Is.EqualTo($"El servicio fue creado exitosamente con los siguientes parametros. Descripcion: Nuevo servicio., Categoria: Categoria, Tipo de pago: Credito, Costo: 100"));

        }
    }
}