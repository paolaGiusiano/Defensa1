using Telegram.Bot.Types;

namespace CoreBot;

/// <summary>
/// Interfaz para implementar el patron Chain of Responsability.
/// La interfaz se crea en funcion del principio de inversion de dependencias,
/// para que los clientes de la cadena de responsabilidad, que pueden ser concretos,
/// no sean dependan de una clase "handler" que potencialmente es abstracta
/// </summary>
public interface IHandler
{
    /// <summary>
    /// Obtiene el proximo handler
    /// </summary>
    /// <value>El handler que sera invocado si este handler no procesa el mensaje</value>
    IHandler Next {get; set;}
    /// <summary>
    /// Procesa el mensaje, o pasa al siguiente handler si no puede
    /// </summary>
    /// <param name="message">Mensaje a procesar</param>
    /// <param name="response">La respuesta al mensaje a procesar</param>
    /// <returns>El handler que proceso el mensaje, o null si no se pudo procesar</returns>
    IHandler Handle(Message message, out string response);
    /// <summary>
    /// Retorna el handler al estado inicial y cancela el proximo handler si existe
    /// </summary>
    void Cancel();
}