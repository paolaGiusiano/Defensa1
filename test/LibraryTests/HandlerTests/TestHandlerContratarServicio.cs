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
    public class TestHandlerContratarServicio
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
        public ContratarServicioHandler handler;
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
        /// Un servicio con pago por hora para datos de prueba
        /// </summary>
        public Servicio servicioHora;
        /// <summary>
        /// Un servicio con pago a termino para datos de prueba
        /// </summary>
        public Servicio servicioTermino;
        
        /// <summary>
        /// Setup para los datos de prueba
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            admin1 = gestorUsuario.AgregarAdmin("Test", "Test", "TestAdministrador", "12345678", "200");
            empleador1 = gestorUsuario.AgregarEmpleador("Test", "Test", "TestEmpleador", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "201");
            trabajador1 = gestorUsuario.AgregarTrabajador("Test", "Test", "TestTrabajador", "09999999", "Montevideo, Francisco Miranda 4270", "12345678", "202");
            categoria1 = gestorCategorias.CrearCategoria("Categoria", admin1);
            servicioHora = gestorServicios.CrearServicio("Servicio por hora", Payment.Debito, Costo.Costo_por_hora, trabajador1, categoria1, 100);
            servicioTermino = gestorServicios.CrearServicio("Servicio con pago a termino", Payment.Debito, Costo.Costo_total_servicio, trabajador1, categoria1, 20);
            handler = new ContratarServicioHandler(null);
            message = new Message();
            message.From = new User();
            message.From.Id = int.Parse(empleador1.ID);

        }
        /// <summary>
        /// Prueba que un usuario puede contratar un servicio por hora simplemente sabiendo el ID del mismo
        /// </summary>
        /// <value></value>
        [Test]
        public void TestContratarHoraPorID()
        {
            message.Text = handler.Keywords[0];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.BusquedaServicioPrompt));
            Assert.That(response, Is.EqualTo("Conoce el ID del servicio que desea contratar?"));

            message.Text = "si";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(result1, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.IdPrompt));
            Assert.That(response, Is.EqualTo("Por favor, inserte el ID del servicio que quiere buscar."));

            message.Text = servicioHora.Id.ToString();
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(result2, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.HoraPrompt));
            Assert.That(response, Is.EqualTo("Como el servicio que esta contratando cobra por hora, por favor inserte la cantidad de horas por las que esta contratando."));

            message.Text = "10";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(result3, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.ContratarServicio));
            Assert.That(response, Is.EqualTo($"Se esta creando su contrato. Digame cuando quiera continuar."));

            message.Text = "Next";
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(result4, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.Start));
            Assert.That(response, Is.EqualTo($"Se ha creado el contrato {DateTime.Now} por 10 horas a su nombre con {servicioHora.Trabajador.Nombre} {servicioHora.Trabajador.Apellido} por {10 * servicioHora.Costo}."));
        }

        /// <summary>
        /// Prueba que el usuario puede contratar un servicio a termino sabiendo su ID
        /// </summary>
        [Test]
        public void TestContratarTerminoPorID()
        {
            message.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.BusquedaServicioPrompt));
            Assert.That(response, Is.EqualTo("Conoce el ID del servicio que desea contratar?"));

            message.Text = "si";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(result1, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.IdPrompt));
            Assert.That(response, Is.EqualTo("Por favor, inserte el ID del servicio que quiere buscar."));
            
            message.Text = servicioTermino.Id.ToString();
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(result2, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.ContratarServicio));
            Assert.That(response, Is.EqualTo($"Se esta creando su contrato. Digame cuando quiera continuar."));

            message.Text = "Next";
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(result3, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.Start));
            Assert.That(response, Is.EqualTo($"Se ha creado el contrato {DateTime.Now} con pago a termino a su nombre con {trabajador1.Nombre} {trabajador1.Apellido} por {servicioTermino.Costo}."));
        }

        /// <summary>
        /// Prueba que el usuario puede contratar un servicio por hora sin saber su ID pero sabiendo todos sus otros parametros
        /// </summary>
        [Test]
        public void TestContratarHoraPorParam()
        {
            message.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.BusquedaServicioPrompt));
            Assert.That(response, Is.EqualTo("Conoce el ID del servicio que desea contratar?"));

            message.Text = "no";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.DescripcionPrompt));
            Assert.That(response, Is.EqualTo("Okay, no hay problema, busquemos el servicio por sus parametros entonces. Por favor inserte la descripcion del servicio."));

            message.Text = servicioHora.DescripcionServicio;
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.CategoriaPrompt));
            Assert.That(response, Is.EqualTo($"Ahora, por favor, elija una categoria para el servicio de la siguiente lista: {this.handler.gestorCategorias.AllNames()}."));

            message.Text = categoria1.Nombre;
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.PagoPrompt));
            Assert.That(response, Is.EqualTo($"Para continuar, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia"));

            message.Text = servicioHora.Pago.ToString();
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.CostoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, agregue como el costo del servicio debe ser calculado: Por hora, A termino."));

            message.Text = "Por hora";
            IHandler result5 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Inserte el email del proveedor del servicio."));

            message.Text = trabajador1.Email;
            IHandler result6 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.HoraPrompt));
            Assert.That(response, Is.EqualTo("Como el servicio que esta contratando cobra por hora, por favor inserte la cantidad de horas por las que esta contratando."));

            message.Text = "20";
            IHandler result7 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.ContratarServicio));
            Assert.That(response, Is.EqualTo($"Se esta creando su contrato. Digame cuando quiera continuar."));

            message.Text = "Next";
            IHandler result8 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.Start));
            Assert.That(response, Is.EqualTo($"Se ha creado el contrato {DateTime.Now} por 20 horas a su nombre con {servicioHora.Trabajador.Nombre} {servicioHora.Trabajador.Apellido} por {20 * servicioHora.Costo}."));
        }

        /// <summary>
        /// Prueba que un usuario puede contratar un servicio con pago a termino sin saber su ID pero sabiendo todos sus otros parametros
        /// </summary>
        [Test]
        public void TestContratarTerminoPorParam()
        {
            message.Text = handler.Keywords[1];
            string response;
            this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.BusquedaServicioPrompt));
            Assert.That(response, Is.EqualTo("Conoce el ID del servicio que desea contratar?"));

            message.Text = "no";
            IHandler result1 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.DescripcionPrompt));
            Assert.That(response, Is.EqualTo("Okay, no hay problema, busquemos el servicio por sus parametros entonces. Por favor inserte la descripcion del servicio."));
            
            message.Text = servicioTermino.DescripcionServicio;
            IHandler result2 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.CategoriaPrompt));
            Assert.That(response, Is.EqualTo($"Ahora, por favor, elija una categoria para el servicio de la siguiente lista: {this.handler.gestorCategorias.AllNames()}."));

            message.Text = categoria1.Nombre;
            IHandler result3 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.PagoPrompt));
            Assert.That(response, Is.EqualTo($"Para continuar, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia"));

            message.Text = servicioTermino.Pago.ToString();
            IHandler result4 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.CostoPrompt));
            Assert.That(response, Is.EqualTo("Siguiente, agregue como el costo del servicio debe ser calculado: Por hora, A termino."));

            message.Text = "A termino";
            IHandler result5 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.EmailPrompt));
            Assert.That(response, Is.EqualTo("Inserte el email del proveedor del servicio."));

            message.Text = trabajador1.Email;
            IHandler result6 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.ContratarServicio));
            Assert.That(response, Is.EqualTo($"Se esta creando su contrato. Digame cuando quiera continuar."));
            
            message.Text = "Next";
            IHandler result7 = this.handler.Handle(message, out response);

            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(ContratarServicioHandler.ContratarServicioState.Start));
            Assert.That(response, Is.EqualTo($"Se ha creado el contrato {DateTime.Now} con pago a termino a su nombre con {servicioTermino.Trabajador.Nombre} {servicioTermino.Trabajador.Nombre} por 20."));
        }
    }
}