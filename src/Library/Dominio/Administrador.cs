using System.Collections.Generic;
using System;

namespace CoreBot
{
    /// <summary>
    /// Clase de persona que representa al administrador del sistema
    /// </summary>
    public class Administrador : Persona
    {
        /// <summary>
        /// Constructor vacio para clases de utilidad y testing
        /// </summary>
        public Administrador()
        {

        }
        /// <summary>
        /// Constructor de la instancia de Administrador
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="id">ID unico del usuario proporcionado por Telegram</param>
        public Administrador(string nombre, string apellido, string email, string password, string id)
          {
               this.Nombre = nombre;
               this.Apellido = apellido;
               this.Email = email;
               this.ID = id;
               this.Contraseña = password;

        }
    }
}