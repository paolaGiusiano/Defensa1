using Telegram.Bot.Types;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "buscar servicio"
    /// </summary>
    public class AgregarAdminHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public AgregarAdminState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public AgregarAdminData Data {get; set;}
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Gestor de usuarios singleton
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public AgregarAdminHandler(BaseHandler next)
            : base(new string[] {"agregar administrador"}, next)
        {
            this.State = AgregarAdminState.Start;
            this.Data = new AgregarAdminData();
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar</returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == AgregarAdminState.Start)
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
                    case AgregarAdminState.Start :
                        this.State = AgregarAdminState.SecretPrompt;
                        response = "La creacion de administradores esta restringida a otros admins y miembros del equipo de desarrollo. Inserte master password o su password personal.";
                        break;
                    case AgregarAdminState.SecretPrompt :
                        this.Data.Key = message.Text;
                        if(this.Data.Key == "P2Bot" || (this.Data.Key == buscadorUsuarios.GetPersona(message.From.Id.ToString()).Contraseña && buscadorUsuarios.GetPersona(message.From.Id.ToString()) is Administrador))
                        {
                            this.State = AgregarAdminState.NombrePrompt;
                            response = "Por favor introduzca el Nombre del Administrador";
                        }
                        else
                        {
                            response = "Usted no tiene acceso a crear un administrador. Volviendo a menu principal.";
                            InternalCancel();
                        }
                        break;
                    case AgregarAdminState.NombrePrompt :
                        this.Data.Nombre = message.Text;
                        if(message.Text != "")
                        {
                            this.State = AgregarAdminState.ApellidoPrompt;
                            response = "Siguiente, introduzca el Apellido del Administrador";
                        }
                        else
                        {
                            response = "El nombre no puede estar vacio.";
                        }
                        break;
                    case AgregarAdminState.ApellidoPrompt :
                        this.Data.Apellido = message.Text;
                        if(message.Text != "")
                        {
                            this.State = AgregarAdminState.EmailPrompt;
                            response = "Siguiente, introduzca el Email del Administrador";
                        }
                        else
                        {
                            response = "El email no puede estar vacio.";
                        }
                        break;
                    case AgregarAdminState.EmailPrompt :
                        this.Data.Email = message.Text;
                        if(this.Data.Email != "")
                        {
                            this.State = AgregarAdminState.PasswordPrompt;
                            response = "Siguiente, introduzca la password del Administrador. Recuerde que debe tener 8 o mas caracteres.";
                        }
                        else
                        {
                            response = "El email no puede estar vacio.";
                        }
                        break;
                    case AgregarAdminState.PasswordPrompt :
                        this.Data.Password = message.Text;
                        if(this.Data.Password.Length >= 8)
                        {
                            this.State = AgregarAdminState.CrearAdmin;
                            response = "Creando Admin. Escriba Next o cualquier mensaje para continuar.";
                        }
                        else
                        {
                            response = "La password debe tener 8 o mas caracteres.";
                        }
                        break;
                    case AgregarAdminState.CrearAdmin :
                        Administrador newAdmin = gestorUsuario.AgregarAdmin(this.Data.Nombre, this.Data.Apellido, this.Data.Email, this.Data.Password, message.From.Id.ToString());
                        response = $"Admin {newAdmin.Nombre} {newAdmin.Apellido} creado.";
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
            this.State = AgregarAdminState.Start;
            this.Data = new AgregarAdminData();
        }
        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum AgregarAdminState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado en el que se espera el secret word para permitir la creacion de un admin
            /// </summary>
            SecretPrompt,
            /// <summary>
            /// Estado en el que se espera el nombre del nuevo admin
            /// </summary>
            NombrePrompt,
            /// <summary>
            /// Estado en el que se espera el apellido del nuevo admin
            /// </summary>
            ApellidoPrompt,
            /// <summary>
            /// Estado en el que se espera el email del nuevo admin
            /// </summary>
            EmailPrompt,
            /// <summary>
            /// Estado en el que se espera el password del nuevo admin
            /// </summary>
            PasswordPrompt,
            /// <summary>
            /// Estado en el que se crea el admin
            /// </summary>
            CrearAdmin
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class AgregarAdminData
        {
            /// <summary>
            /// Key que valida que el usuario puede crear un admin (es un admin o parte del equipo de desarrollo)
            /// </summary>
            public string Key;
            /// <summary>
            /// Nombre del admin
            /// </summary>
            public string Nombre;
            /// <summary>
            /// Apellido del admin
            /// </summary>
            public string Apellido;
            /// <summary>
            /// Email del admin
            /// </summary>
            public string Email;
            /// <summary>
            /// Password del admin
            /// </summary>
            public string Password;
        }
    }
}