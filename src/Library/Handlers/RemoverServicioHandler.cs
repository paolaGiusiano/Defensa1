using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "crear categoria"
    /// </summary>
    public class RemoverServicioHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public RemoverServicioState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public RemoverServicioData Data {get; set;} = new RemoverServicioData();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BuscadorServicios buscadorServicios=BuscadorServicios.getInstance();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BuscadorUsuarios buscadorUsuario=BuscadorUsuarios.getInstance();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GestorServicios gestorServicios=GestorServicios.getInstance();

        
        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public RemoverServicioHandler(BaseHandler next) 
            : base(new string[] {"remover servicio", "borrar servicio", "remover un servicio"}, next)
        {
            this.State = RemoverServicioState.Start;
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar o </returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == RemoverServicioState.Start)
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
        /// <param name="response">La respuesta al mensaje procesado indicando que el mensaje no pudo se procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            BotException.Precondicion(buscadorUsuario.GetPersona(message.From.Id.ToString()) is Administrador, "Usuario no tiene acceso a esta funcionalidad");
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
                    case RemoverServicioState.Start :
                        this.State = RemoverServicioState.IDServicioPrompt;
                        response = "Por favor, elija el ID del servicio que desea remover.";
                        break;
                    case RemoverServicioState.IDServicioPrompt:
                        
                        this.Data.IDServicio = int.Parse(message.Text);
                        this.Data.servicio = buscadorServicios.BuscarServicioUnico(this.Data.IDServicio);
                        if(gestorServicios.TodoServicios.Contains(this.Data.servicio))
                        {
                            this.State = RemoverServicioState.MotivoPrompt;
                            response = $"Por favor, explique el motivo por el que se esta removiendo el servicio.";
                        }
                        else
                        {
                            response = "El servicio no existe.";
                        }
                        break;
                    case RemoverServicioState.MotivoPrompt :
                        this.Data.Motivo = message.Text;
                        if(this.Data.Motivo != "")
                        {
                            gestorServicios.RemoverServicio(this.Data.servicio, buscadorUsuario.GetPersona(message.From.Id.ToString()), this.Data.Motivo);
                            response = "El servicio ha sido removido con exito.";
                        }
                        else
                        {
                            response = "El motivo no puede ser vacio.";
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
            this.State = RemoverServicioState.Start;
            this.Data = new RemoverServicioData();
        }

        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum RemoverServicioState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte la ID del servicio a remover
            /// </summary>
            IDServicioPrompt,
            /// <summary>
            /// Estado del comando en el que se espera el motivo por el que se borra el servicio
            /// </summary>
            MotivoPrompt
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class RemoverServicioData
        {
            /// <summary>
            /// ID del servicio
            /// </summary>
            /// <value>Una int no vacia</value>
            public int IDServicio {get; set;}
            /// <summary>
            /// Motivo por el que se borra el servicio
            /// </summary>
            /// <value>Una string no vacia</value>
            public string Motivo {get; set;}
            /// <summary>
            /// Servicio a ser removido
            /// </summary>
            /// <value>Un servicio buscado</value>
            public Servicio servicio {get; set;}
        }
    }
}