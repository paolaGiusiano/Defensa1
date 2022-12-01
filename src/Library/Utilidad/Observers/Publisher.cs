using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace CoreBot
{
    /// <summary>
    /// Clase que implementa el Patron Observer para notificar usuarios de eventos particulares que ocurren en el sistema
    /// </summary>
    public abstract class Publisher
    {
        /// <summary>
        /// Lista de personas que estan subscriptos al objeto
        /// </summary>
        /// <value></value>
        public List<Persona> Subscriptores {get; set;} = new List<Persona>();
        /// <summary>
        /// Metodo que agrega los subscriptores al publisher
        /// </summary>
        /// <param name="persona">Persona a ser agregada a los subscriptores</param>
        public void AgregarSub(Persona persona)
        {
            this.Subscriptores.Add(persona);
        }
    }
}