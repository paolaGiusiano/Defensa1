using System;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Clase que define usuarios Empleadores y Trabajadores
    /// </summary>
    public class Usuario : Persona
    {
        /// <summary>
        /// Telefono del usuario
        /// </summary>
        public string Telefono {get;set;}
        /// <summary>
        /// Direccion de alojamiento del usuario
        /// </summary>
        public string Direccion {get;set;}
        /// <summary>
        /// Calificacion total del usuario, promedio de todas las calificaciones
        /// </summary>
        public int CalificacionTotal {get;set;}
        /// <summary>
        /// Lista de todas las calificaciones del usuario
        /// </summary>
        public List<Calificacion> Calificaciones {get;set;}
        
        /// <summary>
        /// Metodo modificador del objeto
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="email"></param>
        /// <param name="telefono"></param>
        /// <param name="direccion"></param>
        /// <param name="contraseña"></param>
        public void ModificarUsuario(string nombre, string apellido, string email,string telefono, string direccion,string contraseña)
        {            
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Email = email;
            this.Telefono = telefono;
            this.Direccion = direccion;
            this.Contraseña=contraseña;
        }
        /// <summary>
        /// Metodo destructor del objeto
        /// </summary>
        public void BorrarUsuario()
        {            
            this.Nombre = null;
            this.Apellido = null;
            this.Email = null;
            this.Telefono = null;
            this.Direccion = null;
            this.Direccion = null;
            this.Contraseña=null;
            this.CalificacionTotal = 0;
            this.Calificaciones = null;
        }       
        /// <summary>
        /// Metodo que agrega una calificacion al usuario
        /// </summary>
        /// <param name="calificacion">Calificacion a ser agregada</param>
        public virtual void AgregarCalificacion(Calificacion calificacion)
        {
            this.Calificaciones.Add(calificacion);
            this.CalcularCalificacion();
        }
        
        /// <summary>
        /// Metodo que funciona para calcular la calificacion total dentro de la lista de calificaciones
        /// </summary>   
        public int CalcularCalificacion()
        {
            int aux = 0;
            foreach(Calificacion calificacion in Calificaciones)
            {
                aux = aux + calificacion.Valor;

            }
            
            aux = aux/Calificaciones.Count;
            CalificacionTotal = aux;
            return CalificacionTotal;

        }
        /// <summary>
        /// Override del metodo ToString que convierte el Usuario a un string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Nombre} {this.Apellido}\r\nID: {this.ID}; Calificacion: {this.CalificacionTotal} \r\nInformacion de contacto: {this.Email}, {this.Telefono}\r\nDireccion: {this.Direccion}";
        }
    }
}