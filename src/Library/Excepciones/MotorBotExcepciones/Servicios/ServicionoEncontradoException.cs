using System;

namespace CoreBot
{
    /// <summary>
    /// Clase con la responsablididad de detectar un tipo especifico de error, e informar cual es.
    /// </summary>
    public class ServicionoEncontradoException:BotException
    {
        /// <summary>
        /// Excepcion de servico no encontrado.
        /// </summary>
        public ServicionoEncontradoException()
        {
           CUSTOMMESSAGE = "EL servicio no pudo ser encontrado.";

        }

    }

}