using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase Buscador
    /// </summary>
    [TestFixture]
    public class TestAdministrador
    {
        /// <summary>
        /// Gestor de usuarios que contiene la lista total de todos los usuarios
        /// </summary>
        public GestorUsuario gestor = GestorUsuario.getInstance();
        /// <summary>
        /// Buscador de usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        
        /// <summary>
        /// Prueba que un Administrador es creado con dados parametros
        /// </summary>
        [Test]
        public void TestAgregarAdministrador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string contraseña="1234";
            string id = "0";

            Administrador nuevoAdministrador = gestor.AgregarAdmin(nombre, apellido, email,contraseña, id);

            Assert.AreEqual(nuevoAdministrador.Nombre, nombre);
            Assert.AreEqual(nuevoAdministrador.Apellido, apellido);
            Assert.AreEqual(nuevoAdministrador.Email, email);
            Assert.AreEqual(nuevoAdministrador.Contraseña, contraseña);
        }
        /// <summary>
        /// Prueba que si se solicita mostrar la informacion de un Administrador en base a su ID, el sistema devuelve la misma
        /// </summary>
        [Test]
        public void MostrarAdministrador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string contraseña="1234";
            string id = "0";

            Administrador expected = gestor.AgregarAdmin(nombre, apellido, email, contraseña, id);            
            Persona mostrarAdministrador = buscadorUsuarios.GetPersona(id);
            Assert.AreEqual((Administrador)mostrarAdministrador,expected);            
        }
        /// <summary>
        /// Prueba si el nombre del administador es el esperado
        /// </summary>
        [Test]   
        public void TestNombre()
        {
            Administrador administrador=new Administrador("Jorge","Rodriguez","jorgerodriguez@gmail.com","1234", "1");
            string NombreEsperado="Jorge";
            string NombreAdmin=administrador.Nombre;
            Assert.AreEqual(NombreEsperado,NombreAdmin);
        }
        /// <summary>
        /// Prueba si el apellido del administador es el esperado
        /// </summary>
        [Test]   
        public void TestApellido()
        {
            Administrador administrador=new Administrador("Jorge","Rodriguez","jorgerodriguez@gmail.com","1234", "1");
            string ApellidoEsperado="Rodriguez";
            string ApellidoAdmin=administrador.Apellido;
            Assert.AreEqual(ApellidoEsperado,ApellidoAdmin);
        }
        /// <summary>
        /// Prueba si el Email del administador es el esperado
        /// </summary>
        [Test]   
        public void TestEmail()
        {
            Administrador administrador=new Administrador("Jorge","Rodriguez","jorgerodriguez@gmail.com","1234", "1");
            string EmailEsperado="jorgerodriguez@gmail.com";
            string EmailAdmin=administrador.Email;
            Assert.AreEqual(EmailEsperado,EmailAdmin);
        }
        /// <summary>
        /// Prueba si la contraseña del administador es la esperada
        /// </summary>
        [Test]
        public void TestContraseña()
        {
            Administrador administrador=new Administrador("Jorge","Rodriguez","jorgerodriguez@gmail.com","1234", "1");
            string ContraseñaEsperada="1234";
            string ContraseñaAdmin=administrador.Contraseña;
            Assert.AreEqual(ContraseñaEsperada,ContraseñaAdmin);
        }
        /// <summary>
        /// Prueba si el id del administador es la esperada
        /// </summary>
        [Test]
        public void TestID()
        {
            Administrador administrador=new Administrador("Jorge","Rodriguez","jorgerodriguez@gmail.com","1234", "1");
            string IDEsperada="1";
            string IDAdmin=administrador.ID;
            Assert.AreEqual(IDEsperada,IDAdmin);
        }
    }
}
    
    
