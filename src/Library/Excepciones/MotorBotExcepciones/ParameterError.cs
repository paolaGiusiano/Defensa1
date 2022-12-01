using System;

namespace CoreBot
{
    /// <summary>
    /// Clase con la responsablididad de detectar un tipo especifico de error, e informar cual es.
    /// </summary>
    public class ParameterError:BotException
    {
        /// <summary>
        /// Excepcion de servico no encontrado.
        /// </summary>
        public ParameterError()
        {
           CUSTOMMESSAGE = "El parametro proporcionado no puede estar vacio.";

        }

    }

}