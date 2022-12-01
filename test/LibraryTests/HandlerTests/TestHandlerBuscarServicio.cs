using NUnit.Framework;
using Telegram.Bot.Types;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba el handler de ContratarServicio
    /// </summary>
    [TestFixture]
    public class TestHandlerBuscarServicio
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
        /// Gestor singleton de servicios
        /// </summary>
        public GestorServicios gestorServicios = GestorServicios.getInstance();
        /// <summary>
        /// Mensaje enviado por el usuario
        /// </summary>
        public Message message;
        /// <summary>
        /// Instancia del handler
        /// </summary>
        public BuscarServicioHandler handler;
        /// <summary>
        /// Administrador que crea las categorias
        /// </summary>
        public Administrador admin1;
        /// <summary>
        /// Un empleador para datos de prueba
        /// </summary>
        public Empleador empleador1;
        /// <summary>
        /// Un trabajador que vive lejos del empleador y no tiene calificaciones
        /// </summary>
        public Trabajador trabajador0;
        /// <summary>
        /// Un trabajador que vive cerca del empleador y tiene bajas calificaciones
        /// </summary>
        public Trabajador trabajador1;
        /// <summary>
        /// Un trabajador que vive lejos del empleador y tiene altas calificaciones
        /// </summary>
        public Trabajador trabajador2;
        /// <summary>
        /// Una categoria para datos de prueba
        /// </summary>
        public Categoria categoria1;
        /// <summary>
        /// Un servicio para datos de prueba proveido por trabajador 0
        /// </summary>
        public Servicio servicio0;
        /// <summary>
        /// Un servicio para datos de prueba proveido por trabajador1
        /// </summary>
        public Servicio servicio1;
        /// <summary>
        /// Un servicio para datos de prueba proveido por trabajador2
        /// </summary>
        public Servicio servicio2;
        /// <summary>
        /// Una primera calificacion para trabajador1
        /// </summary>
        public Calificacion calificacion11 = new Calificacion();
        /// <summary>
        /// Una segunda calificacion para trabajador1
        /// </summary>
        public Calificacion calificacion12 = new Calificacion();
        /// <summary>
        /// Una primera calificacion para trabajador2
        /// </summary>
        public Calificacion calificacion21 = new Calificacion();
        /// <summary>
        /// Una segunda calificacion para trabajador2
        /// </summary>
        public Calificacion calificacion22 = new Calificacion();

        /// <summary>
        /// Setup para los datos de prueba
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            admin1 = gestorUsuario.AgregarAdmin("Test", "Test", "TestAdministrador", "12345678", "300");
            empleador1 = gestorUsuario.AgregarEmpleador("Test", "Test", "TestEmpleador1", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "301");
            trabajador0 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador0", "09999999", "Montevideo, Miguel Barreiro 3110", "12345678", "302");
            trabajador1 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador1", "09999999", "Montevideo, Francisco Miranda 4343", "12345678", "303");
            trabajador2 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador2", "09999999", "Montevideo, Miguel Barreiro 3110", "12345678", "304");
            categoria1 = gestorCategorias.CrearCategoria("Categoria",admin1);
            servicio0 = gestorServicios.CrearServicio("Servicio lejos del empleador con baja calificacion", Payment.Debito, Costo.Costo_total_servicio, trabajador0, categoria1, 100);
            servicio1 = gestorServicios.CrearServicio("Servicio cerca del empleador con baja calificacion", Payment.Debito, Costo.Costo_total_servicio, trabajador1, categoria1, 100);
            servicio2 = gestorServicios.CrearServicio("Servicio lejos del empleador con alta calificacion", Payment.Debito, Costo.Costo_total_servicio, trabajador2, categoria1, 100);
            calificacion11.AgregarCalificacion(3);
            calificacion12.AgregarCalificacion(3);
            calificacion21.AgregarCalificacion(5);
            calificacion22.AgregarCalificacion(5);
            trabajador1.AgregarCalificacion(calificacion11);
            trabajador1.AgregarCalificacion(calificacion12);
            trabajador2.AgregarCalificacion(calificacion21);
            trabajador2.AgregarCalificacion(calificacion22);
            handler = new BuscarServicioHandler(null);
            message = new Message();
            message.From = new User();
            message.From.Id = int.Parse(empleador1.ID);

        }
        /// <summary>
        /// Prueba que un usuario puede buscar un servicio sin ordenarlo
        /// </summary>
        [Test]
        public void TestBuscarServicioSinOrden()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.KeywordPrompt));
            Assert.That(response, Is.EqualTo("Por favor, inserte la categoria por la que se buscara el servicio."));

            message.Text = "Categoria";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.PreOrdenPrompt));
            Assert.That(response, Is.EqualTo("Desea ordernar el resultado de busqueda?"));

            message.Text = "no";
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.BuscarServicios));
            Assert.That(response, Is.EqualTo("Buscando servicio. Digame cuando quiere continuar."));

            message.Text = "Next";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.Start));
            Assert.That(response, Is.EqualTo($"\r\n{servicio0.ToString()}\r\n{servicio1.ToString()}\r\n{servicio2.ToString()}"));

        }
        /// <summary>
        /// Prueba que un usuario puede buscar un servicio ordenado por calificaion
        /// </summary>
        [Test]
        public void TestBuscarServicioCalificacion()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.KeywordPrompt));
            Assert.That(response, Is.EqualTo("Por favor, inserte la categoria por la que se buscara el servicio."));

            message.Text = "Categoria";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.PreOrdenPrompt));
            Assert.That(response, Is.EqualTo("Desea ordernar el resultado de busqueda?"));

            message.Text = "si";
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.OrdenPrompt));
            Assert.That(response, Is.EqualTo("Como desea ordenar el resultado, por calificacion del proveedor o distancia entre usted y el proveedor?"));

            message.Text = "calificacion";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.BuscarServicios));
            Assert.That(response, Is.EqualTo("Buscando servicio. Digame cuando quiere continuar."));

            message.Text = "Next";
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.Start));
            Assert.That(response, Is.EqualTo($"\r\n{servicio2.ToString()}\r\n{servicio1.ToString()}\r\n{servicio0.ToString()}"));
        }
        /// <summary>
        /// Prueba que un usuario puede buscar un servicio ordenado por distancia
        /// </summary>
        [Test]
        public void TestBuscarServicioDistancia()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.KeywordPrompt));
            Assert.That(response, Is.EqualTo("Por favor, inserte la categoria por la que se buscara el servicio."));

            message.Text = "Categoria";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.PreOrdenPrompt));
            Assert.That(response, Is.EqualTo("Desea ordernar el resultado de busqueda?"));

            message.Text = "si";
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.OrdenPrompt));
            Assert.That(response, Is.EqualTo("Como desea ordenar el resultado, por calificacion del proveedor o distancia entre usted y el proveedor?"));

            message.Text = "distancia";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.BuscarServicios));
            Assert.That(response, Is.EqualTo("Buscando servicio. Digame cuando quiere continuar."));

            message.Text = "Next";
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(BuscarServicioHandler.BuscarServicioState.Start));
            Assert.That(response, Is.EqualTo($"\r\n{servicio1.ToString()}\r\n{servicio0.ToString()}\r\n{servicio2.ToString()}"));
        }
    }
}