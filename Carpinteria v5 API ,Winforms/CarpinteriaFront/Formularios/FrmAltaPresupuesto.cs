using CarpinteriaFront.AccesoDatos;
using CarpinteriaFront.Entidades;
using CarpinteriaFront.Servicios;
using CarpinteriaFront.Servicios.Implementacion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarpinteriaFront.Formularios
{
    public partial class FrmAltaPresupuesto : Form
    {
        private Presupuesto nuevo; //6 DAO
        //private GestorPresupuesto gestorPresupuestoService;//6 FACT
        private PresupuestoService gestorPresupuestoService;
        //private PresupuestoDao presupuestoDao = new PresupuestoDao();

        public FrmAltaPresupuesto()
        {
            InitializeComponent();

            nuevo = new Presupuesto();
            //gestorPresupuestoService = new GestorPresupuesto(); //7 DAO
            //gestorPresupuestoService = new GestorPresupuesto(new DaoFactory());//7 FACT
            gestorPresupuestoService = new PresupuestoService();
        }

        private void FrmAltaPresupuesto_Load(object sender, EventArgs e)
        {

            //lblPresupuesto.Text += presupuestoDao.ObtenerProximoNumero();// Es directo a la BS
            lblPresupuesto.Text += gestorPresupuestoService.ProximoPresupuesto();
            CargarCombo();
            txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyyy");
            txtCliente.Text = "Consumidor Final";
            txtDescuento.Text = "0";
            //cboProductos.SelectedIndex = 0; no tiene sentido inicializarlo
            nudCantidad.Value = 0;
        }
         
        private void CargarCombo()
        {
            List<Producto> lista = new List<Producto>();
            lista = gestorPresupuestoService.ListarProductos();
            cboProductos.DataSource =  lista;//Origen de datos
            cboProductos.DisplayMember = "Nombre";//Valor que se ve y se elige,puede ir nro nNro
            cboProductos.ValueMember = "ProductoNro";//ID del registro,puede ir nro 0

            cboProductos.DropDownStyle = ComboBoxStyle.DropDownList;//El como no es modificable 
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


            Producto item = (Producto)cboProductos.SelectedItem;//Obtener el item del combo, Devuelve un arraay con los datos
            // [nro_pro,nom,cant]
            int prod = item.ProductoNro;//Aca selecionamos: nro de Prod
            string nom = item.Nombre;//Aca seleccionamos: nombre
            double precio = item.Precio;//Aca seleccionamos cantidad

            

            int cant = Convert.ToInt32(nudCantidad.Value);

            DetallePresupuesto detalle = new DetallePresupuesto(item, cant);

            nuevo.AgregarDetalle(detalle);//Agrego el detalle al presupuesto

            //dgvDetalles.Rows.Add(new object[] { item.Row.ItemArray[0], item.Row.ItemArray[1], item.Row.ItemArray[2], cant});//Agrego al Detalle al dgv

            dgvDetalles.Rows.Add(new object[] { prod,nom,precio,cant, nuevo.CalcularTotal() });//Agrego al Detalle al dgv

            CalcularTotal();
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 5) 
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //VALIDACIONES TODO LO QUE VA A LA BD 
            if(string.IsNullOrEmpty(txtCliente.Text))
            {
                MessageBox.Show("Complete el nombre del cliente", "Control", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                txtCliente.Focus();
                return;
            }
            if(dgvDetalles.RowCount == 0)
            {
                MessageBox.Show("Seleccione un producto", "Control", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                cboProductos.Focus();
                return;
            }
            if (nudCantidad.Value == 0)
            {
                MessageBox.Show("Seleccione una cantidad valida", "Control", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                cboProductos.Focus();
                return;
            }

            //Grabar Maestro y Detalles
            GuardaPresupuesto();
            //GuardarDetallesPresupuesto();
        }

        private void GuardaPresupuesto()
        {
            nuevo.Fecha = Convert.ToDateTime(txtFecha.Text);
            nuevo.Cliente = txtCliente.Text;
            nuevo.Descuento = double.Parse(txtDescuento.Text);
            nuevo.Total = Convert.ToDouble(txtTotal.Text);

            if (gestorPresupuestoService.CrearPresupuesto(nuevo))
            {
                MessageBox.Show("Grabado Correctamente", "Informe", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                this.Close();
            }
            else 
            {
                MessageBox.Show("No se pudo grabar", "Informe", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea salir?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) 
                == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
