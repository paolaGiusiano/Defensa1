
using System;

namespace CoreBot
{
    /// <summary>
    /// Clase que representa la calificacion con la que contaran los contratos en el sistema
    /// </summary>
    public class Calificacion
    {
        /// <summary>
        /// Almacena la fecha de creacion
        /// </summary>
        public DateTime FechaCreacion{get;private set;}
        /// <summary>
        /// Constructor de la clase calificacion
        /// </summary>
        public Calificacion()
        {
            this.FechaCreacion = DateTime.Now;
            this.Valor = 0;
        }
        /// <summary>
        /// Almacena el valor de la calificacion
        /// </summary>
        public int Valor{get; private set;}
        
        /// <summary>
        /// Devuelve la calificacion
        /// </summary>
        /// <returns></returns>
        public int CalcularCalificacion()
        {
            int dias = (DateTime.Now-FechaCreacion).Days;
            if( dias > 30 && this.Valor == 0)
            {
                return 3;
            }
            else
            {
                return this.Valor;
            }
        }
        /// <summary>
        /// Retorna la calificacion con el valor
        /// </summary>
        /// <param name="calificacion"></param>
        /// <returns></returns>
        public Calificacion AgregarCalificacion(int calificacion)
        {
            if(calificacion > 5 || calificacion < 0)
            {
                return null;
            }
            else
            {
                this.Valor = calificacion;
                return this;
            }
        }
    }
}