using Carpinteria.Entidades;
using System.Data;

namespace Carpinteria.AccesoDatos
{
    public interface IPresupuestoDao // 1 DAO
    {
        bool Crear(Presupuesto oPresupuesto);
        int ObtenerProximoNumero();

        DataTable ListarProductos();
    }
}