using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaBackend.dominio
{
    public class DetallePresupuesto
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public DetallePresupuesto()
        {

        }

        public DetallePresupuesto(Producto p, int cant) {
            this.Producto = p;
            this.Cantidad = cant;
        }
        public double CalcularSubTotal() {
            return Producto.Precio * Cantidad;
        }

    }
}
