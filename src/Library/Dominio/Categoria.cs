using System;

namespace CoreBot
{
    /// <summary>
    /// Categoria de un servicio, actua como un tag de la misma
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Nombre de la categoria del servicio
        /// </summary>
        public string Nombre;

        /// <summary>
        /// Constructor de categoria
        /// </summary>
        /// <param name="nombre"></param>
        public Categoria(string nombre)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nombre), "El nombre de la categoria no puede ser vacio.");
            this.Nombre = nombre;
        }
    }
}