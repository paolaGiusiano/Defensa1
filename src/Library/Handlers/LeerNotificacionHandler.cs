using Telegram.Bot.Types;
using System.Collections.Generic;
using System;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "leer notificaciones"
    /// </summary>
    public class LeerNotificacionHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public LeerNotificacionesState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public LeerNotificacionesData Data {get; set;} = new LeerNotificacionesData();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuario = BuscadorUsuarios.getInstance();

        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public LeerNotificacionHandler(BaseHandler next) 
            : base(new string[] {"ver notificaciones", "leer notificaciones", "tengo notificaciones"}, next)
        {
            this.State = LeerNotificacionesState.Start;
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar o </returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == LeerNotificacionesState.Start)
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
            BotException.Precondicion(buscadorUsuario.GetPersona(message.From.Id.ToString()) is Persona, "Usuario no tiene acceso a esta funcionalidad");
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
                    case LeerNotificacionesState.Start :
                        this.State = LeerNotificacionesState.ReadPrompt;
                        response = "Cuales notificaciones quiere ver? Leidas, No leidas o Todas";
                        break;
                    case LeerNotificacionesState.ReadPrompt :
                        this.Data.Choice = message.Text;
                        this.Data.LoggedUser = buscadorUsuario.GetPersona(message.From.Id.ToString());
                        if(string.Equals(this.Data.Choice, "Leidas", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = LeerNotificacionesState.ReadDisplay;
                            response = "Buscando sus notificaciones leidas. Digame cuando quiera seguir.";
                        }
                        else if(string.Equals(this.Data.Choice, "No leidas", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = LeerNotificacionesState.UnreadDisplay;
                            response = "Buscando sus notificaciones sin leer. Digame cuando quiera seguir.";
                        }
                        else if(string.Equals(this.Data.Choice, "Todas", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = LeerNotificacionesState.AllDisplay;
                            response = "Buscando todas sus notificaciones. Digame cuando quiera seguir.";
                        }
                        else
                        {
                            response = "Mensaje invalido. Responda Leidas, No leidas o Todas";
                        }
                        break;
                    case LeerNotificacionesState.ReadDisplay :
                        foreach(Notificacion notif in this.Data.LoggedUser.Notificaciones)
                        {
                            if(notif.Leido == true)
                            {
                                this.Data.Results.Add(notif.ToString());
                            }
                        }
                        if(this.Data.Results == null)
                        {
                            response = "No tiene notificaciones leidas.";
                        }
                        else
                        {
                            response = string.Join("\r\n", this.Data.Results.ToArray());
                        }
                        InternalCancel();
                        break;
                    case LeerNotificacionesState.UnreadDisplay :
                        foreach(Notificacion notif in this.Data.LoggedUser.Notificaciones)
                        {
                            if(notif.Leido == false)
                            {
                                this.Data.Results.Add(notif.ToString());
                                notif.MarcarLeido();
                            }
                        }
                        if(this.Data.Results == null)
                        {
                            response = "No tiene notificaciones sin leer.";
                        }
                        else
                        {
                            response = string.Join("\r\n", this.Data.Results.ToArray());
                        }
                        InternalCancel();
                        break;
                    case LeerNotificacionesState.AllDisplay :
                        foreach(Notificacion notif in this.Data.LoggedUser.Notificaciones)
                        {
                            this.Data.Results.Add(notif.ToString());
                            notif.MarcarLeido();
                        }
                        if(this.Data.Results == null)
                        {
                            response = "No tiene notificaciones.";
                        }
                        else
                        {
                            response = string.Join("\r\n", this.Data.Results.ToArray());
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
            this.State = LeerNotificacionesState.Start;
            this.Data = new LeerNotificacionesData();
        }

        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum LeerNotificacionesState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario decida cuales notificaciones leer
            /// </summary>
            ReadPrompt,
            /// <summary>
            /// Estado del comando en el que se muestran las notificaciones sin leer
            /// </summary>
            UnreadDisplay,
            /// <summary>
            /// Estado del comando en el que se muestran las notificaciones leidas
            /// </summary>
            ReadDisplay,
            /// <summary>
            /// Estado del comando en el que se muestran todas las notificaciones
            /// </summary>
            AllDisplay
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class LeerNotificacionesData
        {
            /// <summary>
            /// Eleccion del usuario si quiere buscar por leidas, no leidas o todas
            /// </summary>
            /// <value>Una string no vacia</value>
            public string Choice {get; set;}
            /// <summary>
            /// Usuario leyendo las notificaciones
            /// </summary>
            /// <value>Una persona</value>
            public Persona LoggedUser {get; set;}
            /// <summary>
            /// Lista de notificaciones convertidas a string
            /// </summary>
            /// <value>Strings que cada uno es el ToString de una notificacion</value>
            public List<string> Results{get; set;} = new List<string>();
        }
    }
}