using System.Collections.Generic;
using System;

namespace CoreBot
{
    /// <summary>
    /// Notificacion individual que el usuario recibe
    /// </summary>
    public class Notificacion
    {
        /// <summary>
        /// Booleano que define si la notificacion fue leida o no
        /// </summary>
        public bool Leido {get; set;}
        /// <summary>
        /// Fecha en la que la notificacion fue enviada
        /// </summary>
        public DateTime FechaEnviado {get; set;}
        /// <summary>
        /// Fecha en la que la notificacion fue leida
        /// </summary>
        public DateTime FechaLeido {get; set;}
        /// <summary>
        /// Mensaje de la notificacion
        /// </summary>
        public string Mensaje {get; set;}
        /// <summary>
        /// Constructor de la notificacion
        /// </summary>
        /// <param name="mensaje">Mensaje a crear la notificacion</param>
        public Notificacion(string mensaje)
        {
            this.FechaEnviado = DateTime.Now;
            this.Leido = false;
            this.Mensaje = mensaje;
        }
        /// <summary>
        /// Metodo que marca la notificacion como leida
        /// </summary>
        public void MarcarLeido()
        {
            this.Leido = true;
            this.FechaLeido = DateTime.Now;
        }
        /// <summary>
        /// Metodo que hace un override a ToString para devolver la informacion de la notificacion en un string
        /// </summary>
        /// <returns>Un string con la fecha enviada y el mensaje</returns>
        public override string ToString()
        {
            return String.Format($"Recibido {this.FechaEnviado}: {this.Mensaje}");
        }
    }
}