using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Clase de testing que prueba la creacion, modificacion y borrado de un usuario trabajador
    /// </summary>
    [TestFixture]
    public class TestTrabajador
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
        /// Metodo de setup que define un trabajador a buscar
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            string nuevoNombre = "Stefans";
            string nuevoApellido = "Pereyra";
            string nuevoEmail = "3333@ucu.edu.uy";
            string nuevoTelefono = "099333333";
            string nuevoDireccion = "Montevideo, 20 de Setiembre 3333";
            string nuevoPassword = "12345";
            string id = "6";
            gestor.AgregarTrabajador(nuevoNombre,nuevoApellido,nuevoEmail,nuevoTelefono,nuevoDireccion,nuevoPassword,id);
        }

        /// <summary>
        /// Prueba que un Trabajador es creado con dados parametros
        /// </summary>
        [Test]
        public void TestCrearTrabajador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string contraseña="1234";
            string id = "0";

            Trabajador nuevoTrabajador = gestor.AgregarTrabajador(nombre, apellido, email, telefono, direccion,contraseña,id);

            Assert.AreEqual(nuevoTrabajador.Nombre, nombre);
            Assert.AreEqual(nuevoTrabajador.Apellido, apellido);
            Assert.AreEqual(nuevoTrabajador.Email, email);
            Assert.AreEqual(nuevoTrabajador.Telefono, telefono);
            Assert.AreEqual(nuevoTrabajador.Direccion, direccion);

            Assert.AreEqual(nuevoTrabajador.Contraseña, contraseña);

        }
        /// <summary>
        /// Prueba que si se modifica un Trabajador para que tenga un valor dado, el valor es el esperado
        /// </summary>
        [Test]
        public void TestModificarTrabajador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string id = "0";
            string contraseña="1234";

            Trabajador nuevoTrabajador = gestor.AgregarTrabajador(nombre, apellido, email, telefono, direccion,contraseña,id);
            nuevoTrabajador.ModificarUsuario("Robert", nuevoTrabajador.Apellido, nuevoTrabajador.Email, nuevoTrabajador.Telefono, nuevoTrabajador.Direccion,nuevoTrabajador.Contraseña);

            Assert.AreEqual(nuevoTrabajador.Nombre, "Robert");
        }
        /// <summary>
        /// Prueba que si se borra un Trabajador, sus valores son null
        /// </summary>
        [Test]
        public void TestBorrarTrabajador()
        {
            string nombre = "Roberto";
            string apellido = "Torres";
            string email = "1111@ucu.edu.com";
            string telefono = "099111111";
            string direccion = "18 de Julio 1111";
            string id = "0";
            string contraseña="1234";

            Trabajador nuevoTrabajador = gestor.AgregarTrabajador(nombre, apellido, email, telefono, direccion,contraseña, id);
            nuevoTrabajador.BorrarUsuario();
            
            Assert.IsNull(nuevoTrabajador.Nombre);
            Assert.IsNull(nuevoTrabajador.Apellido);
            Assert.IsNull(nuevoTrabajador.Calificaciones);
            Assert.IsNull(nuevoTrabajador.Email);
            Assert.IsNull(nuevoTrabajador.Direccion);

            Assert.IsNull(nuevoTrabajador.Contraseña);
            Assert.AreEqual(nuevoTrabajador.CalificacionTotal,0);
        }
        /// <summary>
        /// Prueba que si se solicita mostrar la informacion de un Trabajador en base a su ID, el sistema devuelve la misma
        /// </summary>
        [Test]
        public void MostrarTrabajador()
        {
            string nombre = "Stefans";
            string apellido = "Pereyra";
            string email = "3333@ucu.edu.uy";
            string telefono = "099333333";
            string direccion = "Montevideo, 20 de Setiembre 3333";
            string password = "12345";
            string id = "6";
            Trabajador mostrarTrabajador = buscador.MostrarUsuario(id) as Trabajador;
            Assert.AreEqual(mostrarTrabajador.Nombre,nombre);
            Assert.AreEqual(mostrarTrabajador.Apellido,apellido);
            Assert.AreEqual(mostrarTrabajador.Email,email);
            Assert.AreEqual(mostrarTrabajador.Telefono,telefono);
            Assert.AreEqual(mostrarTrabajador.Direccion,direccion);
            Assert.AreEqual(mostrarTrabajador.Contraseña,password);           
        }
        /// <summary>
        /// Prueba que el trabajador tenga nombre y sea el esperado
        /// </summary>
        [Test]
        public void TestNombre()
        {
            Trabajador trabajador=new Trabajador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234", "2");
            string NombreEsperado="Jorge";
            string NombreTrabajador=trabajador.Nombre;
            Assert.AreEqual(NombreEsperado,NombreTrabajador);
        }
        /// <summary>
        /// Prueba que el trabajador tenga apellido y sea el esperado
        /// </summary>
        [Test]
        public void TestApellido()
        {
            Trabajador trabajador=new Trabajador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","2");
            string ApellidoEsperado="Rodriguez";
            string ApellidoTrabajador=trabajador.Apellido;
            Assert.AreEqual(ApellidoEsperado,ApellidoTrabajador);
        }
        /// <summary>
        /// Prueba que el trabajador tenga email y sea el esperado
        /// </summary>
        [Test]
        public void TestEmail()
        {
            Trabajador trabajador=new Trabajador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","2");
            string EmailEsperado="jorgerodriguez@gmail.com";
            string EmailTrabajador=trabajador.Email;
            Assert.AreEqual(EmailEsperado,EmailTrabajador);
        }
        /// <summary>
        /// Prueba que el trabajador tenga telefono y que este sea el esperado
        /// </summary>
        [Test]
        public void TestTelefono()
        {
            Trabajador trabajador=new Trabajador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","2");
            string TelefonoEsperado="099111111";
            string TelefonoTrabajador=trabajador.Telefono;
            Assert.AreEqual(TelefonoEsperado,TelefonoTrabajador);
        }
        /// <summary>
        /// Prueba que el trabajador tenga direccion y que esta sea la esperada
        /// </summary>
        [Test]
        public void TestDireccion()
        {
            Trabajador trabajador=new Trabajador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","2");
            string DireccionEsperado="18 de Julio 1111";
            string DireccionTrabajador=trabajador.Direccion;
            Assert.AreEqual(DireccionEsperado,DireccionTrabajador);
        }
        /// <summary>
        /// Prueba que el trabajador tenga contraseña y que esta sea la esperada
        /// </summary>
        [Test]
        public void TestContraseña()
        {
            Trabajador trabajador=new Trabajador("Jorge","Rodriguez","jorgerodriguez@gmail.com","099111111","18 de Julio 1111","1234","2");
            string ContraseñaEsperado="1234";
            string ContraseñaTrabajador=trabajador.Contraseña;
            Assert.AreEqual(ContraseñaEsperado,ContraseñaTrabajador);
        }
    }
}