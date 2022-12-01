using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase GestorCategoria
    /// </summary>
    [TestFixture]
    public class TestGestorCategorias
    {
        GestorCategorias gestor = GestorCategorias.getInstance();
        Administrador admin = new Administrador();

        /// <summary>
        /// Prueba que el gestor crea una categoria nueva y la agrega a su lista basado en un nombre
        /// </summary>
        [Test]
        public void TestCrearCategoria()
        {
            string Nombre = "Nueva Categoria";
            Categoria nuevaCategoria = gestor.CrearCategoria(Nombre, admin);

            Assert.AreEqual(nuevaCategoria,gestor.TodaCategoria.Find(Categoria=>Categoria.Nombre==Nombre));
        }
        /// <summary>
        /// Prueba que al remover una categoria del gestor, esta ya no esta presente en la lista de categorias
        /// </summary>
        [Test]
        public void TestRemoverCategoria()
        {
            string Nombre = "Categoria a Remover";
            Categoria nuevaCategoria = gestor.CrearCategoria(Nombre, admin);

            gestor.RemoverCategoria(nuevaCategoria, admin);

            Assert.IsNull(gestor.TodaCategoria.Find(Categoria=>Categoria.Nombre==Nombre));
        }
    }
}