using CarpinteriaFront.AccesoDatos;
using CarpinteriaFront.Entidades;
using CarpinteriaFront.Servicios.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.Servicios.Implementacion
{
    public class PresupuestoService : IPresupuestoServicio
    {
        private IPresupuestoDao dao; // 4 DAO

        public PresupuestoService() // 4 FACT Constructor que toma por Parametro  clase Abstrac
        {
            AbstractDaofactory factory = new DaoFactory();
            dao = factory.CrearPresupuestoDao(); // 5 FACT
        }

        public bool CrearPresupuesto(Presupuesto oPresupuesto)
        {
            return dao.Crear(oPresupuesto);
        }

        public List<Producto> ListarProductos()
        {
            return dao.ListarProductos();
        }

        public int ProximoPresupuesto()
        {
            return dao.ObtenerProximoNumero();
        }
    }
}
