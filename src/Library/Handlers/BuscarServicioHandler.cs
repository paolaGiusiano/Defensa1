using Telegram.Bot.Types;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "buscar servicio"
    /// </summary>
    public class BuscarServicioHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public BuscarServicioState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public BuscarServicioData Data {get; set;}
        /// <summary>
        /// Buscador de servicios singleton que busca un servicios en la lista de todos los servicios
        /// </summary>
        public BuscadorServicios buscadorServicios = BuscadorServicios.getInstance();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public BuscarServicioHandler(BaseHandler next)
            : base(new string[] {"buscar servicio", "buscar un servicio"}, next)
        {
            this.State = BuscarServicioState.Start;
            this.Data = new BuscarServicioData();
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar</returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == BuscarServicioState.Start)
            {
                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Procesa todos los mensajes y retorna true siempre.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            BotException.Precondicion(buscadorUsuarios.GetPersona(message.From.Id.ToString()) is Empleador, "Usuario no tiene acceso a esta funcionalidad.");
            if(string.Equals(message.Text, "cancelar", System.StringComparison.InvariantCultureIgnoreCase))
            {
                response = "Cancelando operacion.";
                Cancel();
            }
            else
            {
                response = "";
                switch(State)
                {
                    case BuscarServicioState.Start :
                        this.State = BuscarServicioState.KeywordPrompt;
                        response = "Por favor, inserte la categoria por la que se buscara el servicio.";
                        break;
                    case BuscarServicioState.KeywordPrompt :
                        this.Data.Keyword = message.Text;
                        if(this.Data.Keyword != "")
                        {
                            this.State = BuscarServicioState.PreOrdenPrompt;
                            response = "Desea ordernar el resultado de busqueda?";
                        }
                        else
                        {
                            response = "La respuesta no puede ser vacia.";
                        }
                        break;
                    case BuscarServicioState.PreOrdenPrompt :
                        if(string.Equals(message.Text, "si", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = BuscarServicioState.OrdenPrompt;
                            response = "Como desea ordenar el resultado, por calificacion del proveedor o distancia entre usted y el proveedor?";
                        }
                        else if(string.Equals(message.Text, "no", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = BuscarServicioState.BuscarServicios;
                            response = "Buscando servicio. Digame cuando quiere continuar.";
                        }
                        else
                        {
                            response = "Por favor responda 'si' o 'no'";
                        }
                        break;
                    case BuscarServicioState.OrdenPrompt :
                        this.Data.Orden = message.Text;
                        if((this.Data.Orden.Contains("calificacion", StringComparison.InvariantCultureIgnoreCase)) || this.Data.Orden.Contains("distancia", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = BuscarServicioState.BuscarServicios;
                            response = "Buscando servicio. Digame cuando quiere continuar.";
                        }
                        else
                        {
                            response = "Opcion no valida. Elija entre si se ordena por calificacion o distancia";
                        }
                        break;
                    case BuscarServicioState.BuscarServicios :
                        if(this.Data.Orden.Contains("calificacion", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.Data.ResultadoServicio = buscadorServicios.BuscarOrdenCalificacion(this.Data.Keyword, buscadorUsuarios.MostrarUsuario(message.From.Id.ToString()));
                        }
                        else if(this.Data.Orden.Contains("distancia", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.Data.ResultadoServicio = buscadorServicios.BuscarOrdenDistancia(this.Data.Keyword, buscadorUsuarios.MostrarUsuario(message.From.Id.ToString()));
                        }
                        else
                        {
                            this.Data.ResultadoServicio = buscadorServicios.BuscarServiciosPorCategoria(this.Data.Keyword, buscadorUsuarios.MostrarUsuario(message.From.Id.ToString()));
                        }
                        foreach(Servicio resultado in this.Data.ResultadoServicio)
                        {
                            response = response + "\r\n" + resultado.ToString();
                        }
                        InternalCancel();
                        break;
                }
            }
        }
        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = BuscarServicioState.Start;
            this.Data = new BuscarServicioData();
        }
        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum BuscarServicioState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado en el que se espera la keyword por la que se buscara el servicio
            /// </summary>
            KeywordPrompt,
            /// <summary>
            /// Estado en el que se espera un si o no del usuario a la opcion de ordenar la lista de salida
            /// </summary>
            PreOrdenPrompt,
            /// <summary>
            /// Estado en el que se espera como el usuario quiere ordenar la lista
            /// </summary>
            OrdenPrompt,
            /// <summary>
            /// Estado en el que se busca la lista de servicios
            /// </summary>
            BuscarServicios
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class BuscarServicioData
        {
            /// <summary>
            /// Keyword de busqueda
            /// </summary>
            public string Keyword;
            /// <summary>
            /// Orden en el que se ordenara el resultado de la busqueda
            /// </summary>
            public string Orden = "";
            /// <summary>
            /// Lista resultado de la busqueda en forma de servicios
            /// </summary>
            public List<Servicio> ResultadoServicio;
            /// <summary>
            /// Lista resultado de la busqueda en forma de strings
            /// </summary>
            public List<string> ResultadoString;
        }
    }
}