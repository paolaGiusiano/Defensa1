using System;
using System.Collections.Generic;
namespace CoreBot
{
    /// <summary>
    /// La clase Empleador contiene la informacion del usuario que utilizara la plataforma para contratar los servicios de otros usuarios
    /// </summary>
    public class Empleador : Usuario
    {
        /// <summary>
        /// Constructor vacio de empleador para testing y genericos
        /// </summary>
        public Empleador()
        {

        }
        /// <summary>
        /// Constructor de la clase Empleador
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="email"></param>
        /// <param name="telefono"></param>
        /// <param name="direccion"></param>
        /// <param name="contraseña"></param>
        /// <param name="id">ID unico del usuario proporcionado por Telegram</param>
        public Empleador(string nombre, string apellido, string email,string telefono, string direccion,string contraseña, string id)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Email = email;
            this.Telefono = telefono;
            this.Direccion = direccion;
            this.ID = id;
            this.Contraseña=contraseña;
            Calificaciones = new List<Calificacion>();
        }
    }
}