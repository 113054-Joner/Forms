using CarpinteriaFront.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.AccesoDatos
{
    class PresupuestoDao : IPresupuestoDao 
    {
        public bool Crear(Presupuesto oPresupuesto)
        {

            return HelperDao.ObtenerInstancia().CrearPresupuesto(oPresupuesto);
        }

        public List<Producto> ListarProductos()//2 SINGLETON
        {
            List<Producto> listProductos = new List<Producto>();
            DataTable tabla = HelperDao.ObtenerInstancia().ConsultaSql("SP_CONSULTAR_PRODUCTOS", null);// SINGLETON

            foreach(DataRow row in tabla.Rows)
            {
                int nro = int.Parse(row["id_producto"].ToString());
                string nombre = row["n_producto"].ToString();
                double precio = double.Parse(row["precio"].ToString());
                bool activo = row["activo"].ToString().Equals("S");

                Producto aux = new Producto(nro, nombre, precio);
                aux.Activo = activo;
                listProductos.Add(aux);
            }

            return listProductos;
        }

        public int ObtenerProximoNumero() // 2 DAO
        {
            return HelperDao.ObtenerInstancia().ProximoId("SP_ULT_ID", "@nro");
            
        }
    }
}
