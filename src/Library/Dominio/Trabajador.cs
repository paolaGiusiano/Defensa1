using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace CoreBot
{
    /// <summary>
    /// La clase Trabajador contiene la informacion del usuario que utilizara la plataforma para ofrecer los servicios a otros usuarios
    /// </summary>
    public class Trabajador : Usuario
    {
        /// <summary>
        /// Lista de servicios que contiene el respectivo trabajador
        /// </summary>
        public List<Servicio> Servicios {get;set;} = new List<Servicio>();
        /// <summary>
        /// Constructor vacio del trabajador para testing y clases de utilidad
        /// </summary>
        public Trabajador()
        {

        }
        /// <summary>
        /// Constructor del trabajador con sus respectivos parametros
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="email"></param>
        /// <param name="telefono"></param>
        /// <param name="direccion"></param>
        /// <param name="contraseña"></param>
        /// <param name="id">ID unico del usuario proporcionado por Telegram</param>
        public Trabajador(string nombre, string apellido, string email,string telefono, string direccion,string contraseña, string id)
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
