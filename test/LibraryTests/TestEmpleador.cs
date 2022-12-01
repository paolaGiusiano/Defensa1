using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba la creacion, modificacion y borrado de un usuario Empleador
    /// </summary>
    [TestFixture]
    public class TestEmpleador
    {
        /// <summary>
        /// Gestor de usuarios que contiene la lista total de todos los usuarios
        /// </summary>
        public GestorUsuario gestor = GestorUsuario.getInstance();
        /// <summary>
        /// Buscador de usuarios que obtiene a los usuarios
        /// </summary>
        public BuscadorUsuarios buscador = BuscadorUsuarios.getInstance();
        
        /// <summary>
        /// Prueba que un Empleador es creado con dados parametros
        /// </summary>
        [Test]
        public void TestCrearEmpleador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string contraseña="1234";
            string id = "1";

            Empleador nuevoEmpleador = gestor.AgregarEmpleador(nombre, apellido, email, telefono, direccion,contraseña, id);

            Assert.AreEqual(nuevoEmpleador.Nombre, nombre);
            Assert.AreEqual(nuevoEmpleador.Apellido, apellido);
            Assert.AreEqual(nuevoEmpleador.Email, email);
            Assert.AreEqual(nuevoEmpleador.Telefono, telefono);
            Assert.AreEqual(nuevoEmpleador.Direccion, direccion);
            Assert.AreEqual(nuevoEmpleador.Contraseña, contraseña);
        }
        /// <summary>
        /// Prueba que si se modifica un Empleador para que tenga un valor dado, el valor es el esperado
        /// </summary>
        [Test]
        public void TestModificarEmpleador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string contraseña="1234";
            string id = "1";

            Empleador nuevoEmpleador = gestor.AgregarEmpleador(nombre, apellido, email, telefono, direccion,contraseña, id);
            nuevoEmpleador.ModificarUsuario("Robert", nuevoEmpleador.Apellido, nuevoEmpleador.Email, nuevoEmpleador.Telefono, nuevoEmpleador.Direccion,nuevoEmpleador.Contraseña);

            Assert.AreEqual(nuevoEmpleador.Nombre, "Robert");
        }
        /// <summary>
        /// Prueba que si se borra un Empleador, sus valores son null
        /// </summary>
        [Test]
        public void TestBorrarEmpleador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string contraseña="1234";
            string id = "1";

            Empleador nuevoEmpleador = gestor.AgregarEmpleador(nombre, apellido, email, telefono, direccion,contraseña, id);
            nuevoEmpleador.BorrarUsuario();
            
            Assert.IsNull(nuevoEmpleador.Nombre);
            Assert.IsNull(nuevoEmpleador.Apellido);
            Assert.IsNull(nuevoEmpleador.Calificaciones);
            Assert.IsNull(nuevoEmpleador.Email);
            Assert.IsNull(nuevoEmpleador.Direccion);
            Assert.IsNull(nuevoEmpleador.Contraseña);
            Assert.AreEqual(nuevoEmpleador.CalificacionTotal,0);
        }
        /// <summary>
        /// Prueba que si se solicita mostrar la informacion de un Empleador en base a su ID, el sistema devuelve la misma
        /// </summary>
        [Test]
        public void MostrarEmpleador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string contraseña="1234";
            string id = "1";

            Empleador nuevoEmpleador = gestor.AgregarEmpleador(nombre, apellido, email, telefono, direccion,contraseña, id);
            
            Empleador mostrarEmpleador = (Empleador)buscador.MostrarUsuario(id);
            Assert.AreEqual(mostrarEmpleador,nuevoEmpleador);            
        }
        /// <summary>
        /// Prueba que el empleador tenga nombre y sea el esperado
        /// </summary>
        [Test]
        public void TestNombre()
        {
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string NombreEsperado="Jorge";
            string NombreEmpleador=empleador.Nombre;
            Assert.AreEqual(NombreEsperado,NombreEmpleador);
        }
        /// <summary>
        /// Prueba que el empleador tenga apellidp y sea el esperado
        /// </summary>
        [Test]
        public void TestApellido()
        {
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string ApellidoEsperado="Rodriguez";
            string ApellidoEmpleador=empleador.Apellido;
            Assert.AreEqual(ApellidoEsperado,ApellidoEmpleador);
        }
        /// <summary>
        /// Prueba que el empleador tenga email y sea el esperado
        /// </summary>
        [Test]
        public void TestEmail()
        {
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string EmailEsperado="jorgerodriguez@gmail.com";
            string EmailEmpleador=empleador.Email;
            Assert.AreEqual(EmailEsperado,EmailEmpleador);
        }
        /// <summary>
        /// Prueba que el empleador tenga telefono y que este sea el esperado
        /// </summary>
        [Test]
        public void TestTelefono()
        {
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string TelefonoEsperado="099111111";
            string TelefonoEmpleador=empleador.Telefono;
            Assert.AreEqual(TelefonoEsperado,TelefonoEmpleador);
        }
        /// <summary>
        /// Prueba que el empleador tenga direccion y que esta sea la esperada
        /// </summary>
        [Test]
        public void TestDireccion()
        {
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string DireccionEsperado="18 de Julio 1111";
            string DireccionEmpleador=empleador.Direccion;
            Assert.AreEqual(DireccionEsperado,DireccionEmpleador);
        }
        /// <summary>
        /// Prueba que el empleador tenga contraseña y que este sea la esperada
        /// </summary>
        [Test]
        public void TestContraseña()
        {
            Empleador empleador=new Empleador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string ContraseñaEsperado="1234";
            string ContraseñaEmpleador=empleador.Contraseña;
            Assert.AreEqual(ContraseñaEsperado,ContraseñaEmpleador);
        }
    }
}