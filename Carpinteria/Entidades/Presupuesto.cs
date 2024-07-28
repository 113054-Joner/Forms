using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.Entidades
{
    public class Presupuesto
    {
        
        public int PresupuestoNro { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public float CostoMO { get; set; }
        public DateTime FechaBaja { get; set; }
        List<DetallePresupuesto> listDetalles { get; set; }

        public Presupuesto(int presupuestoNro, DateTime fecha, string cliente, float costoMO, DateTime fechaBaja, List<DetallePresupuesto> listDetalles)
        {
            PresupuestoNro = presupuestoNro;
            Fecha = fecha;
            Cliente = cliente;
            CostoMO = costoMO;
            FechaBaja = fechaBaja;
            this.listDetalles = listDetalles;
        }

        public Presupuesto()
        {
            listDetalles = new List<DetallePresupuesto>();  
        }

        public void AgregarDetalle(DetallePresupuesto detalle) 
        {
            listDetalles.Add(detalle);
        }

        public void QuitarDetalle(DetallePresupuesto detalle)
        {
            listDetalles.Remove(detalle);
        }

        public float CalcularTotal() 
        { 
            float total = 0;
            foreach(DetallePresupuesto detalle in listDetalles)
            {
                total += detalle.CalcularSubtotal();
            }
            
            return total;
        }

        public void Confirmar (DetallePresupuesto detalle)
        {
            listDetalles.Remove(detalle);
        }
    }
}
