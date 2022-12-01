using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Clase buscadora de usuarios
    /// Implementa patron Facade para simplificar la declaracion de funcionalidades y reducir la cantidad de acoplamiento
    /// a procesos complejos del gestor
    /// </summary>
    public class BuscadorUsuarios
    {
        /// <summary>
        /// Llamado al gestor de usuarios singleton
        /// </summary>
        GestorUsuario gestor = GestorUsuario.getInstance();
        /// <summary>
        /// Instancia signleton del buscador usuario
        /// </summary>
        private static BuscadorUsuarios instance;
        /// <summary>
        /// Funcion de obtener instancia del singleton
        /// </summary>
        /// <returns></returns>
        public static BuscadorUsuarios getInstance()
        {
            if (instance==null)
            {
                instance=new BuscadorUsuarios();
            }
            return instance;
        }

        /// <summary>
        /// Metodo que busca y muestra un usuario basado en su ID unico
        /// </summary>
        /// <param name="ID">ID unico de usuario</param>
        /// <returns></returns>
        public Usuario MostrarUsuario(string ID)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(ID), "El ID de busqueda no debe ser vacio.");
            Usuario usuario = gestor.TodoUsuarios.Find(s => s.ID == ID);
            //BotException.Postcondicion(!string.IsNullOrEmpty(usuario.Nombre), "La busqueda no devolvio nada.");
            return usuario;
        }
        /// <summary>
        /// Devuelve una persona en base a su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Persona GetPersona(string id)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(id), "El ID de busqueda no debe ser vacio.");
            Persona persona = gestor.TodoPersonas.Find(p => p.ID == id);
            BotException.Precondicion(!string.IsNullOrEmpty(persona.Nombre), "La busqueda no dio resultado.");
            return persona;
        }
        /// <summary>
        /// Devuelve una persona en base a su email
        /// </summary>
        /// <param name="email">Email de la persona a buscar</param>
        /// <returns>Una persona</returns>
        public Persona MostrarPersonaEmail(string email)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(email), "El email de busqueda no debe ser vacio.");
            Persona persona = gestor.TodoPersonas.Find(p => p.Email == email);
            BotException.Postcondicion(!string.IsNullOrEmpty(persona.Nombre), "La busqueda no dio resultado.");
            return persona;
        }
    }
}