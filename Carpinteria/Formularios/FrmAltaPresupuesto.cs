using Carpinteria.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria.Formularios
{
    public partial class FrmAltaPresupuesto : Form
    {
        private Presupuesto nuevo;

        public FrmAltaPresupuesto()
        {
            InitializeComponent();

            nuevo = new Presupuesto();
        }

        private void FrmAltaPresupuesto_Load(object sender, EventArgs e)
        {
            ProximoPresupuesto();
            CargarCombo();
            txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyyy");
            txtCliente.Text = "Consumidor Final";
            txtDescuento.Text = "0";
            //cboProductos.SelectedIndex = 0; no tiene sentido inicializarlo
            nudCantidad.Value = 0;
        }
         
        private void CargarCombo()
        {
            string cadenaConexion = "Data Source=localhost;Initial Catalog=db_ordenes;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(cadenaConexion);//1
            connection.Open();
            SqlCommand comando = new SqlCommand(cadenaConexion, connection);//2
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_CONSULTAR_MATERIALES";
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());

            connection.Close();
            
            cboProductos.DataSource = tabla;//Origen de datos
            cboProductos.DisplayMember = "nombre";//Valor que se ve y se elige,puede ir nro nNro
            cboProductos.ValueMember = "codigo";//ID del registro,puede ir nro 0
        }

        private void ProximoPresupuesto()
        {
            SqlConnection conexion = new SqlConnection();//1
            conexion.ConnectionString = "Data Source=localhost;Initial Catalog=db_ordenes;Integrated Security=True;";//1
            conexion.Open();
            SqlCommand command = new SqlCommand();//2
            command.Connection = conexion;//2
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_ULT_ID";//Sentencias
            
            //Sql Parametros
            SqlParameter param = new SqlParameter("@nro",SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            
            command.Parameters.Add(param);

            command.ExecuteNonQuery();//Reader Select -> DataTable , NonQuery Update,Inser,Delete 

            lblPresupuesto.Text += param.Value;

            conexion.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboProductos.SelectedIndex == -1 || cboProductos.Text.Equals(string.Empty)) 
            {
                MessageBox.Show("Debe Seleccionar un producto","Validando",MessageBoxButtons.OK,
                    MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                return ; 
            }
            if(nudCantidad.Value == 0) //string.IsNullOrEmpty int.TryParse(txt , out)
            {
                MessageBox.Show("Debe Ingresar la cantidad del producto", "Validando", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            //Validar si existe el item
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["ColProd"].Value.ToString().Equals(cboProductos.Text)) 
                {
                    MessageBox.Show("Este producto esta en el presupuesto", "Control", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
            }


            DataRowView item = (DataRowView)cboProductos.SelectedItem;//Obtener el item del combo, Devuelve un arraay con los datos
            // [nro_pro,nom,cant]
            int prod = Convert.ToInt32(item.Row.ItemArray[0]);//Aca selecionamos: nro de Prod
            string nom = item.Row.ItemArray[1].ToString();//Aca seleccionamos: nombre
            float precio = float.Parse(item.Row.ItemArray[2].ToString());//Aca seleccionamos cantidad

            Producto p = new Producto(prod,nom,precio);

            int cant = Convert.ToInt32(nudCantidad.Value);

            DetallePresupuesto detalle = new DetallePresupuesto(p, cant);

            nuevo.AgregarDetalle(detalle);//Agrego el detalle al presupuesto

            //dgvDetalles.Rows.Add(new object[] { item.Row.ItemArray[0], item.Row.ItemArray[1], item.Row.ItemArray[2], cant});//Agrego al Detalle al dgv

            dgvDetalles.Rows.Add(new object[] { prod,nom,precio,cant });//Agrego al Detalle al dgv

            CalcularTotal();
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 4) 
            {
                nuevo.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                CalcularTotal();
                return;
            }
        }

        private void CalcularTotal()
        {
            //Carga SubTotal: es todo sin descuento 

            txtSubTotal.Text = nuevo.CalcularTotal().ToString();

            //Carga Total: es todo con descuento, precio final
            //txtTotal.Text = (nuevo.CalcularTotal()-(nuevo.CalcularTotal() * (float.Parse(txtDescuento.Text)/100))).ToString();
            txtTotal.Text = (nuevo.CalcularTotal() * (1 - (float.Parse(txtDescuento.Text) / 100))).ToString();
        }
    }
}
