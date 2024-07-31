using Carpinteria.AccesoDatos;
using Carpinteria.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.Servicios
{
    class GestorPresupuesto
    {
        private IPresupuestoDao dao; // 4 DAO

        public GestorPresupuesto(AbstractDaofactory factory) // 4 FACT Constructor que toma por Parametro  clase Abstrac
        {
            dao = factory.CrearPresupuestoDao(); // 5 FACT
        }
        public int ProximoPresupuesto()
        {
           return dao.ObtenerProximoNumero(); //5 DAO
        }

        public DataTable ListarProductos() 
        {
            return dao.ListarProductos();
        }

        public bool CrearPresupuesto(Presupuesto oPresupuesto)
        {
            return dao.Crear(oPresupuesto);
        }
    }
}
