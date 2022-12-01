using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Clase de servicio singleton que poseera todos los servicios y se encargara de su mantenimiento
    /// </summary>
    public class GestorServicios
    {   
        /// <summary>
        /// Lista que contiene todos los servicios
        /// </summary>
        public List<Servicio> TodoServicios {get; set;} = new List<Servicio>();
        /// <summary>
        /// Lista que contiene todos los publishers de servicios que implementan Observer
        /// </summary>
        public List<PublisherServicio> TodoPublisherServicios {get; set;} = new List<PublisherServicio>();
        /// <summary>
        /// Instancia del gestor de servicios
        /// </summary>
        private static GestorServicios instance;

        private int count;

        /// <summary>
        /// Constructor vacio para clases de testing y utilidad
        /// </summary>
        public GestorServicios()
        {

        }

        /// <summary>
        /// Funcion de obtener instancia del singleton
        /// </summary>
        public static GestorServicios getInstance()
        {
            if (instance==null)
            {
                instance=new GestorServicios();
            }
            return instance;
        }
        /// <summary>
        /// Crea un servicio y llama al metodo de agregar el servicio a la lista de todos los servicios
        /// </summary>
        /// <param name="DescripcionServicio">Descripcion del servicio entrada por el usuario</param>
        /// <param name="pago">Tipo de pago para el servicio</param>
        /// <param name="cost">Forma de calcular el costo del servicio (por hora o a termino)</param>
        /// <param name="trabajador">Trabajador a cargo del servicio</param>
        /// <param name="categoria">Categoria del servicio</param>
        /// <param name="costo">Valor monetario a pagar por el servicio</param>
        public Servicio CrearServicio(string DescripcionServicio,Payment pago, Costo cost,Trabajador trabajador,Categoria categoria, decimal costo)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(DescripcionServicio), "La descripcion del servicio no puede ser vacia.");
            BotException.Precondicion(!string.IsNullOrEmpty(trabajador.ID), "El trabajador debe ser valido.");
            BotException.Precondicion(!string.IsNullOrEmpty(categoria.Nombre), "La categoria debe ser valida.");
            count ++;
            Servicio nuevoServicio = new Servicio(count,DescripcionServicio,pago,cost,trabajador,categoria,costo);
            PublisherServicio nuevoPublisher = new PublisherServicio(nuevoServicio);
            nuevoPublisher.AgregarSub(trabajador);
            this.AgregarServicio(nuevoServicio, nuevoPublisher);
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoServicio.DescripcionServicio), "El servicio no se creo correctamente.");
            //BotException.Postcondicion(nuevoServicio != TodoServicios.Find(Servicio=>Servicio.DescripcionServicio==DescripcionServicio), "No se cargo el servicio al gestor correctamente");
            return nuevoServicio;
        }
        /// <summary>
        /// Agrega un servicio a la lista de todos los servicios
        /// </summary>
        private void AgregarServicio(Servicio servicio, PublisherServicio publisher)
        {
            this.TodoServicios.Add(servicio);
            this.TodoPublisherServicios.Add(publisher);
        }

        /// <summary>
        /// Remueve un servicio de la lista de todos los servicios y setea sus valores a null
        /// </summary>
        public void RemoverServicio(Servicio servicio, Persona usuario, string motivo)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(servicio.DescripcionServicio), "El servicio debe ser valido");
            Autenticar.AutenticarAdmin(usuario);
            servicio.Trabajador.Servicios.Remove(servicio);
            PublisherServicio publisher = this.TodoPublisherServicios.Find(Publisher => Publisher.Servicio == servicio);
            publisher.NotificarDeBaja(motivo);
            this.TodoServicios.Remove(servicio);
            this.TodoPublisherServicios.Remove(publisher);
            //BotException.Postcondicion(string.IsNullOrEmpty(TodoServicios.Find(Servicio=>Servicio.DescripcionServicio==servicio.DescripcionServicio).DescripcionServicio), "El servicio no fue removido correctamente.");
        }
    }
}