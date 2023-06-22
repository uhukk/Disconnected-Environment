using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Envi
{
    public partial class FormHalamanUtama : Form
    {
        public FormHalamanUtama()
        {
            InitializeComponent();
        }

        private void dataProdiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataProdi fdp = new FormDataProdi();
            fdp.Show();
            this.Hide();
        }

        private void dataMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataMahasiswa fdm = new FormDataMahasiswa();
            fdm.Show();
            this.Hide();
        }

        private void dataStatusMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataStatusMahasiswa fdsm = new FormDataStatusMahasiswa();
            fdsm.Show();
            this.Hide();
        }
    }
}
