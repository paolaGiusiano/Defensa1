using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase que implementa el Patron Observer para notificar usuarios subscritos a un contrato de cambios hechos al mismo
    /// </summary>
    public class PublisherContrato : Publisher
    {
        /// <summary>
        /// Contrato que el publisher esta relacionado
        /// </summary>
        public Contrato Contrato;
        /// <summary>
        /// Constructor del publisher
        /// </summary>
        /// <param name="contrato">Contrato al que el publisher estara relacionado</param>
        public PublisherContrato(Contrato contrato)
        {
            this.Contrato = contrato;
        }
        /// <summary>
        /// Metodo que envia una notificacion a los subscriptores del publisher, a ser disparada cuando se evalue el contrato relacionado
        /// </summary>
        /// <param name="calificante">Usuario que califico el contrato</param>
        public void NotificarEvaluacion(Usuario calificante)
        {
            string mensaje = $"El usuario {calificante.Nombre} {calificante.Apellido} ha calificado el contrato {this.Contrato.ID} por el servicio {this.Contrato.Servicio.DescripcionServicio}.";
            foreach(Persona sub in Subscriptores)
            {
                Notificacion notificacion = new Notificacion(mensaje);
                sub.Notificaciones.Add(notificacion);
            }
        }
    }
}