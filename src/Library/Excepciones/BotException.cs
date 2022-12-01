using System;

namespace CoreBot
{

/// <summary>
/// Clase Base de Excepcion
/// </summary>
    public class BotException:Exception
    {
        /// <summary>
        /// Mensaje que se mostrara al usuario
        /// </summary>
        /// <value></value>
        public string CUSTOMMESSAGE {get; protected set;}

        /// <summary>
        /// Constructor de la clase principal de excepciones.
        /// </summary>
        public BotException()
        {
           CUSTOMMESSAGE = "Error Genérico.";
        }

        /// <summary>
        /// Checker de precondiciones de un metodo
        /// </summary>
        /// <param name="condicion"></param>
        /// <param name="mensaje"></param>
        public static void Precondicion(bool condicion, string mensaje)
        {
            if(!condicion)
            {
                throw new Exception(mensaje);
            }
        }
        /// <summary>
        /// Checker de postcondiciones de un metodo
        /// </summary>
        /// <param name="condicion"></param>
        /// <param name="mensaje"></param>
        public static void Postcondicion(bool condicion, string mensaje)
        {
            if(!condicion)
            {
                throw new Exception(mensaje);
            }
        }
        /// <summary>
        /// Checker de invariantes de un metodo
        /// </summary>
        /// <param name="condicion"></param>
        /// <param name="mensaje"></param>
        public static void Invariante(bool condicion, string mensaje)
        {
            if(!condicion)
            {
                throw new Exception(mensaje);
            }
        }
    }

}