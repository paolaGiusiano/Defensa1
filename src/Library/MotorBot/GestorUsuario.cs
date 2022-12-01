using System;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Clase de motor que maneja la existencia de los usuarios y administradores del sistema
    /// </summary>
    public class GestorUsuario
    {
        private static GestorUsuario instance;
        /// <summary>
        /// Cliente de LocationAPI
        /// </summary>
        public LocationApiClient client = new LocationApiClient();
        /// <summary>
        /// Lista completa de todas las personas del sistema
        /// </summary>
        public List<Persona> TodoPersonas {get; set;} = new List<Persona>();
        /// <summary>
        /// Lista completa de todos los usuarios del sistema
        /// </summary>
        public List<Usuario> TodoUsuarios {get; set;} = new List<Usuario>();
        /// <summary>
        /// Lista de todos los administradores
        /// </summary>
        public List<Administrador> Administradores {get; set;} = new List<Administrador>();
        /// <summary>
        /// Lista de todos los trabajadores
        /// </summary>
        public List<Trabajador> Trabajadores {get; set;} = new List<Trabajador>();
        /// <summary>
        /// Lista de todos los empleadores
        /// </summary>
        public List<Empleador> Empleadores {get; set;} = new List<Empleador>();

        /// <summary>
        /// Funcion de obtener instancia del singleton
        /// </summary>
        public static GestorUsuario getInstance()
        {
            if (instance==null)
            {
                instance=new GestorUsuario();
            }
            return instance;
        }
        /// <summary>
        /// Metodo que agrega un admin al sistema
        /// </summary>
        /// <param name="nombre">Nombre del admin</param>
        /// <param name="apellido">Apellido del admin</param>
        /// <param name="email">Email del admin</param>
        /// /// <param name="contraseña">Email del admin</param>
        /// <param name="id">ID unico del usuario proporcionado por Telegram</param>
        /// <returns></returns>
        public Administrador AgregarAdmin(string nombre, string apellido, string email,string contraseña, string id)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nombre), "El nombre no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(apellido), "El apellido no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(email), "El email no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(contraseña), "El contraseña no puede ser vacio.");
            
            Administrador nuevoAdmin = new Administrador(nombre, apellido, email,contraseña,id);
            this.Administradores.Add(nuevoAdmin);
            this.AgregarPersona(nuevoAdmin);

            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoAdmin.Nombre), "El nombre del nuevo admin no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoAdmin.Apellido), "El apellido del nuevo admin no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoAdmin.Email), "El email del nuevo admin no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoAdmin.Contraseña), "El password del nuevo admin no fue cargado correctamente.");
            //BotException.Postcondicion(nuevoAdmin != Administradores.Find(Admin=>Admin.ID == email), "El administrador no fue agregado a la lista correctamente.");
            return nuevoAdmin;
        }
        /// <summary>
        /// Metodo que agrega un empleador al sistema
        /// </summary>
        /// <param name="nombre">Nombre del empleador</param>
        /// <param name="apellido">Apellido del empleador</param>
        /// <param name="email">Email del empleador</param>
        /// <param name="telefono">Telefono del empleador</param>
        /// <param name="direccion">Direccion del empleador</param>
        /// <param name="contraseña">Direccion del empleador</param>
        /// <param name="id">ID unico del usuario proporcionado por Telegram</param>
        /// <returns></returns>
        public Empleador AgregarEmpleador(string nombre, string apellido, string email, string telefono, string direccion,string contraseña, string id)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nombre), "El nombre no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(apellido), "El apellido no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(email), "El email no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(contraseña), "El contraseña no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(telefono), "El telefono no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(direccion), "La direccion no puede ser vacia.");
            
            Empleador nuevoEmpleador = new Empleador(nombre, apellido, email, telefono, direccion,contraseña, id);
            this.Empleadores.Add(nuevoEmpleador);
            this.AgregarUsuario(nuevoEmpleador);
            
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoEmpleador.Nombre), "El nombre del nuevo empleador no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoEmpleador.Apellido), "El apellido del nuevo empleador no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoEmpleador.Email), "El email del nuevo empleador no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoEmpleador.Contraseña), "El password del nuevo empleador no fue cargado correctamente.");
            BotException.Precondicion(!string.IsNullOrEmpty(nuevoEmpleador.Telefono), "El telefono del nuevo empleador no fue cargado correctamente.");
            BotException.Precondicion(!string.IsNullOrEmpty(nuevoEmpleador.Direccion), "La direccion del nuevo empleador no fue cargado correctamente.");
            //BotException.Postcondicion(nuevoEmpleador != Empleadores.Find(Emp=>Emp.ID == email), "El empleador no fue agregado a la lista correctamente.");
            return nuevoEmpleador;
        }
        /// <summary>
        /// Metodo que agrega un trabajador al sistema
        /// </summary>
        /// <param name="nombre">Nombre del trabajador</param>
        /// <param name="apellido">Apellido del trabajador</param>
        /// <param name="email">Email del trabajador</param>
        /// <param name="telefono">Telefono del trabajador</param>
        /// <param name="direccion">Direccion del trabajador</param>
        /// <param name="contraseña">Direccion del trabajador</param>
        /// <param name="id">ID unico del usuario proporcionado por Telegram</param>
        /// <returns></returns>
        public Trabajador AgregarTrabajador(string nombre, string apellido, string email, string telefono, string direccion,string contraseña, string id)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nombre), "El nombre no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(apellido), "El apellido no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(email), "El email no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(contraseña), "El contraseña no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(telefono), "El telefono no puede ser vacio.");
            BotException.Precondicion(!string.IsNullOrEmpty(direccion), "La direccion no puede ser vacia.");
            
            Trabajador nuevoTrabajador = new Trabajador(nombre, apellido, email, telefono, direccion,contraseña, id);
            this.Trabajadores.Add(nuevoTrabajador);
            this.AgregarUsuario(nuevoTrabajador);
            
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoTrabajador.Nombre), "El nombre del nuevo trabajador no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoTrabajador.Apellido), "El apellido del nuevo trabajador no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoTrabajador.Email), "El email del nuevo trabajador no fue cargado correctamente.");
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevoTrabajador.Contraseña), "El password del nuevo trabajador no fue cargado correctamente.");
            BotException.Precondicion(!string.IsNullOrEmpty(nuevoTrabajador.Telefono), "El telefono del nuevo trabajador no fue cargado correctamente.");
            BotException.Precondicion(!string.IsNullOrEmpty(nuevoTrabajador.Direccion), "La direccion del nuevo trabajador no fue cargado correctamente.");
            //BotException.Postcondicion(nuevoTrabajador != Trabajadores.Find(Emp=>Emp.ID == email), "El trabajador no fue agregado a la lista correctamente.");
            return nuevoTrabajador;
        }
        
        /// <summary>
        /// Agrega un nuevo usuario a la lista de todos los usuarios
        /// </summary>
        /// <param name="nuevoUsuario">Un usuario valido, sea empleador o trabajador</param>
        private void AgregarUsuario(Usuario nuevoUsuario)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nuevoUsuario.ID), "El usuario debe ser valido."); 
            this.TodoUsuarios.Add(nuevoUsuario);
            BotException.Postcondicion(nuevoUsuario.ID == TodoUsuarios.Find(Usu=>Usu.ID == nuevoUsuario.ID).ID, "El usuario no fue agregado a la lista correctamente.");
            this.AgregarPersona(nuevoUsuario);
        }
        /// <summary>
        /// Agregar una nueva persona a la lista 
        /// </summary>
        /// <param name="nuevaPersona">Una persona valida, sea empleador, trabajador o admin</param>
        private void AgregarPersona(Persona nuevaPersona)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nuevaPersona.ID), "La persona debe ser valido."); 
            this.TodoPersonas.Add(nuevaPersona);
            //BotException.Postcondicion(nuevaPersona.ID == TodoPersonas.Find(Per=>Per.ID == nuevaPersona.EMAIL).ID, "La persona no fue agregada a la lista correctamente.");
        }
        /// <summary>
        /// Remueve Administradores de las listas del gestor
        /// </summary>
        /// <param name="viejoAdmin"></param>
        public void RemoverAdmin(Administrador viejoAdmin)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(viejoAdmin.ID), "El admin debe ser valido."); 
            this.Administradores.Remove(viejoAdmin);
            this.RemoverPersona(viejoAdmin);
            //BotException.Postcondicion(string.IsNullOrEmpty(Administradores.Find(Admin=>Admin.ID == viejoAdmin.ID).ID), "La persona no fue removida de la lista correctamente.");
        }
        /// <summary>
        /// Remueve Trabajadores de las listas del gestor
        /// </summary>
        /// <param name="viejoTrabajador"></param>
        public void RemoverTrabajador(Trabajador viejoTrabajador)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(viejoTrabajador.ID), "El trabajador debe ser valido."); 
            this.Trabajadores.Remove(viejoTrabajador);
            this.RemoverUsuario(viejoTrabajador);
            //BotException.Postcondicion(string.IsNullOrEmpty(Trabajadores.Find(Trabajador=>Trabajador.ID == viejoTrabajador.ID).ID), "El trabajador no fue removido de la lista correctamente.");
        }
        /// <summary>
        /// Remueve Empleadores de las listas del gestor
        /// </summary>
        /// <param name="viejoEmpleador"></param>
        public void RemoverEmpleador(Empleador viejoEmpleador)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(viejoEmpleador.ID), "El empelador debe ser valido."); 
            this.Empleadores.Remove(viejoEmpleador);
            this.RemoverUsuario(viejoEmpleador);
            //BotException.Postcondicion(string.IsNullOrEmpty(Empleadores.Find(Empleador=>Empleador.ID == viejoEmpleador.ID).ID), "El empleador no fue removido de la lista correctamente.");
        }
        /// <summary>
        /// Remueve un usuario de la lista de usuario y llama a remover Persona
        /// </summary>
        /// <param name="viejoUsuario"></param>
        private void RemoverUsuario(Usuario viejoUsuario)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(viejoUsuario.ID), "El usuario debe ser valido."); 
            this.TodoUsuarios.Remove(viejoUsuario);
            this.RemoverPersona(viejoUsuario);
            //BotException.Postcondicion(string.IsNullOrEmpty(TodoUsuarios.Find(usuario=>usuario.ID == viejoUsuario.ID).ID), "El usuario no fue removido de la lista correctamente.");
        }
        /// <summary>
        /// Remueve una Persona de la lista de Personas
        /// </summary>
        /// <param name="viejoPersona"></param>
        private void RemoverPersona(Persona viejoPersona)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(viejoPersona.ID), "La persona debe ser valido."); 
            this.TodoPersonas.Remove(viejoPersona);
            //BotException.Postcondicion(string.IsNullOrEmpty(TodoPersonas.Find(Persona=>Persona.ID == viejoPersona.ID).ID), "La persona no fue removida de la lista correctamente.");
        }
    }
}