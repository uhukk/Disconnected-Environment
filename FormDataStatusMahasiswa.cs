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
    public partial class FormDataStatusMahasiswa : Form
    {
        private string stringConnection = "Data Source=LAPTOP-MAPV1IJL;" +
            "Database=ProdiTI;User ID=sa;Password=123";
        private SqlConnection koneksi;

        private void refreshForm()
        {
            cbxNama.Enabled = false;
            cbxStatusMahasiswa.Enabled = false;
            cbxTahunMasuk.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxStatusMahasiswa.SelectedIndex = -1;
            cbxTahunMasuk.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
        }
        public FormDataStatusMahasiswa()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshForm();
        }

        private void dataGridView()
        {
            koneksi.Open();
            string str = "SELECT * from dbo.Status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void cbNama()
        {
            koneksi.Open();
            string str = "SELECT nama_mahasiswa FROM dbo.Mahasiswa WHERE " +
                "NOT EXISTS(SELECT id_status FROM dbo.Status_mahasiswa WHERE " +
                "status_mahasiswa.nim = mahasiswa.nim)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            koneksi.Close();

            cbxNama.DisplayMember = "nama_mahasiswa";
            cbxNama.ValueMember = "nama_mahasiswa";
            cbxNama.DataSource = ds.Tables[0];
        }

        private void cbTahunMasuk()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTahunMasuk.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbxTahunMasuk.Items.Add(l.ToString());
                }
            }
        }

        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "SELECT nim from dbo.Mahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Open();

            txtNIM.Text = nim;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxTahunMasuk.Enabled = true;
            cbxNama.Enabled = true;
            cbxStatusMahasiswa.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatusMahasiswa.Text;
            string tahunMasuk = cbxTahunMasuk.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";

            koneksi.Open();
            string str = "SELECT count (*) FROM dbo.Status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();

            if (count == 0)
            {
                kodeStatus = "1";
            }
            else
            {
                string queryString = "SELECT Max(id_status) from dbo.Status_mahasiswa";
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(queryString, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
                kodeStatus = Convert.ToString(finalKodeStatusInt);

            }
            string queryStrings = "INSERT into dbo.Status_mahasiswa (id_status, nim, " +
                "status_mahasiswa, tahun_masuk)" + "values(@ids, @NIM, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(queryStrings, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("ids", kodeStatus));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("sm", statusMahasiswa));
            cmd.Parameters.Add(new SqlParameter("tm", tahunMasuk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshForm();
            dataGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void FormDataStatusMahasiswa_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormHalamanUtama fhu = new FormHalamanUtama();
            fhu.Show();
            this.Hide();
        }
    }
}
