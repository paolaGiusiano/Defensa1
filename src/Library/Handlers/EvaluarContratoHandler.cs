using Telegram.Bot.Types;
using System;
namespace CoreBot
{
    /// <summary>
    /// Un handler del patron chain of responsability que implementa el comando de "Evaluar servicio"
    /// </summary>

    public class EvaluarContratoHandler : BaseHandler
    {
        /// <summary>
        /// Instancia singleton que se usa para retornar el contrato necesario.
        /// </summary>
        /// <returns></returns>
        public GestorContratos gestorContratos = GestorContratos.getInstance();
        /// <summary>
        /// Instancia Singleton para traerme el usuario
        /// </summary>
        /// <returns></returns>
        public BuscadorUsuarios buscadorUsuario = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Almacena los datos para evaluar un Contrato
        /// </summary>
        /// <returns></returns>
        public EvaluarContratoData Data {get; set;} = new EvaluarContratoData();
        /// <summary>
        /// Almacena el estado para el patron state
        /// </summary>
        /// <value></value>
        public EvaluarContratoState State {get;set;}
        /// <summary>
        /// Constructor, que llama al constructor de la clase padre
        /// </summary>
        /// <param name="next"></param>

        public EvaluarContratoHandler(BaseHandler next) 
        : base(new string []{"evaluar contrato", "evaluar un contrato", "calificar un contrato"},next)
        {
            State = EvaluarContratoState.Start;
        }

               /// <summary>
        ///  Override de la clase base de los handlers para chequear si el mensaje puede ser procesado.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool CanHandle(Message message)
        {
            if(this.State == EvaluarContratoState.Start)
            {
                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Procesa cada evaluacion, y cambia el la propieda state en cada pasada, para captar en que parte del proceso se genero una incosistencia.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="response"></param>
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
                    case EvaluarContratoState.Start:
                        this.State = EvaluarContratoState.Usuario;
                        response="Validando Usuario. Dime cuando quieras continuar.";
                        break;
                        //Agregar buscador de contratos porque esta mal sino.
                    case EvaluarContratoState.Usuario:
                        Data.Trabajador_Empleador = buscadorUsuario.MostrarUsuario(message.From.Id.ToString());
                        if(Data.Trabajador_Empleador is Trabajador)
                        {
                            this.State = EvaluarContratoState.RecuperarContrato;
                            response = "Ingrese un numero de id del contrato que desea evaluar.";
                        }else if( Data.Trabajador_Empleador is Empleador)
                        {
                            this.State = EvaluarContratoState.RecuperarContrato;
                            response = "Ingrese un numero de id del contrato que desea evaluar.";


                        }else
                        {
                            //Es por si le pasan un admin.
                            response = "Debe recibir una instancia de Empleador o Trabajador";
                        }
                        break;

                    case EvaluarContratoState.RecuperarContrato:
                        try
                        {
                            Data.ContratoaCalificar = gestorContratos.BuscarContrato(message.Text);
                            this.State = EvaluarContratoState.Calificacion;
                            response = "Ingrese un numero de 1 a 5, para calificar.";

                        }catch(ArgumentNullException)
                        {
                            response = "El ID no es correcto, o no existe, intentelo de nuevo.";

                        }catch(ParameterError error)
                        {
                            response = error.CUSTOMMESSAGE;
                        }
                    
                        break;
                    case EvaluarContratoState.Calificacion:
                        int ValorCalificacion;
                        Calificacion c = new Calificacion();

                        try{
                            ValorCalificacion = Convert.ToInt32(message.Text);
                            if(ValorCalificacion>5 || ValorCalificacion<0 ){
                                throw new InvalidInteger();
                            }else{
                                
                                if(this.Data.Trabajador_Empleador is Trabajador)
                                {
                                    gestorContratos.EvaluarContratoEmpleador(ValorCalificacion, this.Data.ContratoaCalificar, this.Data.Trabajador_Empleador);
                                    response = "Se evaluó el servicio con exito.";
                                    State = EvaluarContratoState.EvaluarServicio;
                                    Data.Trabajador_Empleador.AgregarCalificacion(c.AgregarCalificacion(ValorCalificacion));
                                    Data.Evaluacion = c.AgregarCalificacion(ValorCalificacion);

                                }
                                else if(this.Data.Trabajador_Empleador is Empleador)
                                {
                                    gestorContratos.EvaluarContratoTrabajdor(ValorCalificacion, this.Data.ContratoaCalificar, this.Data.Trabajador_Empleador);
                                    response = "Se evaluó el servicio con exito.";
                                    State = EvaluarContratoState.EvaluarServicio;
                                    Data.Trabajador_Empleador.AgregarCalificacion(c.AgregarCalificacion(ValorCalificacion));
                                    Data.Evaluacion = c.AgregarCalificacion(ValorCalificacion);

                                    
                                }
                                else
                                {
                                    response = "Error.";
                                }


                            }

                        }
                        catch (InvalidInteger inv)
                        {
                            response = inv.CUSTOMMESSAGE;
                        }
                        catch(Exception exc )
                        {
                            if(exc is FormatException || exc is OverflowException){
                                response = "El valor debe ser un número.";

                            }else{
                                response = "Error generico, intentelo de nuevo" ;
                            }
                        }
                        break;
                    case EvaluarContratoState.EvaluarServicio:
                        InternalCancel();
                        break;
                }
            }
        }



        /// <summary>
        /// Cancela el handler, y retorna al estado inicial.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = EvaluarContratoState.Start;
            this.Data = new EvaluarContratoData();

        }
    }
    /// <summary>
    /// Enum necesario para trabajar con el patron state.
    /// </summary>
    public enum EvaluarContratoState
        {
            /// <summary>
            /// Estado inicial del comando.
            /// </summary>
            Start,
            /// <summary>
            /// Evalua la instancia de Trabajador o empleador no sea null o tenga parametros faltantes
            /// </summary>
            Usuario,

            /// <summary>
            /// Evalua la instancia de recuperar el contrato a calificar.
            /// </summary>
            RecuperarContrato,
            /// <summary>
            /// Evalua que el llamado a la funcion calificar sea correcto.
            /// </summary>
            Calificacion,           
            /// <summary>
            /// Crea el servicio si todo ocurre con exito
            /// </summary>
            EvaluarServicio
        }
    /// <summary>
    /// Clase con los datos para evaluar el servicio
    /// </summary>
    public class EvaluarContratoData
    {
        /// <summary>
        /// Almacena la instancia de trabajador o empleador.
        /// </summary>
        /// <value></value>
        public Usuario Trabajador_Empleador {get;set;}
        /// <summary>
        /// Almacena la calificacion del servicio.
        /// </summary>
        /// <value></value>
        public Calificacion Evaluacion {get;set;}
        /// <summary>
        /// Almacena el Contrato recuperado de debe Evaluar
        /// </summary>
        /// <value></value>
        public Contrato ContratoaCalificar{get;set;}


    }
}