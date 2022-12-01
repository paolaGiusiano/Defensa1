using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Clase de servicio singleton que poseera todos los contratos y se encargara de su mantenimiento
    /// </summary>

    public class GestorContratos
    {   
        /// <summary>
        /// Lista que contiene todos los contratos
        /// </summary>
        public List<Contrato> TodoContratos {get; set;} = new List<Contrato>();
        /// <summary>
        /// Lista que contiene todos los publishers de contratos para implementar patron Observer
        /// </summary>
        public List<PublisherContrato> TodoPublisherContrato {get; set;} = new List<PublisherContrato>();
        /// <summary>
        /// Instancia del Gestor de contratos para Singleton
        /// </summary>

        private static GestorContratos instance;
        
        /// <summary>
        /// Constructor del gestor
        /// </summary>


        private GestorContratos()
        {

        }
        
        /// <summary>
        /// Funcion de obtener instancia del singleton
        /// </summary>        
        public static GestorContratos getInstance()
        {

            if (instance==null)
            {
                instance=new GestorContratos();
            }
            return instance;

        }
       
       /// <summary>
       /// <para>Retorna un Contrato en funcion del Id proporcionado.</para>
       /// 
       /// <para>Excepciones:</para>
       ///<para> ParameterInvalid:
       ///     Lanza una Excepcion si el id del parametro es null.</para>
       /// <para> ARgumentNullException:
       ///     Lanza una Excepcion si no se puede encotrar el Contrato.</para>
       /// </summary>
       /// <returns></returns>
        public Contrato BuscarContrato(string id)
        {
            string ID =  id?? throw new ParameterError();
            Contrato c = null;
            try
            {
                c = TodoContratos.Find(x => x.ID == ID );

            }catch(ArgumentNullException)
            {
                return null;

            }
            
            return c;

        }
        /// <summary>
        /// Metodo que crea un nuevo contrato a termino
        /// </summary>
        public Contrato CrearContrato(Usuario empleador, Servicio servicio)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(empleador.ID), "El empleador debe ser valido.");
            BotException.Precondicion(!string.IsNullOrEmpty(servicio.DescripcionServicio), "El servicio debe ser valido.");
            Autenticar.AutenticarEmpleador(empleador);
            Contrato nuevoContrato = new Contrato((Empleador)empleador,servicio);
            PublisherContrato nuevoPublisher = new PublisherContrato(nuevoContrato);
            nuevoPublisher.AgregarSub(empleador);
            nuevoPublisher.AgregarSub(servicio.Trabajador);
            this.AgregarContrato(nuevoContrato, nuevoPublisher);
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoContrato.ID), "El contrato no fue creado correctamente.");
            //BotException.Postcondicion(nuevoContrato != TodoContratos.Find(Contrato=>Contrato.ID==nuevoContrato.ID), "No se cargo el servicio al gestor correctamente");
            return nuevoContrato;
        }
        /// <summary>
        /// Metodo que crea un nuevo contrato por hora
        /// </summary>
        /// <param name="empleador">Empleador contratando el contrato</param>
        /// <param name="servicio">Servicio a contratar</param>
        /// <param name="hora">Cantidad de horas por las que se contrata el servicio</param>
        /// <returns>Un contrato cargado a la lista del gestor de contratos</returns>
        public Contrato CrearContratoHora(Usuario empleador, Servicio servicio, decimal hora)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(empleador.ID), "El empleador debe ser valido.");
            BotException.Precondicion(!string.IsNullOrEmpty(servicio.DescripcionServicio), "El servicio debe ser valido.");
            BotException.Precondicion(hora > 0, "La cantidad de horas debe ser mayor que cero.");
            Autenticar.AutenticarEmpleador(empleador);
            Contrato nuevoContrato = new Contrato((Empleador)empleador,servicio,hora);
            PublisherContrato nuevoPublisher = new PublisherContrato(nuevoContrato);
            nuevoPublisher.AgregarSub(empleador);
            nuevoPublisher.AgregarSub(servicio.Trabajador);
            this.AgregarContrato(nuevoContrato, nuevoPublisher);
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoContrato.ID), "El contrato no fue creado correctamente.");
            //BotException.Postcondicion(nuevoContrato != TodoContratos.Find(Contrato=>Contrato.ID==nuevoContrato.ID), "No se cargo el servicio al gestor correctamente");
            return nuevoContrato;
        }
        /// <summary>
        /// Metodo que agrega un contrato a la lista de todos los contratos
        /// </summary>

        private void AgregarContrato(Contrato contrato, PublisherContrato publisher)
        {

            this.TodoContratos.Add(contrato);
            this.TodoPublisherContrato.Add(publisher);
        }
        /// <summary>
        /// Metodo que busca y muestra el contrato
        /// </summary>

        public Contrato MostrarContrato(Usuario empleador, Servicio servicio)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(empleador.ID), "El empleador debe ser valido.");
            BotException.Precondicion(!string.IsNullOrEmpty(servicio.DescripcionServicio), "El servicio debe ser valido.");
            Autenticar.AutenticarEmpleador(empleador);
            Contrato contrato = this.TodoContratos.Find(c => c.Empleador == (Empleador)empleador && c.Servicio == servicio);
            BotException.Postcondicion(!string.IsNullOrEmpty(contrato.ID), "No se encontro un contrato con estas caracteristicas.");
            return contrato;
        }
        /// <summary>
        /// Metodo que evalua el contrato usado para implementar Facade - el metodo unifica mutliples pasos necesarios
        /// para evaluar un contrato - y OCP - el metodo se separa del usado por el empleador para permitir mayor apertura
        /// a la extension y menor necesidad de modificacion
        /// </summary>
        /// <param name="valor">Valor de la calificacion a darle al Empleador</param>
        /// <param name="contrato">Contrato a ser calificado</param>
        /// <param name="trabajador">Trabajador calificante</param>
        public void EvaluarContratoEmpleador(int valor, Contrato contrato, Persona trabajador)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(trabajador.ID), "El empleador debe ser valido");
            BotException.Precondicion(!string.IsNullOrEmpty(contrato.ID), "El contrato debe ser valido");
            BotException.Precondicion(valor >= 0, "El valor de la calificacion debe ser mayor que cero");
            Autenticar.AutenticarTrabajador(trabajador);
            Calificacion nuevaCalificacion = new Calificacion();
            nuevaCalificacion.AgregarCalificacion(valor);
            contrato.CalificarContratoEmpleador(contrato.Empleador, nuevaCalificacion);
            PublisherContrato publisher =this.TodoPublisherContrato.Find(Publisher => Publisher.Contrato == contrato);
            publisher.NotificarEvaluacion((Trabajador)trabajador);
        }
        /// <summary>
        /// Metodo que evalua el contrato usado para implementar Facade - el metodo unifica mutliples pasos necesarios
        /// para evaluar un contrato - y OCP - el metodo se separa del usado por el trabajador para permitir mayor apertura
        /// a la extension y menor necesidad de modificacion
        /// </summary>
        /// <param name="valor">Valor de la calificacion a darle al Trabajador</param>
        /// <param name="contrato">Contrato a ser calificado</param>
        /// <param name="empleador">Empleador calificante</param>
        public void EvaluarContratoTrabajdor(int valor, Contrato contrato, Persona empleador)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(empleador.ID), "El empleador debe ser valido");
            BotException.Precondicion(!string.IsNullOrEmpty(contrato.ID), "El contrato debe ser valido");
            BotException.Precondicion(valor >= 0, "El valor de la calificacion debe ser mayor que cero");
            Autenticar.AutenticarEmpleador(empleador);
            Calificacion nuevaCalificacion = new Calificacion();
            nuevaCalificacion.AgregarCalificacion(valor);
            contrato.CalificarContratoTrabajador(contrato.Trabajador, nuevaCalificacion);
            PublisherContrato publisher =this.TodoPublisherContrato.Find(Publisher => Publisher.Contrato == contrato);
            publisher.NotificarEvaluacion((Empleador)empleador);
        }
    }

}