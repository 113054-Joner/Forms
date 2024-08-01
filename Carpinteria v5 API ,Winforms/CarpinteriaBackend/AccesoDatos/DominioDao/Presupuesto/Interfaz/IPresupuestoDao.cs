using CarpinteriaFront.Entidades;
using System.Data;

namespace CarpinteriaFront.AccesoDatos
{
    public interface IPresupuestoDao // 1 DAO
    {
        bool Crear(Presupuesto oPresupuesto);
        int ObtenerProximoNumero();

        List<Producto> ListarProductos();
    }
}