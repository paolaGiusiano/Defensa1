using NUnit.Framework;
using Telegram.Bot.Types;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// CLase de Test
    /// </summary>
    [TestFixture]
    public class TestHandlerEvaluarContrato
    {   
        /// <summary>
        /// Singleton gestion usuarios
        /// </summary>
        /// <returns></returns>
        public GestorUsuario users = GestorUsuario.getInstance();
        /// <summary>
        /// Instancia singleton para buscar los contratos antes de calificarlos
        /// </summary>
        /// <returns></returns>
        public GestorContratos contratos = GestorContratos.getInstance();
        /// <summary>
        /// Instancia Singleton para traerme el usuario
        /// </summary>
        /// <returns></returns>
        public BuscadorUsuarios buscadorUsuario = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Mensaje enviado por el usuario
        /// </summary>
        public Message message;
        
        /// <summary>
        /// Handler base para el test
        /// </summary>
        /// <returns></returns>
        public EvaluarContratoHandler handler = new EvaluarContratoHandler(null);

        /// <summary>
        /// Configuracion que se hace una vez sola
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            message = new Message();
            message.Text = "evaluar contrato";
            Empleador e = new Empleador("Test", "Test", "TestEmpleador1", "0999999", "Montevideo, Francisco Miranda 4270", "12345678", "666");
            contratos.CrearContrato(e, new Servicio(999,"Test",Payment.Debito, Costo.Costo_por_hora, new Trabajador(), new Categoria("Test"), 100m));
            users.AgregarEmpleador("Ronald","Araujo", "ronald@mail.com","099999999", "Galicia 2244" ,"laCeleste", "666");

        }
        /// <summary>
        /// Evalua la calificacion del contrato.
        /// </summary>
        [Test]
        public void TestEvaluarContrato()
        {
            //Comienza a trabajar el Handler        
            string response = "";
            handler.Handle(message, out response);

            Assert.That(handler.State, Is.EqualTo(EvaluarContratoState.Usuario));

            Telegram.Bot.Types.User e = new User();
            e.Id = 666;
            message.From = e;
            IHandler step1 = handler.Handle(message, out response);
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(EvaluarContratoState.RecuperarContrato));
            Assert.That(response, Is.EqualTo("Ingrese un numero de id del contrato que desea evaluar."));
            
            message.Text = System.DateTime.Now.ToString();
            IHandler step2 = handler.Handle(message, out response);
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(EvaluarContratoState.Calificacion));
            Assert.That(response, Is.EqualTo("Ingrese un numero de 1 a 5, para calificar."));

            message.Text = "5";
            IHandler step3 = handler.Handle(message, out response);
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(EvaluarContratoState.EvaluarServicio));
            Assert.That(response, Is.EqualTo("Se evaluó el servicio con exito."));



        }






    }












}