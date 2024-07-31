using Carpinteria.AccesoDatos;
using Carpinteria.Entidades;
using System.Data;

namespace Carpinteria.Servicios.Factory
{
    public interface IPresupuestoServicio
    {
         int ProximoPresupuesto();
        DataTable ListarProductos();
        bool CrearPresupuesto(Presupuesto oPresupuesto);
    }
}