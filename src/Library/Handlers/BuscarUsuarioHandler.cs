using Telegram.Bot.Types;
using System;

namespace CoreBot
{
    /// <summary>
    /// Handler que maneja el comando "buscar usuario" para implementar Chain of Responsability
    /// </summary>
    public class BuscarUsuarioHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public BuscarUsuarioState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public BuscarUsuarioData Data {get; set;}
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public BuscarUsuarioHandler(BaseHandler next)
            : base(new string[] {"buscar usuario", "buscar un usuario"}, next)
        {
            this.State = BuscarUsuarioState.Start;
            this.Data = new BuscarUsuarioData();
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar</returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == BuscarUsuarioState.Start)
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
                    case BuscarUsuarioState.Start :
                        State = BuscarUsuarioState.BusquedaPrompt;
                        response = "Quiere buscar por ID del usuario o por su email?";
                        break;
                    case BuscarUsuarioState.BusquedaPrompt :
                        this.Data.FormaBusqueda = message.Text;
                        if(string.Equals(this.Data.FormaBusqueda, "id", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            State = BuscarUsuarioState.IDPrompt;
                            response = "Inserte ID del usuario que quiere buscar.";
                        }
                        else if(string.Equals(this.Data.FormaBusqueda, "email", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            State = BuscarUsuarioState.EmailPrompt;
                            response = "Inserte email del usuario que quiere buscar.";
                        }
                        else
                        {
                            response = "Comando invalido. Quiere buscar por ID o email?";
                        }
                        break;
                    case BuscarUsuarioState.IDPrompt :
                        this.Data.ID = message.Text;
                        if(ulong.Parse(this.Data.ID) > 0)
                        {
                            State = BuscarUsuarioState.BuscarUsuarioID;
                            response = "Buscando usuario. Digame cuando quiere continuar.";
                        }
                        else
                        {
                            response = "El ID no es un numero valido.";
                        }
                        break;
                    case BuscarUsuarioState.EmailPrompt :
                        this.Data.Email = message.Text;
                        if(this.Data.Email.Contains('@', StringComparison.InvariantCultureIgnoreCase))
                        {
                            State = BuscarUsuarioState.BuscarUsuarioEmail;
                            response = "Buscando usuario. Digame cuando quiere continuar.";
                        }
                        else
                        {
                            response = "El email no es valido.";
                        }
                        break;
                    case BuscarUsuarioState.BuscarUsuarioEmail :
                        Persona persona1 = buscadorUsuarios.MostrarPersonaEmail(this.Data.Email);
                        if(persona1 is Usuario)
                        {
                            response = ((Usuario)persona1).ToString();
                        }
                        else
                        {
                            response = "El usuario no se encontro.";
                        }
                        InternalCancel();
                        break;
                    case BuscarUsuarioState.BuscarUsuarioID :
                        Persona persona2 = buscadorUsuarios.GetPersona(this.Data.ID);
                        if(persona2 is Usuario)
                        {
                            response = ((Usuario)persona2).ToString();
                        }
                        else
                        {
                            response = "El usuario no se encontro.";
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
            this.State = BuscarUsuarioState.Start;
            this.Data = new BuscarUsuarioData();
        }
        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum BuscarUsuarioState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado en el que se espera la forma por la que se quiere buscar el usuario (ID o Email)
            /// </summary>
            BusquedaPrompt,
            /// <summary>
            /// Estado en el que se espera el email del usuario a buscar
            /// </summary>
            EmailPrompt,
            /// <summary>
            /// Estado en el que se espera el ID del usuario a buscar
            /// </summary>
            IDPrompt,
            /// <summary>
            /// Estado en el que se busca al usuario por ID
            /// </summary>
            BuscarUsuarioID,
            /// <summary>
            /// Estado en el que se busca al usuario por email
            /// </summary>
            BuscarUsuarioEmail
        }
        /// <summary>
        /// Clase interna que almacena la data proveida por el usuario
        /// </summary>
        public class BuscarUsuarioData
        {
            /// <summary>
            /// Forma en la que se quiere buscar, email o ID
            /// </summary>
            public string FormaBusqueda;
            /// <summary>
            /// Email del usuario a buscar
            /// </summary>
            public string Email;
            /// <summary>
            /// ID del usuario a buscar
            /// </summary>
            public string ID;
        }
    }
}