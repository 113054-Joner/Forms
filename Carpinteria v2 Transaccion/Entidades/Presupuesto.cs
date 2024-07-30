using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.Entidades
{
    public class Presupuesto
    {
        
        public int PresupuestoNro { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }
        public DateTime FechaBaja { get; set; }
        List<DetallePresupuesto> listDetalles { get; set; }

        //public Presupuesto(int presupuestoNro, DateTime fecha, string cliente, double total, DateTime fechaBaja, List<DetallePresupuesto> listDetalles)
        //{
        //    PresupuestoNro = presupuestoNro;
        //    Fecha = fecha;
        //    Cliente = cliente;
        //    Total = total;
        //    FechaBaja = fechaBaja;
        //    this.listDetalles = listDetalles;
        //}

        public Presupuesto()
        {
            listDetalles = new List<DetallePresupuesto>();  
        }

        public void AgregarDetalle(DetallePresupuesto detalle) 
        {
            listDetalles.Add(detalle);
        }

        public void QuitarDetalle(int indice)
        {
            listDetalles.RemoveAt(indice);
        }

        public float CalcularTotal() 
        { 
            float total = 0;
            foreach(DetallePresupuesto detalle in listDetalles)
            {
                total += detalle.CalcularSubtotal();
            }
            
            return total;
        }

        public bool Confirmar ()
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
                command.Parameters.AddWithValue("@fecha", this.Fecha);
                command.Parameters.AddWithValue("@cliente", this.Cliente);
                command.Parameters.AddWithValue("@total", this.Total);
                command.Parameters.AddWithValue("@descuento", this.Descuento);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@nro";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;

                command.Parameters.Add(param);

                command.ExecuteNonQuery();

                this.PresupuestoNro = (int)param.Value;

                int detalleNro = 1;
                foreach (DetallePresupuesto d in listDetalles)
                {

                    SqlCommand commandDetalle = new SqlCommand();//2
                    commandDetalle.Connection = conexion;//2

                    commandDetalle.Transaction = transaccion;//4T  

                    commandDetalle.CommandType = CommandType.StoredProcedure;
                    commandDetalle.CommandText = "SP_INSERTAR_DETALLES_PRESUPUESTO";//Sentencias
                    commandDetalle.Parameters.AddWithValue("@presupuesto_nro", this.PresupuestoNro);
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
                if(conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return estado;
        }
    }
}
