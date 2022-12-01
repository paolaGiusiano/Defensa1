using Telegram.Bot.Types;
using System.Collections.Generic;

namespace CoreBot
{

    public class MayorCalificacion2: BaseHandler{

        //copio y pego codigo similar y les hago los cambios que corresponde

        public Calificacion2State State {get; set;} 

        public Calificacion2Data Data {get; set;}

        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Buscador de usuarios singleton que busca un usuario en la lista de todos los usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuario = BuscadorUsuarios.getInstance();


        public MayorCalificacion2(BaseHandler next)
            : base(new string[] {"empleador"}, next)
        {
            this.State = Calificacion2State.Start;
            this.Data = new Calificacion2Data();
        }

        /// <summary>
        /// Override de la clase base de los handlers para chequear si el mensaje puede ser procesado
        /// </summary>
        /// <param name="message">Mensaje recibido del usuario</param>
        /// <returns>True si se puede procesar o </returns>
        protected override bool CanHandle(Message message)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            if(this.State == Calificacion2State.Start)
            {
                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }


         protected override void InternalHandle(Message message, out string response)
        {
            BotException.Precondicion(message.Text != "" && message.From != null, "No hay mensaje o nadie esta mandando el mensaje.");
            BotException.Precondicion(buscadorUsuario.GetPersona(message.From.Id.ToString()) is Administrador, "Usuario no tiene acceso a esta funcionalidad");
            if(string.Equals(message.Text, "cancelar", System.StringComparison.InvariantCultureIgnoreCase))
            {
                response = "Cancelando operacion.";
                Cancel();
            }
            else
            {
                response = "";
                switch(State)
                {
                    case Calificacion2State.Start :
                        if (buscadorUsuario.GetPersona(message.From.Id.ToString()) is Administrador)
                            this.State = Calificacion2State.Verificacion;
                            response = "contaseña de nuevo";
                        break;
                    case Calificacion2State.Verificacion :
                        this.Data.Key = message.Text;
                        if(this.Data.Key == "P2Bot")
                        { 
                            this.State = Calificacion2State.Start;
                            response = $"nombre del empleador que desea";
                            InternalCancel();
                        }
                        else{
                            response = "Acceso denegado";
                        }
                        break;
                    case Calificacion2State.empleadorBuascado :
                        if (message.Text == ""){
                            response = $"Error, vacio!";
                        }
                        else{
                            response = $"Ok para proseguir";
                        }
                        break;
                    
                }    
            }
        }

        protected override void InternalCancel()
        {
            this.State = Calificacion2State.Start;
            this.Data = new Calificacion2Data();
        }

        public enum Calificacion2State
        {
            /// <summary>
            /// Estado inicial del comando
            /// </summary>
            Start,
            /// <summary>
            ///
            /// </summary>
            Verificacion,
            /// <summary>
            /// Estado en el que se espera el nombre del nuevo admin
            /// </summary>
            empleadorBuascado,
            /// <summary>
            /// Estado en el que se espera el apellido del nuevo admin
            /// </summary>
            

        }


    public class Calificacion2Data{
        public string Key {get; set;}

    }


    }

}