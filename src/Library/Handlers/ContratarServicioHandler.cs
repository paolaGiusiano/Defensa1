using Telegram.Bot.Types;
using System;

namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "contratar servicio"
    /// </summary>
    public class ContratarServicioHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando
        /// </summary>
        public ContratarServicioState State {get; set;}
        /// <summary>
        /// Datos que se van obteniendo del comando
        /// </summary>
        public ContratarServicioData Data {get; set;}
        /// <summary>
        /// Gestor de contratos singleton que posee la lista de contratos y se encarga de crear nuevos
        /// </summary>
        public GestorContratos gestorContratos = GestorContratos.getInstance();
        /// <summary>
        /// Buscador de servicios singleton que busca un servicios en la lista de todos los servicios
        /// </summary>
        public BuscadorServicios buscadorServicios = BuscadorServicios.getInstance();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Gestor de categorias singleton que posee la lista de categorias y se encarga de crear nuevas
        /// </summary>
        /// <returns></returns>
        public GestorCategorias gestorCategorias = GestorCategorias.getInstance();

        /// <summary>
        /// Constructor que implementa el base de los handlers
        /// </summary>
        /// <param name="next">Siguiente handler a pasar el comando de no procesarlo</param>
        public ContratarServicioHandler(BaseHandler next)
            : base(new string[] {"contratar servicio", "contratar un servicio"}, next)
        {
            this.State = ContratarServicioState.Start;
            this.Data = new ContratarServicioData();
        }
        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar</returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == ContratarServicioState.Start)
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
            BotException.Precondicion(buscadorUsuarios.GetPersona(message.From.Id.ToString()) is Empleador, "Usuario no tiene acceso a esta funcionalidad");
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
                    case ContratarServicioState.Start :
                        this.State = ContratarServicioState.BusquedaServicioPrompt;
                        response = "Conoce el ID del servicio que desea contratar?";
                        break;
                    case ContratarServicioState.BusquedaServicioPrompt :
                        this.Data.FormaBusqueda = message.Text;
                        if(string.Equals(this.Data.FormaBusqueda, "si", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = ContratarServicioState.IdPrompt;
                            response = "Por favor, inserte el ID del servicio que quiere buscar.";
                        }
                        else if(string.Equals(this.Data.FormaBusqueda, "no", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.State = ContratarServicioState.DescripcionPrompt;
                            response = "Okay, no hay problema, busquemos el servicio por sus parametros entonces. Por favor inserte la descripcion del servicio.";
                        }
                        else
                        {
                            response = "Comando no valido, por favor responda 'si' o 'no'";
                        }
                        break;
                    case ContratarServicioState.IdPrompt :
                        this.Data.ID = int.Parse(message.Text);
                        this.Data.ServicioBuscado = buscadorServicios.BuscarServicioUnico(this.Data.ID);
                        if(this.Data.ServicioBuscado != null)
                        {
                            this.Data.Cost = this.Data.ServicioBuscado.Cost;
                            if(this.Data.Cost == Costo.Costo_por_hora)
                            {
                                this.State = ContratarServicioState.HoraPrompt;
                                response = "Como el servicio que esta contratando cobra por hora, por favor inserte la cantidad de horas por las que esta contratando.";
                            }
                            else if(this.Data.Cost == Costo.Costo_total_servicio)
                            {
                                this.State = ContratarServicioState.ContratarServicio;
                                response = $"Se esta creando su contrato. Digame cuando quiera continuar.";
                            }
                            else
                            {
                                response = "Algo salio mal, el servicio no posee un medio de pago valido.";
                            }
                        }
                        else
                        {
                            response = "El id buscado no existe.";
                        }
                        break;
                    case ContratarServicioState.DescripcionPrompt :
                        this.Data.Descripcion = message.Text;
                        if(this.Data.Descripcion != "")
                        {
                            this.State = ContratarServicioState.CategoriaPrompt;
                            response = $"Ahora, por favor, elija una categoria para el servicio de la siguiente lista: {this.gestorCategorias.AllNames()}.";
                        }
                        else
                        {
                            response = "La descripcion no puede estar vacia. Por favor inserte una descripcion del servicio.";
                        }
                        break;
                    case ContratarServicioState.CategoriaPrompt :
                        this.Data.Categoria = message.Text;
                        if(this.gestorCategorias.AllNames().Contains(this.Data.Categoria))
                        {
                            this.State = ContratarServicioState.PagoPrompt;
                            response = $"Para continuar, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia";
                        }
                        else
                        {
                            response = $"La categoria entrada no fue valida. Por favor elija una categoria de la siguiente lista: {this.gestorCategorias.AllNames()}.";
                        }
                        break;
                    case ContratarServicioState.PagoPrompt :
                        if(string.Equals(message.Text, "Debito", StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(message.Text, "Efectivo", StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(message.Text, "Credito", StringComparison.InvariantCultureIgnoreCase) || 
                        string.Equals(message.Text, "Transeferencia", StringComparison.InvariantCultureIgnoreCase))
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
                            this.State = ContratarServicioState.CostoPrompt;
                            response = "Siguiente, agregue como el costo del servicio debe ser calculado: Por hora, A termino.";
                        }
                        else
                        {
                            response = "El medio de pago no fue valido, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia";
                        }
                        break;
                    case ContratarServicioState.CostoPrompt :
                        if(string.Equals(message.Text, "Por hora", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.Data.Cost = Costo.Costo_por_hora;
                            this.State = ContratarServicioState.EmailPrompt;
                            response = "Inserte el email del proveedor del servicio.";
                        }
                        else if(message.Text == "A termino")
                        {
                            this.Data.Cost = Costo.Costo_total_servicio;
                            this.State = ContratarServicioState.EmailPrompt;
                            response = "Inserte el email del proveedor del servicio.";
                        }
                        else
                        {
                            response = "Por favor elija una de las dos opciones: Por hora, A termino";
                        }
                        break;
                    case ContratarServicioState.EmailPrompt :
                        Persona aux = buscadorUsuarios.MostrarPersonaEmail(message.Text);
                        if(aux.Email != "" && aux is Trabajador)
                        {
                            this.Data.Trabajador = (Trabajador)aux;
                            if(this.Data.Cost == Costo.Costo_total_servicio)
                            {
                                this.State = ContratarServicioState.ContratarServicio;
                                response = $"Se esta creando su contrato. Digame cuando quiera continuar.";
                            }
                            else if(this.Data.Cost == Costo.Costo_por_hora)
                            {
                                this.State = ContratarServicioState.HoraPrompt;
                                response = "Como el servicio que esta contratando cobra por hora, por favor inserte la cantidad de horas por las que esta contratando.";
                            }
                        }
                        else
                        {
                            response = "El usuario que usted busco no es valido. Por favor, inserte el email del proveedor del servicio";
                        }
                        break;
                    case ContratarServicioState.HoraPrompt :
                        this.Data.Hora = decimal.Parse(message.Text);
                        if(this.Data.Hora > 0)
                        {
                            this.State = ContratarServicioState.ContratarServicio;
                            response = $"Se esta creando su contrato. Digame cuando quiera continuar.";
                        }
                        else
                        {
                            response = "No puede contratar un servicio por cero horas o menos.";
                        }
                        break;
                    case ContratarServicioState.ContratarServicio :
                        if(this.Data.ServicioBuscado == null)
                        {
                            this.Data.ServicioBuscado = buscadorServicios.MostrarServicio(this.Data.Descripcion, gestorCategorias.TodaCategoria.Find(c => c.Nombre == this.Data.Categoria), this.Data.Pago, this.Data.Cost, this.Data.Trabajador.Email);
                        }
                        if(this.Data.Hora == 0)
                        {
                            this.Data.NuevoContrato = gestorContratos.CrearContrato((Empleador)buscadorUsuarios.MostrarUsuario(message.From.Id.ToString()), this.Data.ServicioBuscado);
                            response = $"Se ha creado el contrato {this.Data.NuevoContrato.ID} con pago a termino a su nombre con {this.Data.ServicioBuscado.Trabajador.Nombre} {this.Data.ServicioBuscado.Trabajador.Apellido} por {this.Data.NuevoContrato.CostoTotal}.";
                        }
                        else
                        {
                            this.Data.NuevoContrato = gestorContratos.CrearContratoHora((Empleador)buscadorUsuarios.MostrarUsuario(message.From.Id.ToString()), this.Data.ServicioBuscado, this.Data.Hora);
                            response = $"Se ha creado el contrato {this.Data.NuevoContrato.ID} por {this.Data.Hora} horas a su nombre con {this.Data.ServicioBuscado.Trabajador.Nombre} {this.Data.ServicioBuscado.Trabajador.Apellido} por {this.Data.NuevoContrato.CostoTotal}.";
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
            this.State = ContratarServicioState.Start;
            this.Data = new ContratarServicioData();
        }
        /// <summary>
        /// Lista de enum con cada estado del comando
        /// </summary>
        public enum ContratarServicioState
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            /// Estado del comando en el que se espera un si o no a la pregunta si el usuario sabe el ID del servicio
            /// </summary>
            BusquedaServicioPrompt,
            /// <summary>
            /// Estado del comando en el que se espera un ID del servicio
            /// </summary>
            IdPrompt,
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
            CostoPrompt,
            /// <summary>
            /// Estado del comando en el que se espera el email del trabajador duenho del servicio
            /// </summary>
            EmailPrompt,
            /// <summary>
            /// Estado del comando en el que se espera la cantidad de horas por las cuales se facturara el servicio.
            /// Solo seria distinto de cero si el Cost del servicio es Costo_por_hora
            /// </summary>
            HoraPrompt,
            /// <summary>
            /// Estado del comando en el que se contrata un servicio
            /// </summary>
            ContratarServicio,
        }
        /// <summary>
        /// Clase que almacena la informacion a ser usada por el comando
        /// </summary>
        public class ContratarServicioData
        {
            /// <summary>
            /// Flag que determina si el usuario quiere buscar por ID o por parametros
            /// </summary>
            /// <value>Un string "si" o "no"</value>
            public string FormaBusqueda {get; set;}
            /// <summary>
            /// ID unico del servicio
            /// </summary>
            /// <value>Un string no vacio</value>
            public int ID {get; set;}
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
            /// Trabajador duenho del servicio
            /// </summary>
            /// <value>Un trabajador no vacio</value>
            public Trabajador Trabajador {get; set;}
            /// <summary>
            /// Horas que un servicio por hora va a ser facturado por
            /// </summary>
            /// <value>Un decimal igual o mayor a cero</value>
            public decimal Hora {get; set;}
            /// <summary>
            /// Servicio que sera contratado
            /// </summary>
            /// <value>Un servicio no vacio</value>
            public Servicio ServicioBuscado {get; set;}
            /// <summary>
            /// Contrato creado
            /// </summary>
            /// <value></value>
            public Contrato NuevoContrato {get; set;}
        }
    }
}