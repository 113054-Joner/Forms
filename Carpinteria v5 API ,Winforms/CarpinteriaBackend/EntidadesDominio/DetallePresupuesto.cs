using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.Entidades
{
    public class DetallePresupuesto
    {
        public Producto ProductoDet { get; set; }
        public int Cantidad { get; set; }

        public DetallePresupuesto()
        {
            
        }
        public DetallePresupuesto(Producto producto, int cantidad)
        {
            this.ProductoDet = producto;
            this.Cantidad = cantidad;
        }

        public double CalcularSubtotal()
        {
            double resultado = Cantidad * ProductoDet.Precio;
            return resultado;
        }
    }
}
