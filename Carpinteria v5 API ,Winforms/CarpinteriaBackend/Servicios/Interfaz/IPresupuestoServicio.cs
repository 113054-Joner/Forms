using CarpinteriaFront.AccesoDatos;
using CarpinteriaFront.Entidades;
using System.Data;

namespace CarpinteriaFront.Servicios.Factory
{
    public interface IPresupuestoServicio
    {
        public int ProximoPresupuesto();
        public List<Producto> ListarProductos();
        public bool CrearPresupuesto(Presupuesto oPresupuesto);
    }
}