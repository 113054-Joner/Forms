using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.Entidades
{
    public class DetallePresupuesto
    {
        public Producto producto { get; set; }
        public int cantidad { get; set; }

        public DetallePresupuesto(Producto producto, int cantidad)
        {
            this.producto = producto;
            this.cantidad = cantidad;
        }

        public float CalcularSubtotal()
        {
            float resultado = cantidad * producto.Precio;
            return resultado;
        }
    }
}
