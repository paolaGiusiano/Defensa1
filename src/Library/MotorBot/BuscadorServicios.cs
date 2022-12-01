using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Clase de servicio que buscara los servicios basado en categorias, ordenado por calificacion o distancia
    /// Implementa patron Facade para simplificar la declaracion de funcionalidades y reducir la cantidad de acoplamiento
    /// a procesos complejos de multiples gestores
    /// </summary>
    public class BuscadorServicios
    {
        /// <summary>
        /// Lista de resultados de la busqueda
        /// </summary>
        List<Servicio> ResultadoBusqueda;
        /// <summary>
        /// Cliente de LocationAPI usado por la calculadora
        /// </summary>
        /// <returns></returns>
        public LocationApiClient client = new LocationApiClient();
        /// <summary>
        /// Singleton del gestor de Servicios
        /// </summary>
        public GestorServicios gestor = GestorServicios.getInstance();
        /// <summary>
        /// Calculadora de distancias
        /// </summary>
        /// <returns></returns>
        public DistanceCalculator calculator;
        /// <summary>
        /// Instancia de buscador servicio
        /// </summary>
        private static BuscadorServicios instance;
        
        /// <summary>
        /// Constructor vacio para clases de testing y utilidad
        /// </summary>
        private BuscadorServicios()
        {

        }

        /// <summary>
        /// Funcion de obtener instancia del singleton
        /// </summary>
        public static BuscadorServicios getInstance()
        {
            if (instance == null)
            {
                instance = new BuscadorServicios();
            }
            return instance;
        }
        
        /// <summary>
        /// Busca servicios basado en una keyword y devuelve una lista de los resultados
        /// </summary>
        /// <param name="Keyword">categoria del servicio definida por el usuario como parametro</param> 
        /// <param name="empleador">empleador buscado servicios</param>
        /// <returns>retorna el atributo ResultadoBusqueda del buscador</returns> 
        public List<Servicio> BuscarServiciosPorCategoria(string Keyword, Usuario empleador)
        {
            BotException.Postcondicion(!string.IsNullOrEmpty(Keyword), "Se debe insertar una categoria para el criterio de busqueda.");
            Autenticar.AutenticarEmpleador(empleador);
            List<Servicio> aux = new List<Servicio>();
            foreach(Servicio servicio in gestor.TodoServicios)
            {
                if(servicio.Categoria.Nombre == Keyword)
                {
                    aux.Add(servicio);
                }
            }
            BotException.Postcondicion(aux.Count!=0, "El resultado de la busqueda fue vacio.");
            return aux;
        }
        
        /// <summary>
        /// Busca un servicio basado en un keyword y devuelve una lista ordenada por la calificacion total del Trabajador del servicio
        /// </summary>
        /// <param name="Keyword">Categoria por la que se busca la calificacion</param>
        /// <param name="empleador">Usuario que busca los Servicios</param>
        /// <returns></returns>//  
        public List<Servicio> BuscarOrdenCalificacion(string Keyword, Usuario empleador)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(Keyword), "Se debe insertar una categoria para el criterio de busqueda.");
            Autenticar.AutenticarEmpleador(empleador);
            List<Servicio> CopiaResultado;
            List<Servicio> aux;
            CopiaResultado = this.BuscarServiciosPorCategoria(Keyword, empleador);
            aux = CopiaResultado.OrderByDescending(Servicio=>Servicio.Trabajador.CalificacionTotal).ToList();
            BotException.Postcondicion(CopiaResultado.Count!=0, "El resultado de la busqueda fue vacio.");
            return aux;
        }

        /// <summary>
        /// Busca un servicio basado en un keyword y devuelve una lista ordenada por la direccion del Trabajador del servicio
        /// Debido a que la distancia seria medida por un API Geolocator, se espera poder usar el Servicio.Trabajador.Direccion como argumento de un metodo que calcule distancia para la 3ra entrega
        /// </summary>
        /// <param name="Keyword"> es una categoria de servicio definida por el usuario como parametro</param>
        /// <param name="empleador"> es el empleador solicitando el servicio</param>
        public List<Servicio> BuscarOrdenDistancia(string Keyword, Usuario empleador)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(Keyword), "Se debe insertar una categoria para el criterio de busqueda.");
            Autenticar.AutenticarEmpleador(empleador);
            this.calculator = new DistanceCalculator(client);
            List<Servicio> CopiaResultado = new List<Servicio>();
            CopiaResultado = this.BuscarServiciosPorCategoria(Keyword, empleador);
            CopiaResultado.Sort((a, b) => this.calculator.CalculateDistance(a.Trabajador.Direccion,empleador.Direccion).Distance.CompareTo(this.calculator.CalculateDistance(b.Trabajador.Direccion,empleador.Direccion).Distance));
            BotException.Postcondicion(CopiaResultado.Count!=0, "El resultado de la busqueda fue vacio.");
            return CopiaResultado;
        }
        /// <summary>
        /// Muestra un servicio unico basado en su informacion
        /// </summary>
        /// <param name="DescripcionServicio"></param>
        /// <param name="categoria"></param>
        /// <param name="pago"></param>
        /// <param name="cost"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Servicio MostrarServicio(string DescripcionServicio, Categoria categoria, Payment pago, Costo cost, string email)
        {
            Servicio servicio = null;
            BotException.Precondicion(!string.IsNullOrEmpty(DescripcionServicio) && categoria != null && !string.IsNullOrEmpty(email), "Los parametros del servicio no pueden ser vacios");
            servicio = gestor.TodoServicios.Find(s => s.DescripcionServicio == DescripcionServicio && s.Categoria.Nombre == categoria.Nombre && s.Pago == pago && s.Cost == cost && s.Trabajador.Email == email);
            
            return servicio;
        }
        /// <summary>
        /// Buscador de servicios por ID
        /// </summary>
        /// <param name="Id">ID del servicio determinada por el gestor de servicios</param>
        /// <returns>El servicio encontrado</returns>
        public Servicio BuscarServicioUnico(int Id)
        {
            Servicio servicio = null;
            BotException.Precondicion(Id > 0, "Id entrada no valida.");
            servicio = gestor.TodoServicios.Find(s => s.Id == Id);
            BotException.Postcondicion(servicio != null, "No se encontro el servicio");

            return servicio;
        }
    }
}