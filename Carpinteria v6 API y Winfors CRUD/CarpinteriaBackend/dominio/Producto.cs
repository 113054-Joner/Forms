using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaBackend.dominio
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

        public Producto(int nro, string nom, double pre)
        {
            ProductoNro = nro;
            Nombre = nom;
            Precio = pre;
            Activo = true;
        }

        public override string ToString()
        {
            return Nombre;
        }

    }
}
