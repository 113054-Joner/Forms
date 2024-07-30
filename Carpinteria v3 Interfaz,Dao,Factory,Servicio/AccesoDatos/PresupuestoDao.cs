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
            bool estado = true;
            //MAESTRO
            SqlConnection conexion = new SqlConnection();//1

            //Transaccion
            SqlTransaction transaccion = null;//1T
            try
            {

                conexion.ConnectionString = "Data Source=localhost;Initial Catalog=Carpinteria;Integrated Security=True;";//1
                conexion.Open();
                transaccion = conexion.BeginTransaction();//2T

                SqlCommand command = new SqlCommand();//2
                command.Connection = conexion;//2

                command.Transaction = transaccion;//3T  

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_INSERTAR_PRESUPUESTO";//Sentencias
                command.Parameters.AddWithValue("@fecha", oPresupuesto.Fecha);
                command.Parameters.AddWithValue("@cliente", oPresupuesto.Cliente);
                command.Parameters.AddWithValue("@total", oPresupuesto.Total);
                command.Parameters.AddWithValue("@descuento", oPresupuesto.Descuento);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@nro";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;

                command.Parameters.Add(param);

                command.ExecuteNonQuery();

                oPresupuesto.PresupuestoNro = (int)param.Value;

                int detalleNro = 1;
                foreach (DetallePresupuesto d in oPresupuesto.Detalles )
                {

                    SqlCommand commandDetalle = new SqlCommand();//2
                    commandDetalle.Connection = conexion;//2

                    commandDetalle.Transaction = transaccion;//4T  

                    commandDetalle.CommandType = CommandType.StoredProcedure;
                    commandDetalle.CommandText = "SP_INSERTAR_DETALLES_PRESUPUESTO";//Sentencias
                    commandDetalle.Parameters.AddWithValue("@presupuesto_nro", oPresupuesto.PresupuestoNro);
                    commandDetalle.Parameters.AddWithValue("@detalle_nro", detalleNro);
                    commandDetalle.Parameters.AddWithValue("@id_producto", d.producto.ProductoNro);
                    commandDetalle.Parameters.AddWithValue("@cantidad", d.cantidad);
                    commandDetalle.ExecuteNonQuery();
                    detalleNro++;
                }

                transaccion.Commit();//5T Guarda todo 


            }
            catch (Exception ex)
            {
                transaccion.Rollback();//6T No se ejecuta nada
                estado = false;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return estado;
        }

        public DataTable ListarProductos()
        {
            string cadenaConexion = "Data Source=localhost;Initial Catalog=Carpinteria;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(cadenaConexion);//1
            connection.Open();
            SqlCommand comando = new SqlCommand(cadenaConexion, connection);//2
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_CONSULTAR_PRODUCTOS";
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());

            connection.Close();

            return tabla;
        }

        public int ObtenerProximoNumero() // 2 DAO
        {
            SqlConnection conexion = new SqlConnection();//1
            conexion.ConnectionString = "Data Source=localhost;Initial Catalog=Carpinteria;Integrated Security=True;";//1
            conexion.Open();
            SqlCommand command = new SqlCommand();//2
            command.Connection = conexion;//2
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_ULT_ID";//Sentencias

            //Sql Parametros
            SqlParameter param = new SqlParameter("@nro", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            command.Parameters.Add(param);

            command.ExecuteNonQuery();//Reader Select -> DataTable , NonQuery Update,Inser,Delete 

            

            conexion.Close();

            return (int)param.Value;
        }
    }
}
