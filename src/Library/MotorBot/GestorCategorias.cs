using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot
{
    /// <summary>
    /// Gestor singleton de las categorias del sistema
    /// </summary>
    public class GestorCategorias
    {
        private static GestorCategorias instance{get;set;}
        /// <summary>
        /// Lista de todas las categorias del sistema
        /// </summary>
        public List<Categoria> TodaCategoria {get;set;} = new List<Categoria>();
        /// <summary>
        /// Constructor del gestor
        /// </summary>
        public GestorCategorias()
        {
            Categoria categoriaDefault = new Categoria("Otros");
            TodaCategoria.Add(categoriaDefault);
        }
        /// <summary>
        /// Metodo que llama a la instancia unica del constructor
        /// </summary>
        public static GestorCategorias getInstance()
        {
            if (instance == null)
            {
                instance = new GestorCategorias();
            }
            return instance;
        }

        /// <summary>
        /// Metodo que crea una categoria. Funcionalmente sera accedido solo por el Admin
        /// </summary>
        /// <param name="nombre">Nombre que la categoria nueva va a tener</param>
        /// <param name="usuario">Usuario usando la funcionalidad</param>
        public Categoria CrearCategoria(string nombre, Persona usuario)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(nombre), "El nombre de una categoria no puede ser vacio.");
            Autenticar.AutenticarAdmin(usuario);
            Categoria nuevaCategoria = new Categoria(nombre);
            this.AgregarCategoria(nuevaCategoria);
            BotException.Postcondicion(!string.IsNullOrEmpty(nuevaCategoria.Nombre), "La categoria no fue creada correctamente.");
            //BotException.Postcondicion(nuevaCategoria != TodaCategoria.Find(Categoria=>Categoria.Nombre==nombre), "No se cargo la categoria al gestor correctamente");
            return nuevaCategoria;
        }
        /// <summary>
        /// Metodo que remueve una categoria de la lista de todas las categorias del sistema
        /// </summary>
        /// <param name="viejaCategoria">Categoria a remover</param>
        /// <param name="usuario">Usuario accediendo a la funcionalidad</param>
        public void RemoverCategoria(Categoria viejaCategoria, Persona usuario)
        {
            BotException.Precondicion(!string.IsNullOrEmpty(viejaCategoria.Nombre), "El nombre de una categoria no puede ser vacio.");
            Autenticar.AutenticarAdmin(usuario);
            this.TodaCategoria.Remove(viejaCategoria);
        }
        /// <summary>
        /// Metodo que agrega una categoria a la lista de todas las categorias del sistema
        /// </summary>
        /// <param name="nuevaCategoria">Categoria a agregar a la lista</param>
        private void AgregarCategoria(Categoria nuevaCategoria)
        {
            this.TodaCategoria.Add(nuevaCategoria);
        }
        /// <summary>
        /// Metodo que devuelve todos los nombres de todas las categorias guardadas.
        /// </summary>
        /// <returns>String conteniendo todos los nombres de las categorias separados por punto y comas</returns>
        public string AllNames()
        {
            List<string> nombres = new List<string>();
            foreach(Categoria categoria in TodaCategoria)
            {
                nombres.Add(categoria.Nombre);
            }
            return string.Join(";", nombres);
        }
    }
}