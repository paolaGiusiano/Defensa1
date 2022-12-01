using System;
using System.Collections.Generic;

namespace CoreBot
{
    
    /// <summary>
    /// Clase contrato, la cual se relaciona con servicio, empleador y trabajador
    /// </summary>
    public class Contrato
    {
        /// <summary>
        /// ID unico del contrato
        /// </summary>
        public string ID {get;set;}
        /// <summary>
        /// Usuario trabajador del contrato
        /// </summary>
        public Trabajador Trabajador{get;set;}
        /// <summary>
        /// Usuario empleador del contrato
        /// </summary>
        public Empleador Empleador{get;set;}
        /// <summary>
        /// Servicio a contratar
        /// </summary>
        public Servicio Servicio{get;set;}
        /// <summary>
        /// Costo total del contrato
        /// </summary>
        public decimal CostoTotal{get;set;}
        /// <summary>
        /// Fecha en el que el contrato se firmo
        /// </summary>
        public DateTime FechaFirma{get;set;}
        /// <summary>
        /// Fecha de cierre del contrato
        /// </summary>
        public DateTime FechaCierre{get;set;}
        /// <summary>
        /// Calificacion que el Trabajador dio al Empleador
        /// </summary>
        public Calificacion CalificacionEmpleador{get;set;}
        /// <summary>
        /// Calificacion que el Empleador dio al Trabajador
        /// </summary>
        public Calificacion CalificacionTrabajador{get;set;}
       
        /// <summary>
        /// Constructor vacio de Contrato para genericos y testing
        /// </summary>
        public Contrato()
        {

        }
        
        /// <summary>
        /// Constructor de servicio para un contrato de pago a termino
        /// </summary>
        /// <param name="empleador">Empleador del servicio</param>
        /// <param name="servicio">Servicio a contratar</param>
        /// <returns></returns>
        public Contrato(Empleador empleador, Servicio servicio)
        {
            this.FechaCierre = DateTime.Now;
            this.FechaCierre = FechaFirma.AddDays(30);
            this.Empleador = empleador;
            this.Trabajador = servicio.Trabajador;
            this.CostoTotal = servicio.Costo;
            this.Servicio = servicio;
            this.ID = DateTime.Now.ToString();
        }
        /// <summary>
        /// Constructor de servicio para costo por hora
        /// </summary>
        /// <param name="empleador">Empleador a contratar el servicio</param>
        /// <param name="servicio">Servicio a contratar</param>
        /// <param name="horas">Cantidad de horas a contratar</param>
        public Contrato(Empleador empleador, Servicio servicio, decimal horas)
        {
            this.FechaFirma = DateTime.Now;
            this.FechaCierre = FechaCierre.AddDays(30);
            this.Empleador = empleador;
            this.Trabajador = servicio.Trabajador;
            this.CostoTotal = this.CalcularCosto(servicio,horas);
            this.Servicio = servicio;
            this.ID = DateTime.Now.ToString();
        }
        /// <summary>
        /// Se separa la funcionalidad de calificar un usuario en calificar un empleador para cumplir con OCP y reducir acoplamiento
        /// </summary>
        /// <param name="empleador">El empleador a ser calificado</param>
        /// <param name="calificacion">La calificacion a ser agregada</param>
        public void CalificarContratoEmpleador(Empleador empleador, Calificacion calificacion)
        {
            this.CalificacionEmpleador = calificacion;
        }
        /// <summary>
        /// Se separa la funcionalidad de calificar un usuario en calificar un trabajador para cumplir con OCP y reducir acoplamiento
        /// </summary>
        /// <param name="trabajador">El trabajador a ser calificado</param>
        /// <param name="calificacion">La calificacion a ser agregada</param>
        public void CalificarContratoTrabajador(Trabajador trabajador, Calificacion calificacion)
        {
            this.CalificacionTrabajador = calificacion;
        }
        
        /// <summary>
        /// Calculadora de costo total para servicio con costo por hora
        /// </summary>
        /// <param name="servicio">Servicio siendo contratado</param>
        /// <param name="horas">Cantidad de horas a contratar</param>
        /// <returns></returns>
        public decimal CalcularCosto(Servicio servicio, decimal horas)
        {
            decimal resultado = servicio.Costo*horas;
            return resultado;
        }
    }
}