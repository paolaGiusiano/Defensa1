using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoreBot
{
    /// <summary>
    /// Testing de la clase Buscador
    /// </summary>
    [TestFixture]
    public class TestUserStories
    {
        /// <summary>
        /// Gestor de categorias
        /// </summary>
        public GestorCategorias gestorCategorias = GestorCategorias.getInstance();
        /// <summary>
        /// Gestor de contratos
        /// </summary>
        public GestorContratos gestorContratos = GestorContratos.getInstance();
        /// <summary>
        /// Gestor de servicios
        /// </summary>
        public GestorServicios gestorServicios = GestorServicios.getInstance();
        /// <summary>
        /// Gestor de Usuarios
        /// </summary>
        public GestorUsuario gestorUsuario = GestorUsuario.getInstance();
        /// <summary>
        /// Buscador de servicios
        /// </summary>
        public BuscadorServicios buscadorServicios = BuscadorServicios.getInstance();
        /// <summary>
        /// Buscador de usuarios
        /// </summary>
        public BuscadorUsuarios buscadorUsuarios = BuscadorUsuarios.getInstance();
        /// <summary>
        /// Variable global que representa un admin del sistema
        /// </summary>
        public Administrador admin1;
        /// <summary>
        /// Variable global que representa un trabajador ya existente
        /// </summary>
        public Trabajador trabajador1 = new Trabajador();
        /// <summary>
        /// Variable global que representa un segundo trabajador ya existente
        /// </summary>
        public Trabajador trabajador2 = new Trabajador();
        /// <summary>
        /// Variable global que representa un empleador ya existente
        /// </summary>
        public Empleador empleador1 = new Empleador();
        /// <summary>
        /// Variable global que representa un servicio ya existente ofrecido por el trabajador 1
        /// </summary>
        public Servicio servicio1 = new Servicio();
        /// <summary>
        /// Variable global que representa un segundo servicio ya existente ofrecido por el trabajador 2
        /// </summary>
        public Servicio servicio2 = new Servicio();
        /// <summary>
        /// Variable global que representa un tercer servicio ya existente, ofrecido por el trabajador 1
        /// </summary>
        public Servicio servicio3 = new Servicio();
        /// <summary>
        /// Variable global que representa un cuarto servicio ya existente, ofrecido por el trabajador 2
        /// </summary>
        public Servicio servicio4 = new Servicio();
        /// /// <summary>
        /// Variable global que representa una categoria ya existente
        /// </summary>
        public Categoria categoria1 = new Categoria("Otros");
        /// <summary>
        /// Variable global que representa una segunda categoria ya existente;
        /// </summary>
        public Categoria categoria2 = new Categoria("Roperia");
        /// <summary>
        /// Variable global que representa una calificacion de un trabajador
        /// </summary>
        public Calificacion calificacion1a = new Calificacion();
        /// <summary>
        /// Variable global que representa una segunda calificacion de un trabajador
        /// </summary>
        public Calificacion calificacion1b = new Calificacion();
        /// <summary>
        /// Variable global que representa una calificacion de un segundo trabajador
        /// </summary>
        public Calificacion calificacion2a = new Calificacion();
        /// <summary>
        /// Variable global que representa una segunda calificacion de un segundo trabajador
        /// </summary>
        public Calificacion calificacion2b = new Calificacion();
        /// <summary>
        /// Variable global que representa una calificacion de un empleador
        /// </summary>
        public Calificacion calificacion3 = new Calificacion();
        /// <summary>
        /// Setup de data de testing para los casos de prueba
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            admin1 = gestorUsuario.AgregarAdmin("Andres", "Arancio", "XXXX@ucu.edu.uy", "12345678", "0");
            trabajador1 = gestorUsuario.AgregarTrabajador("Roberto","Torres","1111@ucu.edu.uy","099111111","Montevideo, Av. 18 de Julio 1824","12345","8");
            servicio1 = gestorServicios.CrearServicio("Conduccion",Payment.Debito,Costo.Costo_por_hora,trabajador1,categoria1, 100);
            servicio3 = gestorServicios.CrearServicio("Hago Remeras",Payment.Debito,Costo.Costo_total_servicio,trabajador1,categoria2, 100);
            trabajador2 = gestorUsuario.AgregarTrabajador("Roberta","Muralla","0000@ucu.edu.uy","099000000","Montevideo, Bv. Gral. Artigas 1031","12345","1");
            servicio2 = gestorServicios.CrearServicio("Confexion de sombreros",Payment.Debito,Costo.Costo_por_hora,trabajador2,categoria2, 100);
            servicio4 = gestorServicios.CrearServicio("Programacion",Payment.Efectivo,Costo.Costo_por_hora,trabajador2,categoria1, 100);
            empleador1 = gestorUsuario.AgregarEmpleador("Alejadro","Grande","4444@ucu.edu.uy","099444444","Montevideo, Bv. Gral Artigas 1139","12345","5");
            calificacion1a.AgregarCalificacion(5);
            calificacion1b.AgregarCalificacion(4);
            calificacion2b.AgregarCalificacion(2);
            calificacion2a.AgregarCalificacion(1);
            calificacion3.AgregarCalificacion(3);
            trabajador1.AgregarCalificacion(calificacion1a);
            trabajador1.AgregarCalificacion(calificacion1b);
            trabajador2.AgregarCalificacion(calificacion2b);
            trabajador2.AgregarCalificacion(calificacion2a);
            empleador1.AgregarCalificacion(calificacion3);
        }
        /// <summary>
        /// Como administrador quiero poder indicar categorias sobre las cuales se realizaran las ofertas de servicios 
        /// para que de esa forma los trabajadores puedan clasificarlos
        /// IN: Nombre de una nueva categoria
        /// OUT: La categoria es creada
        /// </summary>
        [Test]   
        public void US1()
        {   
            string nuevoNombreCategoria = "Jardineria";
            Categoria nuevaCategoria = gestorCategorias.CrearCategoria(nuevoNombreCategoria, admin1);
            Assert.AreEqual(nuevoNombreCategoria,nuevoNombreCategoria);
        }
        /// <summary>
        /// Como administrador quiero poder dar de baja ofertas de servicios, avisando al trabajador 
        /// para que de esa forma pueda evitar ofertas inadecuadas
        /// IN: Servicio con una oferta inadecuada
        /// OUT: Ese servicio ya no esta en la lista, la informacion de contacto del trabajador de ese servicio
        /// </summary>
        [Test]
        public void US2()
        {
            Servicio servicioInadecuado = gestorServicios.CrearServicio("Inadecuado",Payment.Transferencia,Costo.Costo_por_hora,trabajador1,categoria1, 100);
            gestorServicios.RemoverServicio(servicioInadecuado, admin1, "Servicio inadecuado");
            Assert.IsNull(buscadorServicios.MostrarServicio("Inadecuado",categoria1,Payment.Transferencia,Costo.Costo_por_hora,trabajador1.Email));
        }
        /// <summary>
        /// Como trabajador quiero poder registrarme a la plataforma, indicando mis datos personales e informacion de contacto
        /// para que de esta forma pueda proveer mi informacion de contacto a empleadores
        /// IN: Datos del trabajador
        /// OUT: Trabajador existe en la lista de trabajadores del gestor con sus datos personales
        /// </summary>
        [Test]
        public void US3()
        {
            string nuevoNombre = "Ricardo";
            string nuevoApellido = "Montiel";
            string nuevoEmail = "NNNN@ucu.edu.uy";
            string nuevoTelefono = "099222222";
            string nuevoDireccion = "Montevideo, 19 de Agosto 2222";
            string nuevoPassword = "12345";
            string id = "3";
            
            Trabajador nuevoTrabajador = gestorUsuario.AgregarTrabajador(nuevoNombre,nuevoApellido,nuevoEmail,nuevoTelefono,nuevoDireccion,nuevoPassword,id);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(id).Nombre,nuevoTrabajador.Nombre);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(id).Apellido,nuevoTrabajador.Apellido);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(id).Telefono,nuevoTrabajador.Telefono);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(id).Direccion,nuevoTrabajador.Direccion);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(id).Email,nuevoTrabajador.Email);
        }
        /// <summary>
        /// Como trabajador quiero poder hacer ofertas de servicios, mi oferta indicara en que categoria publicar, tendra una descripcion
        /// del servicio ofertado y un precio
        /// para que de esa forma mis ofertas sean ofrecidas a quienes quieren contratar mis servicios
        /// IN: Datos del servicio a ofrecer
        /// OUT: Una oferta de servicios con esos datos se puede encontrar en la lista de servicios
        /// </summary>
        [Test]
        public void US4()
        {
            string descripcionServicio = "Construyo casas";
            Payment tipoPago = Payment.Debito;
            Costo costo = Costo.Costo_total_servicio;

            Servicio nuevoServicio = gestorServicios.CrearServicio(descripcionServicio,tipoPago,costo,trabajador1,categoria1, 100);
            Assert.AreEqual(buscadorServicios.MostrarServicio(descripcionServicio,categoria1,tipoPago,costo,trabajador1.Email).DescripcionServicio,descripcionServicio);
            Assert.AreEqual(buscadorServicios.MostrarServicio(descripcionServicio,categoria1,tipoPago,costo,trabajador1.Email).Pago,tipoPago);
            Assert.AreEqual(buscadorServicios.MostrarServicio(descripcionServicio,categoria1,tipoPago,costo,trabajador1.Email).Cost,costo);
        }
        /// <summary>
        /// Como empleador quiero registrarme en la plataforma, indicando mis datos personales e informacion de contacto
        /// para que de esa forma pueda proveer informacion de contacto a los trabajadores que quiero contratar
        /// IN: Datos del empleador
        /// OUT: Empleador existe en la lista de empleadores del gestor con sus datos personales
        /// </summary>
        [Test]
        public void US5()
        {
            string nuevoNombre = "Stefans";
            string nuevoApellido = "Pereyra";
            string nuevoEmail = "3333@ucu.edu.uy";
            string nuevoTelefono = "099333333";
            string nuevoDireccion = "Montevideo, 20 de Setiembre 3333";
            string nuevoPassword = "12345";
            string id = "4";
            
            Empleador nuevoEmpleador = gestorUsuario.AgregarEmpleador(nuevoNombre,nuevoApellido,nuevoEmail,nuevoTelefono,nuevoDireccion,nuevoPassword,id);

            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(nuevoEmpleador.ID).Nombre,nuevoNombre);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(nuevoEmpleador.ID).Apellido,nuevoApellido);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(nuevoEmpleador.ID).Email,nuevoEmail);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(nuevoEmpleador.ID).Telefono,nuevoTelefono);
            Assert.AreEqual(buscadorUsuarios.MostrarUsuario(nuevoEmpleador.ID).Direccion,nuevoDireccion);
        }
        /// <summary>
        /// Como empleador quiero poder buscar ofertas de trabajo, opcionalmente filtrando por categoria
        /// para de esta forma poder contratar un servicio
        /// IN: Categoria por la cual buscar servicios
        /// OUT: Lista de servicios que coincidan con esa categoria
        /// </summary>
        [Test]
        public void US6()
        {
            string keyword = "Roperia";
            List<Servicio> resultadoBusqueda = buscadorServicios.BuscarOrdenCalificacion(keyword, empleador1);
            foreach(Servicio servicio in resultadoBusqueda)
            {
                Assert.AreEqual(servicio.Categoria.Nombre,keyword);
            }
        }
        /// <summary>
        /// Como empleador quiero poder ver el resultado de las busquedas de oferta de trabajo ordenado de forma ascendiente de distancia
        /// del trabajador a mi ubicacion, es decir, las mas cercanas primero
        /// para que de esta forma pueda poder contratar un servicio
        /// IN: Categoria por la que se busca el servicio
        /// OUT: Lista de servicios que coinciden con la categoria ordenada por direccion
        /// NOTA: Al tener una clase de API que calcule la distancia entre dos puntos, se cambiara este metodo para coincidir con el
        /// requerimiento original
        /// </summary>
        [Test]
        public void US7()
        {
            string keyword = "Roperia";
            List<Servicio> resultadoBusqueda = buscadorServicios.BuscarOrdenDistancia(keyword, empleador1);
            List<Servicio> auxiliar = new List<Servicio>();
            
            foreach(Servicio auxiliarServicio1 in gestorServicios.TodoServicios)
            {
                if(auxiliarServicio1.Categoria.Nombre == keyword)
                {
                    auxiliar.Add(auxiliarServicio1);
                    double result = buscadorServicios.calculator.CalculateDistance(auxiliarServicio1.Trabajador.Direccion,empleador1.Direccion).Distance;
                }
            }
            auxiliar.Sort((a, b) => buscadorServicios.calculator.CalculateDistance(a.Trabajador.Direccion,empleador1.Direccion).Distance.CompareTo(buscadorServicios.calculator.CalculateDistance(b.Trabajador.Direccion,empleador1.Direccion).Distance));

            Assert.AreEqual(auxiliar,resultadoBusqueda);
        }
        /// <summary>
        /// Como empleador quiero poder ver el resultado de las busquedas de oferta de trabajo ordenado de forma descendiente por
        /// reputacion, es decir, las de mejor reputacion primero
        /// para que de esa forma pueda contratar un servicio
        /// IN: Categoria por la cual buscar servicios
        /// OUT: Lista de servicios que coinciden con la categoria ordenados por calificacion del trabajador
        /// </summary>
        [Test]
        public void US8()
        {
            string keyword = "Roperia";
            List<Servicio> resultadoBusqueda = buscadorServicios.BuscarOrdenCalificacion(keyword, empleador1);
            List<Servicio> auxiliar = new List<Servicio>();
            List<Servicio> result = new List<Servicio>();
            foreach(Servicio auxiliarServicio1 in gestorServicios.TodoServicios)
            {
                if(auxiliarServicio1.Categoria.Nombre == keyword)
                {
                    auxiliar.Add(auxiliarServicio1);
                }
            }

            result = auxiliar.OrderByDescending(Servicio=>Servicio.Trabajador.CalificacionTotal).ToList();

            Assert.AreEqual(result, resultadoBusqueda);
        }
        /// <summary>
        /// Como empleador quiero poder contactar a un trabajador
        /// para que de esta forma pueda contratar una oferta de servicios determinada
        /// IN: ID de un trabajdor
        /// OUT: Informacion de contacto del trabajador
        /// </summary>
        [Test]
        public void US9()
        {
            string searchID = trabajador1.ID;
            string expectedEmail = trabajador1.Email;
            string expectedTelefono = trabajador1.Telefono;

            Usuario resultadoBusqueda = buscadorUsuarios.MostrarUsuario(searchID);
            Assert.AreEqual(expectedEmail, resultadoBusqueda.Email);
            Assert.AreEqual(expectedTelefono, resultadoBusqueda.Telefono);
        }
        /// <summary>
        /// Como trabajador quiero poder calificar a un empleador, el empleador me tiene que calificar a mi tambien, si no me califica
        /// en un mes, la calificacion sera neutral
        /// para que de esta forma pueda definir la reputacion de mi empleador
        /// IN: Calificacion a dar a un contrato en particular
        /// OUT: La calificacion se agrega a la lista de calificaciones del empleador y su calificacion total se actualiza
        /// NOTA: Testing de la calificacion neutral no fue posible sin alterar el clock de la computadora.
        /// Como posible mejora se podria hacer que el DateTime de Calificacion se reciba como variable
        /// </summary>
        [Test]
        public void US10()
        {
            int markCalificacion = 5;
            Contrato contratoCompleto = gestorContratos.CrearContrato(empleador1,servicio1);
            Calificacion nuevaCalificacion = new Calificacion();
            nuevaCalificacion.AgregarCalificacion(markCalificacion);
            empleador1.AgregarCalificacion(nuevaCalificacion);
            contratoCompleto.CalificarContratoEmpleador(empleador1,nuevaCalificacion);
            Assert.AreEqual(4,empleador1.CalificacionTotal);
        }
        /// <summary>
        /// Como empleador quiero poder calificar a un trabajador, el trabajador me tiene que calificar a mi tambien, si no me califica
        /// en un mes, la calificacion sera neutral
        /// para que de esta forma pueda definir la reputacion de mi trabajador
        /// IN: Calificacion a dar a un contrato en particular
        /// OUT: La calificacion se agrega a la lista de calificaciones del trabajador y su calificacion total se actualiza
        /// </summary>
        [Test]
        public void US11()
        {
            int markCalificacion = 0;
            Contrato contratoCompleto = gestorContratos.CrearContrato(empleador1,servicio1);
            Calificacion nuevaCalificacion = new Calificacion();
            nuevaCalificacion.AgregarCalificacion(markCalificacion);
            trabajador1.AgregarCalificacion(nuevaCalificacion);
            contratoCompleto.CalificarContratoTrabajador(trabajador1,nuevaCalificacion);
            Assert.AreEqual(3,trabajador1.CalificacionTotal);
        }
        /// <summary>
        /// Como trabajador quiero poder saber la calificacion de un empleador que me contacte
        /// para que desta forma pueda decidir sobre su solicitud de contratacion
        /// IN: ID del empleador a buscar
        /// OUT: Calificacion total del empleador
        /// </summary>
        [Test]
        public void US12()
        {
            string searchID = empleador1.ID;
            Usuario searchEmpleador = buscadorUsuarios.MostrarUsuario(searchID);
            Assert.AreEqual(empleador1.CalificacionTotal,searchEmpleador.CalificacionTotal);
        }
    }
}