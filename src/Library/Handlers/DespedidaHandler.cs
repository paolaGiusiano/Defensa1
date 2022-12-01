using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Handler del patron Chain of Responsability que implementa los comandos de despedida
    /// que normalmente marcaran el final de una sesion de usuario con el bot
    /// </summary>
    public class DespedidaHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una instancia de <see cref="DespedidaHandler"/>. Esta clase inicializa las despedidas
        /// </summary>
        /// <param name="next">El proximo handler</param>
        public DespedidaHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new string[] {"chau", "adios", "eso es todo"};
        }
        /// <summary>
        /// Procesa los mensajes "chau", "adios" y "eso es todo"
        /// </summary>
        /// <param name="message">Mensaje a procesar</param>
        /// <param name="response">Respuesta del mensaje a procesar</param>
        /// <returns>true si lo pudo procesar; false si no</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            response = "Gracias por usar nuestros servicios. Que tengas buen dia!";
        }
    }
}