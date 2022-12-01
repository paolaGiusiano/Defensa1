using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase que implementa el Patron Observer para notificar usuarios de eventos particulares que ocurren en el sistema
    /// </summary>
    public class PublisherServicio : Publisher
    {
        /// <summary>
        /// Servicio relacionado al publisher
        /// </summary>
        public Servicio Servicio;
        /// <summary>
        /// Constructor del publisher
        /// </summary>
        /// <param name="servicio">Servicio relacionado al publisher</param>
        public PublisherServicio(Servicio servicio)
        {
            this.Servicio = servicio;
        }
        /// <summary>
        /// Metodo que envia una notificacion a los subscriptores, a ser disparada cuando se de de baja un servicio
        /// </summary>
        /// <param name="razon">Razon por la que se dio de baja el servicio</param>
        public void NotificarDeBaja(string razon)
        {
            string mensaje = $"El servicio {this.Servicio.DescripcionServicio} de ID {this.Servicio.Id} ha sido removido del sistema por el siguiente motivo: {razon}. Por cualquier duda, contacte a nuestros administradores.";
            foreach(Persona sub in Subscriptores)
            {
                Notificacion notificacion = new Notificacion(mensaje);
                sub.Notificaciones.Add(notificacion);
            }
        }
    }
}