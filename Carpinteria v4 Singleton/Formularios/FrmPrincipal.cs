﻿using Carpinteria.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void cosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAltaPresupuesto frmAlta = new FrmAltaPresupuesto();
            frmAlta.ShowDialog();//Conviene para tener 1 sola ventana
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
