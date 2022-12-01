using System;
using System.Collections.Generic;

namespace CoreBot
{
    /// <summary>
    /// Clase servicio la cual contiene detalles acerca de como será el servicio prestado y algunos metodos para la modificación del mismo
    /// </summary>
    public class Servicio
    {
        /// <summary>
        /// Id del servicio
        /// </summary>
        public int Id {get; internal set;}
        /// <summary>
        /// Categoria del servicio
        /// </summary>
        public Categoria Categoria{get;set;}
        /// <summary>
        /// Método de pago del servicio, el cual puede ser credito o debito por ejemplo
        /// </summary>
        public Payment Pago{get;set;}
        /// <summary>
        /// Tipo de costo del servicio, el cual puede ser de momento por hora o costo total del servicio
        /// </summary>
        public Costo Cost{get;set;}
        /// <summary>
        /// Descripción que contiene el servicio
        /// </summary>
        public string DescripcionServicio{get;set;}
        /// <summary>
        /// Costo del servicio
        /// </summary>
        
        public decimal Costo{get;set;}
        /// <summary>
        /// Constructor vacio para testing y genericos
        /// </summary>
        public Servicio()
        {

        }
        /// <summary>
        /// Constructor de servicio con sus respectivos parametros
        /// </summary>
        /// <param name="id">Id unica del servicio</param>
        /// <param name="DescripcionServicio">Descripcion del servicio entrada por el usuario</param>
        /// <param name="pago">Tipo de pago para el servicio</param>
        /// <param name="cost">Forma de calcular el costo del servicio (por hora o a termino)</param>
        /// <param name="trabajador">Trabajador a cargo del servicio</param>
        /// <param name="categoria">Categoria del servicio</param>
        /// <param name="costo">Valor monetario a pagar por el servicio</param>
        public Servicio(int id, string DescripcionServicio, Payment pago, Costo cost,Trabajador trabajador, Categoria categoria, decimal costo)
        {       
            this.Id = id;
            this.DescripcionServicio=DescripcionServicio;
            this.Categoria = categoria;
            this.Pago = pago;
            this.Cost = cost;
            this.Trabajador=trabajador;
            this.Costo = costo;
        }

        /// <summary>
        /// Trabajador el cual crea el servicio
        /// </summary>
        public Trabajador Trabajador{get ; set ;}
        /// <summary>
        /// Metodo que sirve para actualizar el servicio
        /// </summary>
        public virtual void ActualizarServicio(Categoria NuevaCategoria, Payment NuevoPago, Costo NewCost)
        {       
            this.Categoria=NuevaCategoria;
            this.Pago=NuevoPago;
            this.Cost=NewCost;
        }

        /// <summary>
        /// Metodo exclusivo para modificar una categoría
        /// </summary>
        /// <param name="categoria"></param>
        public virtual void ModificarCategoria(Categoria categoria)
        {
            this.Categoria=categoria;
        }
        
        /// <summary>
        /// Metodo para agregar un servicio a la lista de servicios del trabajador correspondiente
        /// </summary>
        /// <param name="servicio"></param>
        /// <param name="trabajador"></param>
        public virtual void AgregarServicio(Servicio servicio, Trabajador trabajador)
        {
            trabajador.Servicios.Add(servicio);
        }



        /// <summary>
        /// Override de la funcion to string para evitar que muestre el nombre de clase en la visualización
        /// </summary>
        /// <returns>Un string conteniendo todos los parametros del servicio</returns>
        public override string ToString()
        {
           return String.Format("ID {5}; Descripcion: {0}; Categoria: {1}; Tipo de Pago: {2};Costo: {3}; Formato del Costo: {4}", DescripcionServicio, Categoria.Nombre, Pago, Costo, Cost, Id);
        }
    }
}