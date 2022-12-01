using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase GestorUsuario
    /// </summary>
    [TestFixture]
    public class TestGestorUsuario
    {
        /// <summary>
        /// Instancia del gestor de usuario singleton
        /// </summary>
        public GestorUsuario gestor = GestorUsuario.getInstance();
        
        /// <summary>
        /// TestAgregarAdmin prueba que se creo un admin con los parámetros pasados
        /// </summary>
        [Test]
        public void TestAgregarAdmin()
        {
            Administrador nuevoAdmi = gestor.AgregarAdmin("Jose", "Giusiano", "aaa@gmail.com", "1234", "0");
            Assert.AreEqual(nuevoAdmi.Nombre, "Jose");
            Assert.AreEqual(nuevoAdmi.Apellido, "Giusiano");
            Assert.AreEqual(nuevoAdmi.Email, "aaa@gmail.com");
            Assert.AreEqual(nuevoAdmi.Contraseña, "1234");

        }


        /// <summary>
        /// TestAgregarEmpleador prueba que se creo un empleador con los parámetros pasados
        /// </summary>
        [Test]
        public void TestAgregarEmpleador()
        {
            Empleador nuevoEmpleador = gestor.AgregarEmpleador("Jose", "Giusiano", "aaa@gmail.com","111111111","mercedes 1111", "1234", "1");
            Assert.AreEqual(nuevoEmpleador.Nombre, "Jose");
            Assert.AreEqual(nuevoEmpleador.Apellido, "Giusiano");
            Assert.AreEqual(nuevoEmpleador.Email, "aaa@gmail.com");
            Assert.AreEqual(nuevoEmpleador.Telefono, "111111111");
            Assert.AreEqual(nuevoEmpleador.Direccion, "mercedes 1111");
            Assert.AreEqual(nuevoEmpleador.Contraseña, "1234");

        }



        /// <summary>
        /// TestAgregarTrabajador prueba que se creo un trabajador con los parámetros pasados
        /// </summary>
        [Test]
        public void TestAgregarTrabajador()
        {
            Trabajador nuevoTrabajador = gestor.AgregarTrabajador("Jose", "Giusiano", "aaa@gmail.com","111111111","mercedes 1111", "1234", "2");
            Assert.AreEqual(nuevoTrabajador.Nombre, "Jose");
            Assert.AreEqual(nuevoTrabajador.Apellido, "Giusiano");
            Assert.AreEqual(nuevoTrabajador.Email, "aaa@gmail.com");
            Assert.AreEqual(nuevoTrabajador.Telefono, "111111111");
            Assert.AreEqual(nuevoTrabajador.Direccion, "mercedes 1111");
            Assert.AreEqual(nuevoTrabajador.Contraseña, "1234");

        }


        /// <summary>
        /// TestRemoverAdmin prueba si se elimino el admin que se quiere elimanar
        /// </summary>
        [Test]
        public void TestRemoverAdmin()
        {
            Administrador nuevoAdmi = gestor.AgregarAdmin("Juan", "Perez", "bbb@gmail.com", "5678", "3");
            gestor.RemoverAdmin(nuevoAdmi);
            int i = 0;
            foreach(Administrador admi in gestor.Administradores){
                if (admi.Equals(nuevoAdmi)){
                    i++;
                }
            }
            Assert.AreEqual(i, 0);
        
        }


        /// <summary>
        /// TestRemoverTrabajador prueba si se elimino de la lista trabajadores el trabajador que se quiere elimanar
        /// </summary>
        [Test]
        public void TestRemoverTrabajador()
        {
            Trabajador nuevoTrabajador = gestor.AgregarTrabajador("Jose", "Giusiano", "aaa@gmail.com","111111111","mercedes 1111", "1234", "4");
            gestor.RemoverTrabajador(nuevoTrabajador);
            int i = 0;
            foreach(Trabajador trabajador in gestor.Trabajadores){
                if (trabajador.Equals(nuevoTrabajador)){
                    i++;
                }
            }
            Assert.AreEqual(i, 0);
        }


        /// <summary>
        /// TestRemoverEmpleador prueba si se elimino de la lista Empleadores el empleador que se quiere elimanar
        /// </summary>
        [Test]
        public void TestRemoverEmpleador()
        {
            Empleador nuevoEmpleador = gestor.AgregarEmpleador("Jose", "Giusiano", "aaa@gmail.com","111111111","mercedes 1111", "1234", "5");
            gestor.RemoverEmpleador(nuevoEmpleador);
            int i = 0;
            foreach(Empleador empleador in gestor.Empleadores){
                if (empleador.Equals(nuevoEmpleador)){
                    i++;
                }
            }
            Assert.AreEqual(i, 0);
        
        }



    }

}