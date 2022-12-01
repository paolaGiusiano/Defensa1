using System;

namespace CoreBot
{
    /// <summary>
    /// Clase con la responsablididad de detectar un tipo especifico de error, e informar cual es.
    /// </summary>
    public class InvalidInteger:BotException
    {
        /// <summary>
        /// Excepcion de servico no encontrado.
        /// </summary>
        public InvalidInteger()
        {
           CUSTOMMESSAGE = "El valor debe ser un numero entre 1 y 5.";

        }

    }

}