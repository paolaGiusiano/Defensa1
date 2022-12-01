using Telegram.Bot.Types;
using System;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "crear servicio"
    /// </summary>
    public class CrearServicioHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public CrearServicioState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public CrearServicioData Data {get; set;} = new CrearServicioData();
        /// <summary>
        /// Gestor de servicios singleton que posee la lista de servicios y se encarga de crear nuevos
        /// </summary>
        public GestorServicios gestorServicios = GestorServicios.getInstance();
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
        public CrearServicioHandler(BaseHandler next) 
            : base(new string[] {"crear servicio", "crear un servicio", "agregar un servicio"}, next)
        {
            this.State = CrearServicioState.Start;
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar o </returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == CrearServicioState.Start)
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
            BotException.Precondicion(buscadorUsuario.GetPersona(message.From.Id.ToString()) is Trabajador, "Usuario no tiene acceso a esta funcionalidad");
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
                    case CrearServicioState.Start :
                        this.State = CrearServicioState.DescripcionPrompt;
                        response = "Por favor, inserte la descripcion del servicio.";
                        break;
                    case CrearServicioState.DescripcionPrompt :
                        this.Data.Descripcion = message.Text;
                        if(this.Data.Descripcion != "")
                        {
                            this.State = CrearServicioState.CategoriaPrompt;
                            response = $"Ahora, por favor, elija una categoria para el servicio de la siguiente lista: {this.gestorCategorias.AllNames()}.";
                        }
                        else
                        {
                            response = "La descripcion no puede estar vacia. Por favor inserte una descripcion del servicio.";
                        }
                        break;
                    case CrearServicioState.CategoriaPrompt :
                        this.Data.Categoria = message.Text;
                        if(this.gestorCategorias.AllNames().Contains(this.Data.Categoria))
                        {
                            this.State = CrearServicioState.PagoPrompt;
                            response = $"Para continuar, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia";
                        }
                        else
                        {
                            response = $"La categoria entrada no fue valida. Por favor elija una categoria de la siguiente lista: {this.gestorCategorias.AllNames()}.";
                        }
                        break;
                    case CrearServicioState.PagoPrompt :
                        if(string.Equals(message.Text, "Debito", StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(message.Text, "Debito", StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(message.Text, "Credito", StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(message.Text, "Transferencia", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if(string.Equals(message.Text, "Debito", StringComparison.InvariantCultureIgnoreCase))
                            {
                                this.Data.Pago = Payment.Debito;
                            }
                            else if(string.Equals(message.Text, "Efectivo", StringComparison.InvariantCultureIgnoreCase))
                            {
                                this.Data.Pago = Payment.Efectivo;
                            }
                            else if(string.Equals(message.Text, "Credito", StringComparison.InvariantCultureIgnoreCase))
                            {
                                this.Data.Pago = Payment.Credito;
                            }
                            else if(string.Equals(message.Text, "Transferencia", StringComparison.InvariantCultureIgnoreCase))
                            {
                                this.Data.Pago = Payment.Transferencia;
                            }
                            this.State = CrearServicioState.CostPrompt;
                            response = "Siguiente, agregue como el costo del servicio debe ser calculado: Por hora, A termino.";
                        }
                        else
                        {
                            response = "El medio de pago no fue valido, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia";
                        }
                        break;
                    case CrearServicioState.CostPrompt :
                        response = "Para terminar, agregue el costo del servicio total en pesos.";
                        this.State = CrearServicioState.CostoPrompt;
                        if(string.Equals(message.Text, "Por hora", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.Data.Cost = Costo.Costo_por_hora;
                        }
                        else if(string.Equals(message.Text, "A termino", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.Data.Cost = Costo.Costo_total_servicio;
                        }
                        else
                        {
                            response = "Por favor elija una de las dos opciones: Por hora, A termino";
                        }
                        break;
                    case CrearServicioState.CostoPrompt :
                        this.Data.Costo = decimal.Parse(message.Text);
                        if(this.Data.Costo > 0)
                        {
                            this.State = CrearServicioState.CrearServicio;
                            response = "El servicio esta siendo creado. Digame cuando quiere continuar.";
                        }
                        else
                        {
                            response = "El valor del servicio debe ser mayor que cero.";
                        }
                        break;
                    case CrearServicioState.CrearServicio : 
                        Servicio nuevoServicio = gestorServicios.CrearServicio(this.Data.Descripcion, this.Data.Pago, this.Data.Cost, (Trabajador)buscadorUsuario.MostrarUsuario(message.From.Id.ToString()),gestorCategorias.TodaCategoria.Find(Categoria=>Categoria.Nombre == this.Data.Categoria), this.Data.Costo);
                        response = $"El servicio fue creado exitosamente con los siguientes parametros. Descripcion: {nuevoServicio.DescripcionServicio}, Categoria: {nuevoServicio.Categoria.Nombre}, Tipo de pago: {nuevoServicio.Pago}, Costo: {nuevoServicio.Costo}";
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
            this.State = CrearServicioState.Start;
            this.Data = new CrearServicioData();
        }

        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum CrearServicioState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte la descripcion de un servicio
            /// </summary>
            DescripcionPrompt,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte la categoria de un servicio
            /// </summary>
            CategoriaPrompt,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte el tipo de pago de un servicio
            /// </summary>
            PagoPrompt,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte la forma en la que se calcula el costo de un servicio
            /// </summary>
            CostPrompt,
            /// <summary>
            /// Estado del comando en el que se espera que el usuario inserte el costo del servicio
            /// </summary>
            CostoPrompt,
            /// <summary>
            /// Estado del comando en el que se crea el servicio
            /// </summary>
            CrearServicio
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class CrearServicioData
        {
            /// <summary>
            /// Descripcion del servicio
            /// </summary>
            /// <value>Una string no vacia</value>
            public string Descripcion {get; set;}
            /// <summary>
            /// Nombre de la categoria del servicio
            /// </summary>
            /// <value>Una string no vacia</value>
            public string Categoria {get; set;}
            /// <summary>
            /// Tipo de pago del servicio
            /// </summary>
            /// <value>Valor del enum Payment</value>
            public Payment Pago {get; set;}
            /// <summary>
            /// Forma de calcular el costo de un servicio (por hora o a termino)
            /// </summary>
            /// <value>Valor del enum Costo</value>
            public Costo Cost {get; set;}
            /// <summary>
            /// Costo del servicio
            /// </summary>
            /// <value>Un decimal distinto de cero</value>
            public decimal Costo {get; set;}
        }
    }
}