using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Clase que confirma el acceso del usuario tratando de acceder a una funcionalidad
    /// A ser usada en los gestores y buscadores que implementan Facade
    /// </summary>
    public class Autenticar
    {
        /// <summary>
        /// Mensaje de error a enviar si el usuario no tiene acceso
        /// </summary>
        public static string SinAcceso = "El usuario no tiene acceso a esta funcionalidad.";
        /// <summary>
        /// Confirma que el usuario es un administrador
        /// </summary>
        /// <param name="usuario">Usuario loggeado</param>
        public static void AutenticarAdmin(Persona usuario)
        {
            BotException.Invariante(usuario is Administrador, SinAcceso);
        }
        /// <summary>
        /// Confirma que el usuario es un empleador
        /// </summary>
        /// <param name="usuario">Usuario loggeado</param>
        public static void AutenticarEmpleador(Persona usuario)
        {
            BotException.Invariante(usuario is Empleador, SinAcceso);
        }
        /// <summary>
        /// Confirma que el usuario es un trabajador
        /// </summary>
        /// <param name="usuario">Usuario loggeado</param>
        public static void AutenticarTrabajador(Persona usuario)
        {
            BotException.Invariante(usuario is Trabajador, SinAcceso);
        }
    }
}