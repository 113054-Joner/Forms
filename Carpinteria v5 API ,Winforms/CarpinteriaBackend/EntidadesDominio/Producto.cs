using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.Entidades
{
    public class Producto
    {
        public int ProductoNro { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public bool Activo { get; set; }

        public Producto()
        {
            this.ProductoNro = 0;
            this.Nombre = string.Empty;
            this.Precio = 0;
            this.Activo = false;
        }

        public Producto(int productoNro,string nombre,double precio)
        {
            ProductoNro = productoNro;
            Nombre = nombre;
            Precio = precio;
            Activo = true;
        }

        //public override string ToString()
        //{
        //    return "Producto Nro: " + ProductoNro 
        //        + "\nNombre: "+ Nombre
        //        +"\nPrecio: "+ Precio;
        //}

        public override string ToString()
        {
            return Nombre;
        }
    }
}
