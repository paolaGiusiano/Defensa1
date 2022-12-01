using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "crear categoria"
    /// </summary>
    public class CrearCategoriaHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public CrearCategoriaState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public CrearCategoriaData Data {get; set;} = new CrearCategoriaData();
        /// <summary>
        /// Gestor de categorias singleton que posee la lista de categorias disponibles
        /// </summary>
        public GestorCategorias gestorCategorias = GestorCategorias.getInstance();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuario = BuscadorUsuarios.getInstance();

        
        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public CrearCategoriaHandler(BaseHandler next) 
            : base(new string[] {"crear categoria", "crear una categoria", "agregar una categoria"}, next)
        {
            this.State = CrearCategoriaState.Start;
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar o </returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == CrearCategoriaState.Start)
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
                    case CrearCategoriaState.Start :
                        this.State = CrearCategoriaState.CategoriaNombrePrompt;
                        response = "Por favor, elija el nombre de la categoria.";
                        break;
                    case CrearCategoriaState.CategoriaNombrePrompt :
                        this.Data.CategoriaNombre = message.Text;
                        if(this.Data.CategoriaNombre != "" && !gestorCategorias.AllNames().Contains(this.Data.CategoriaNombre))
                        {
                            Categoria nuevaCategoria = gestorCategorias.CrearCategoria(this.Data.CategoriaNombre, buscadorUsuario.GetPersona(message.From.Id.ToString()));
                            this.State = CrearCategoriaState.Start;
                            response = $"Su categoria: {this.Data.CategoriaNombre} ha sido creada con éxito.";
                            InternalCancel();
                        }
                        else if(gestorCategorias.AllNames().Contains(this.Data.CategoriaNombre))
                        {
                            response = "La categoria ya existe.";
                        }
                        else
                        {
                            response = "La categoria no puede estar vacia. Por favor intente nuevamente.";
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
            this.State = CrearCategoriaState.Start;
            this.Data = new CrearCategoriaData();
        }

        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum CrearCategoriaState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte la categoria de un servicio
            /// </summary>
            CategoriaNombrePrompt,
            
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class CrearCategoriaData
        {
            /// <summary>
            /// Nombre de la categoria del servicio
            /// </summary>
            /// <value>Una string no vacia</value>
            public string CategoriaNombre {get; set;}
            
        }
    }

}