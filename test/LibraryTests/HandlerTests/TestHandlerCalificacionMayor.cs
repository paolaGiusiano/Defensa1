using Telegram.Bot.Types;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Clase que prueba el handler de buscar ususario
    /// </summary>
    [TestFixture]
    public class TestCalificacionMayor
    {

        public BuscadorUsuarios buscadorUsuarios = new BuscadorUsuarios();
   
        public MayorCalificacion2 m;
      
        public Message message = new Message();

        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();

    

        [OneTimeSetUp]
        public void Setup()
        {
            Administrador newAdmin = gestorUsuario.AgregarAdmin("Paola", "Giusiano", "ppp@gmail.com", "333", "444");
            Empleador empleador =gestorUsuario.AgregarEmpleador("Jose", "Varela", "aaa@gmail.com","11111111", "mercedes y gaboto","222","111");
            Trabajador trabajador = gestorUsuario.AgregarTrabajador("Pp", "Morales", "ccc@gmail.com","22222222", "18 y gaboto","2222","222");
            Servicio servicio = GestorServicios.getInstance().CrearServicio("Jrdineria", Payment.Debito, Costo.Costo_por_hora,trabajador, new Categoria("Otros"),1m);
            GestorContratos.getInstance().CrearContrato(empleador,servicio);
            Servicio servicio1 = GestorServicios.getInstance().CrearServicio("Limpieza", Payment.Debito, Costo.Costo_por_hora,trabajador, new Categoria("Otros"),1m);
            GestorContratos.getInstance().CrearContrato(empleador,servicio1);
            empleador.AgregarCalificacion(new Calificacion().AgregarCalificacion(5));
            empleador.AgregarCalificacion(new Calificacion().AgregarCalificacion(1));

            

            message.From = new User();
            message.From.Id = int.Parse("444");
            m = new MayorCalificacion2(null);
        }
    }
}