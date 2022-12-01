using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace CoreBot
{
    /// <summary>
    /// Un programa que implementa un bot de Telegram.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// La instancia del bot.
        /// </summary>
        private static TelegramBotClient Bot;

        // El token provisto por Telegram al crear el bot. Mira el archivo README.md en la raíz de este repo para
        // obtener indicaciones sobre cómo configurarlo.
        private static string token = "5878862599:AAHc1hRKRADACCnBOixwDZeKf5HzByWIncI";

        private static IHandler handler;
        private static LocationApiClient client;

        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        public static void Main(string[] args)
        {
            client = new LocationApiClient();
            handler =
                new SaludoHandler(
                new DespedidaHandler(
                new BuscarServicioHandler(
                new ContratarServicioHandler(
                new CrearCategoriaHandler(
                new CrearServicioHandler(
                new EvaluarContratoHandler(
                new LeerNotificacionHandler(
                new RegistrarUsuarioHandler(
                new AgregarAdminHandler(
                new BuscarUsuarioHandler(
                new RemoverServicioHandler(null))))))))))));

            Bot = new TelegramBotClient(token);
            var cts = new CancellationTokenSource();

            // Comenzamos a escuchar mensajes. Esto se hace en otro hilo (en background). El primer método
            // HandleUpdateAsync es invocado por el bot cuando se recibe un mensaje. El segundo método HandleErrorAsync
            // es invocado cuando ocurre un error.
            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                },
                cts.Token
            );

            Console.WriteLine($"Bot is up!");

            // Esperamos a que el usuario aprete Enter en la consola para terminar el bot.
            Console.ReadLine();

            // Terminamos el bot.
            cts.Cancel();
        }

        /// <summary>
        /// Maneja las actualizaciones del bot (todo lo que llega), incluyendo mensajes, ediciones de mensajes,
        /// respuestas a botones, etc. En este ejemplo sólo manejamos mensajes de texto.
        /// </summary>
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // Sólo respondemos a mensajes de texto
                if (update.Type == UpdateType.Message)
                {
                    await HandleMessageReceived(botClient, update.Message);
                }
            }
            catch(Exception e)
            {
                await HandleErrorAsync(botClient, e, cancellationToken);
            }
        }

        private static async Task HandleMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Received a message from {message.From.FirstName} saying: {message.Text}");

            string response = string.Empty;

            handler.Handle(message, out response);

            if (!string.IsNullOrEmpty(response))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, response);
            }
        }
        /// <summary>
        /// Metodo que maneja un error del bot
        /// </summary>
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }
    }
}