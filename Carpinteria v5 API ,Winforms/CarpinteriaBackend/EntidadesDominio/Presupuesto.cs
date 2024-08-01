using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.Entidades
{
    public class Presupuesto
    {
        
        public int PresupuestoNro { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }
        public DateTime FechaBaja { get; set; }
        public List<DetallePresupuesto> Detalles { get; set; }

        //public Presupuesto(int presupuestoNro, DateTime fecha, string cliente, double total, DateTime fechaBaja, List<DetallePresupuesto> listDetalles)
        //{
        //    PresupuestoNro = presupuestoNro;
        //    Fecha = fecha;
        //    Cliente = cliente;
        //    Total = total;
        //    FechaBaja = fechaBaja;
        //    this.listDetalles = listDetalles;
        //}

        public Presupuesto()
        {
            Detalles = new List<DetallePresupuesto>();  
        }

        public void AgregarDetalle(DetallePresupuesto detalle) 
        {
            Detalles.Add(detalle);
        }

        public void QuitarDetalle(int indice)
        {
            Detalles.RemoveAt(indice);
        }

        public double CalcularTotal() 
        {
            double total = 0;
            foreach(DetallePresupuesto detalle in Detalles)
            {
                total += detalle.CalcularSubtotal();
            }
            
            return total;
        }

    }
}
