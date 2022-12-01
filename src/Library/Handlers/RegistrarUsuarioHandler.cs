using Telegram.Bot.Types;
using System;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "registrar usuario"
    /// </summary>
    public class RegistrarUsuarioHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public RegistrarUsuarioState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public RegistrarUsuarioData Data {get; set;}
        /// <summary>
        /// Gestor de usuarios singleton que contiene la lista de todos los usuarios del sistema y agrega nuevos
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Calculadora de distancia que corroborara si la direccion insertada existe
        /// </summary>
        public DistanceCalculator calculator;
        /// <summary>
        /// Cliente para el funcionamiento de la calculadora de distancia
        /// </summary>
        public LocationApiClient client = new LocationApiClient();
        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public RegistrarUsuarioHandler(BaseHandler next)
            : base(new string[] {"registrarme", "registrar", "agregar usuario"}, next)
        {
            this.State = RegistrarUsuarioState.Start;
            this.Data = new RegistrarUsuarioData();
            calculator = new DistanceCalculator(client);
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar</returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == RegistrarUsuarioState.Start)
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
                    case RegistrarUsuarioState.Start :
                        this.State = RegistrarUsuarioState.ProfilePrompt;
                        response = "Por favor defina que tipo de usuario quiere registrar: empleador o trabajador";
                        break;
                    case RegistrarUsuarioState.ProfilePrompt :
                        this.Data.Profile = message.Text;
                        if(string.Equals(this.Data.Profile, "empleador", StringComparison.InvariantCultureIgnoreCase) || string.Equals(this.Data.Profile, "trabajador", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = RegistrarUsuarioState.NombrePrompt;
                            response = "Siguiente, por favor inserte el nombre del usuario a registrar.";
                        }
                        else
                        {
                            response = "Perfil seleccionado invalido, por favor elija entre empleador o trabajador.";
                        }
                        break;
                    case RegistrarUsuarioState.NombrePrompt :
                        this.Data.Nombre = message.Text;
                        if(this.Data.Nombre != "")
                        {
                            this.State = RegistrarUsuarioState.ApellidoPrompt;
                            response = "Siguiente, por favor inserte el apellido del usuario a registrar.";
                        }
                        else
                        {
                            response = "El nombre no puede ser vacio.";
                        }
                        break;
                    case RegistrarUsuarioState.ApellidoPrompt :
                        this.Data.Apellido = message.Text;
                        if(this.Data.Apellido != "")
                        {
                            this.State = RegistrarUsuarioState.EmailPrompt;
                            response = "Siguiente, por favor inserte el email del usuario a registrar.";
                        }
                        else
                        {
                            response = "El apellido no puede ser vacio.";
                        }
                        break;
                    case RegistrarUsuarioState.EmailPrompt :
                        this.Data.Email = message.Text;
                        if(this.Data.Apellido != "")
                        {
                            this.State = RegistrarUsuarioState.PasswordPrompt;
                            response = "Siguiente, por favor inserte la contraseña del usuario a registrar. Debe ser al menos 8 caracteres.";
                        }
                        else
                        {
                            response = "El apellido no puede ser vacio.";
                        }
                        break;
                    case RegistrarUsuarioState.PasswordPrompt :
                        this.Data.Password = message.Text;
                        if(this.Data.Password.Length >= 8)
                        {
                            this.State = RegistrarUsuarioState.DireccionPrompt;
                            response = "Siguiente, por favor inserte la direccion del usuario a registrar en el siguiente formato: Departamento, Calle numero de edificio";
                        }
                        else
                        {
                            response = "La contraseña debe tener 8 o mas caracteres.";
                        }
                        break;
                    case RegistrarUsuarioState.DireccionPrompt :
                        this.Data.Direccion = message.Text;
                        if(this.calculator.CalculateDistance(this.Data.Direccion, this.Data.Direccion).FromExists)
                        {
                            this.State = RegistrarUsuarioState.TelefonoPrompt;
                            response = "Siguiente, por favor inserte el telefono de contacto del usuario a registrar.";
                            break;
                        }
                        else
                        {
                            response = "La direccion no existe. Inserte una direccion valida.";
                        }
                        break;
                    case RegistrarUsuarioState.TelefonoPrompt :
                        this.Data.Telefono = message.Text;
                        if(this.Data.Telefono != "")
                        {
                            this.State = RegistrarUsuarioState.RegistrarUsuario;
                            response = "Creando usuario. Cuando quiera continuar, digamelo.";
                        }
                        else
                        {
                            response = "El telefono no puede ser vacio.";
                        }
                        break;
                    case RegistrarUsuarioState.RegistrarUsuario :
                        if(string.Equals(this.Data.Profile, "empleador", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Empleador nuevoEmpleador = gestorUsuario.AgregarEmpleador(this.Data.Nombre, this.Data.Apellido, this.Data.Email, this.Data.Telefono, this.Data.Direccion, this.Data.Password, message.From.Id.ToString());
                            response = $"Se ha creado el empleador {nuevoEmpleador.Nombre} {nuevoEmpleador.Apellido} con el ID {nuevoEmpleador.ID}.";
                            InternalCancel();
                        }
                        else if(string.Equals(this.Data.Profile, "trabajador", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Trabajador nuevoTrabajador = gestorUsuario.AgregarTrabajador(this.Data.Nombre, this.Data.Apellido, this.Data.Email, this.Data.Telefono, this.Data.Direccion, this.Data.Password, message.From.Id.ToString());
                            response = $"Se ha creado el trabajador {nuevoTrabajador.Nombre} {nuevoTrabajador.Apellido} con el ID {nuevoTrabajador.ID}.";
                            InternalCancel();
                        }
                        else
                        {
                            response = "Error desconocido. Algo salio mal.";
                        }
                        break;
                        
                }
            }
        }
        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = RegistrarUsuarioState.Start;
            this.Data = new RegistrarUsuarioData();
        }
        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum RegistrarUsuarioState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado en el que se espera el perfil del usuario, empleador o trabajador
            /// </summary>
            ProfilePrompt,
            /// <summary>
            /// Estado en el que se espera el nombre del usuario
            /// </summary>
            NombrePrompt,
            /// <summary>
            /// Estado en el que se espera el apellido del usuario 
            /// </summary>
            ApellidoPrompt,
            /// <summary>
            /// Estado en el que se espera el email del usuario 
            /// </summary>
            EmailPrompt,
            /// <summary>
            /// Estado en el que se espera el password del usuario 
            /// </summary>
            PasswordPrompt,
            /// <summary>
            /// Estado en el que se espera el telefono del usuario 
            /// </summary>
            TelefonoPrompt,
            /// <summary>
            /// Estado en el que se espera la direccion del usuario 
            /// </summary>
            DireccionPrompt,
            /// <summary>
            /// Estado en el que se crea al usuario 
            /// </summary>
            RegistrarUsuario
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class RegistrarUsuarioData
        {
            /// <summary>
            /// Perfil del usuario, sea empleador o trabajador
            /// </summary>
            public string Profile;
            /// <summary>
            /// Nombre del usuario
            /// </summary>
            public string Nombre;
            /// <summary>
            /// Apellido del usuario
            /// </summary>
            public string Apellido;
            /// <summary>
            /// Email del usuario
            /// </summary>
            public string Email;
            /// <summary>
            /// Password del usuario
            /// </summary>
            public string Password;
            /// <summary>
            /// Telefono del usuario
            /// </summary>
            public string Telefono;
            /// <summary>
            /// Direccion del usuario
            /// </summary>
            public string Direccion;
        }
    }   
}