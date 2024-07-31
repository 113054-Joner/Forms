using Carpinteria.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Carpinteria.AccesoDatos
{
    public class HelperDao
    {
        /*
         PARA HACER LA FORMA DE BOTTA 
        Contiene metodo de conexion, CONSULTA DB y PROXIMO ID , NO USA LISTA DE PARAMETRO se complica con INSERTS

         
        private static HelperDao instancia;

        private string cadenaConexion;


        private HelperDao()
        {
            //cadenaConexion = "Data Source=localhost;Initial Catalog=Carpinteria;Integrated Security=True;"; 
            cadenaConexion = Properties.Resources.strConexion;
        }
        public static HelperDao ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new HelperDao();
            }
            return instancia;
        }

        public DataTable ConsultaSql(string nombreSp)//1 SINGLETON
        {
            SqlConnection connection = new SqlConnection(cadenaConexion);//1
            SqlCommand comando = new SqlCommand(cadenaConexion, connection);//2
            DataTable tabla = new DataTable();

            try
            {
                connection.Open();
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = nombreSp;
                tabla.Load(comando.ExecuteReader());
                connection.Close();

                return tabla;
            }
            catch (Exception ex)
            {

                throw(ex);
            }
            finally
            {
                if(connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        public int ProximoId(string nombreSp,string nombreParametro)
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);//1
            SqlCommand command = new SqlCommand();//2
            //Sql Parametros
            SqlParameter param = new SqlParameter(nombreParametro, SqlDbType.Int);
            
            try
            {
                conexion.Open();
                command.Connection = conexion;//2
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = nombreSp;//Sentencias

                //Sql Parametros
                param.Direction = ParameterDirection.Output;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();//Reader Select -> DataTable , NonQuery Update,Inser,Delete 


                return (int)param.Value;
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }
        }
        */


        private static HelperDao instancia;
        private SqlConnection conexion;

        private HelperDao()
        {
            conexion = new SqlConnection(Properties.Resources.strConexion);
        }

        public static HelperDao ObtenerInstancia()
        {
            if (instancia == null)
                instancia = new HelperDao();
            return instancia;
        }

        public DataTable ConsultaSql(string spNombre, List<Parametro> listaParam)
        {
            SqlCommand cmd = new SqlCommand(spNombre, conexion);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = conexion;
            //cmd.CommandText = spNombre; 
            DataTable tabla = new DataTable();
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                if (listaParam != null)
                {
                    foreach (Parametro oParametro in listaParam)
                    {
                        cmd.Parameters.AddWithValue(oParametro.Clave, oParametro.Valor);
                    }
                }
                tabla.Load(cmd.ExecuteReader());


                return tabla;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }

            

           
        }

        public int ProximoId(string spNombre, string pOutNombre)
        {
            SqlCommand command = new SqlCommand(spNombre, conexion);
            SqlParameter pOut = new SqlParameter();

            try
            {
                conexion.Open();

                command.CommandType = CommandType.StoredProcedure;

                pOut.ParameterName = pOutNombre;
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                command.Parameters.Add(pOut);
                command.ExecuteNonQuery();

                return (int)pOut.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            { 
                if(conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }

            
        }

        public bool CrearPresupuesto(Presupuesto oPresupuesto)
        {
            bool estado = true;
            SqlTransaction transaction = null;
            string nomSp = "SP_INSERTAR_PRESUPUESTO";//INSERTAMOS MAESTRO
            SqlCommand command = new SqlCommand(nomSp,conexion);
            try
            {

                conexion.Open();
                transaction = conexion.BeginTransaction();
                command.Connection = conexion;
                command.Transaction = transaction;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@fecha", oPresupuesto.Fecha);
                command.Parameters.AddWithValue("@cliente", oPresupuesto.Cliente);
                command.Parameters.AddWithValue("@descuento", oPresupuesto.Descuento);
                command.Parameters.AddWithValue("@total", oPresupuesto.CalcularTotal());

                //parámetro de salida:
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@nro";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                command.Parameters.Add(pOut);
                command.ExecuteNonQuery();

                int presupuestoNro = (int)pOut.Value;

                SqlCommand cmdDetalle;
                int detalleNro = 1;
                foreach (DetallePresupuesto item in oPresupuesto.Detalles)
                {
                    cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLES_PRESUPUESTO", conexion, transaction);//IGUAL A 
                    //SqlCommand cmdDetalle = new SqlCommand();
                    //cmdDetalle.Connection = conexion;
                    //cmdDetalle.CommandText = spNombre; 
                    //cmdDetalle.Transaction = transaction;
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@presupuesto_nro", presupuestoNro);
                    cmdDetalle.Parameters.AddWithValue("@detalle_nro", detalleNro);
                    cmdDetalle.Parameters.AddWithValue("@id_producto", item.ProductoDet.ProductoNro);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", item.Cantidad);
                    cmdDetalle.ExecuteNonQuery();

                    detalleNro++;
                }
                transaction.Commit();
            }

            catch (Exception)
            {
                if (transaction != null)
                    transaction.Rollback();
                estado = false;
            }

            finally
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return estado;

        }




      

    }

}
