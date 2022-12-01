using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Handler del patron Chain of Responsability que implementa los comandos de saludo 
    /// que normalmente inicializaran el bot
    /// </summary>
    public class SaludoHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SaludoHandler"/>. Esta clase inicializa los saludos
        /// </summary>
        /// <param name="next">El proximo handler</param>
        public SaludoHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new string[] {"hola", "buenas", "hello"};
        }
        /// <summary>
        /// Procesa el mensaje "hola", "buenas" o "hello" y retorna true; retorna false en caso contrario
        /// </summary>
        /// <param name="message">Mensaje a procesar</param>
        /// <param name="response">Respuesta a mensaje procesado</param>
        /// <returns>true si el mensaje fue procesado; false de lo contrario</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            response = "Hola! Bienvenido al Bot de Trabajo!";
        }
    }
}