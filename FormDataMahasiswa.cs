﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Envi
{
    public partial class FormDataMahasiswa : Form
    {
        private string stringConnection = "Data Source=LAPTOP-MAPV1IJL;" +
            "Database=ProdiTI;User ID=sa;Password=123";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customerBindingSource = new BindingSource();
        
        private void FormDataMahasiswa_Load()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter(new SqlCommand("SELECT m.nim, m.nama_mahasiswa, m.alamat, m.jenis_kel, m.tgl_lahir, p.nama_prodi FROM dbo.Mahasiswa m JOIN dbo.Prodi p ON m.id_prodi = p.id_prodi", koneksi));

            DataSet ds = new DataSet();
            dataAdapter1.Fill(ds);

            this.customerBindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "NIM", true));
            this.txtNama.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "nama_mahasiswa", true));
            this.txtAlamat.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "alamat", true));
            this.cbxJenisKelamin.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "jenis_kel", true));
            this.dtTanggalLahir.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "nama_prodi", true));
            koneksi.Close();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTanggalLahir.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJenisKelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            ProdiCbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text;
            nama = txtNama.Text;
            jk = cbxJenisKelamin.Text;
            alamat = txtAlamat.Text;
            tgl = dtTanggalLahir.Value;
            prodi = cbxProdi.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "SELECT id_prodi FROM dbo.Prodi WHERE nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@dd", prodi)); // Fix the parameter name here
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "INSERT INTO dbo.Mahasiswa (nim, nama_mahasiswa, jenis_kel, alamat, tgl_lahir, id_prodi)" +
                "VALUES (@NIM, @Nm, @Jk, @Al, @Tgll, @Idp)";

            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@NIM", nim));
            cmd.Parameters.Add(new SqlParameter("@Nm", nama));
            cmd.Parameters.Add(new SqlParameter("@Jk", jk));
            cmd.Parameters.Add(new SqlParameter("@Al", alamat));
            cmd.Parameters.Add(new SqlParameter("@Tgll", tgl));
            cmd.Parameters.Add(new SqlParameter("@Idp", hs));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshForm();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJenisKelamin.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
        }

        private void refreshForm()
        {
            txtNIM.Enabled = false;
            txtNama.Enabled = false;
            txtAlamat.Enabled = false;
            cbxJenisKelamin.Enabled = false;
            dtTanggalLahir.Enabled = false;
            cbxProdi.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            clearBinding();
            FormDataMahasiswa_Load();
        }
        public FormDataMahasiswa()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            this.bnMahasiswa.BindingSource = this.customerBindingSource;
            refreshForm();
        }

        private void ProdiCbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.Prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }

        private void FormDataMahasiswa_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormHalamanUtama fhu = new FormHalamanUtama();
            fhu.Show();
            this.Hide();
        }
    }
}
