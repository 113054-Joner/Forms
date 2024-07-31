using Carpinteria.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.AccesoDatos
{
    class PresupuestoDao : IPresupuestoDao 
    {
        public bool Crear(Presupuesto oPresupuesto)
        {

            return HelperDao.ObtenerInstancia().CrearPresupuesto(oPresupuesto);
        }

        public DataTable ListarProductos()//2 SINGLETON
        {

            return HelperDao.ObtenerInstancia().ConsultaSql("SP_CONSULTAR_PRODUCTOS",null);// SINGLETON
        }

        public int ObtenerProximoNumero() // 2 DAO
        {
            return HelperDao.ObtenerInstancia().ProximoId("SP_ULT_ID", "@nro");
            
        }
    }
}
