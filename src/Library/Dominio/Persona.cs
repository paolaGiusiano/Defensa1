using System;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Clase persona la cual contiene atributos que tienen todos los usuarios del sistema
    /// </summary>
    public class Persona : ICampoUnico
    {
        /// <summary>
        /// ID unico de una persona, sea Admin, Trabajador o Empleador. Normalmente, el ID sera proporcionado por Telegram
        /// </summary>
        public string ID{get;set;}
        /// <summary>
        /// Nombre de la persona
        /// </summary>
        /// <value></value>
        public string Nombre{get;set;}
        /// <summary>
        /// Apellido de la persona
        /// </summary>
        /// <value></value>
        public string Apellido{get;set;}
        /// <summary>
        /// Email de la persona, el cuál es único
        /// </summary>
        /// <value></value>
        public string Email{get;set;}
        /// <summary>
        /// Contraseña de la persona
        /// </summary>
        public string Contraseña{get;set;}  
        /// <summary>
        /// Lista de notificaciones del usuario
        /// </summary>
        public List<Notificacion> Notificaciones = new List<Notificacion>();
        /// <summary>
        /// Buscador de ID unica del sistema
        /// </summary>
        public string GetCampoUnico() // Para que pueda pasar por la clase generica de listas
        {
            return ID;
        }
    }
}