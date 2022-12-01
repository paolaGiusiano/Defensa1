using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase Buscador
    /// </summary>
    [TestFixture]
    public class TestCategoria
    {
        /// <summary>
        /// Testea el nombre de la categoria
        /// </summary>
        [Test]
        public void TestNombre()
        {
            string expected = "Jardinero";
            Categoria c = new Categoria("Jardinero");
            Assert.AreEqual(c.Nombre, expected);

        }
        
    }
}