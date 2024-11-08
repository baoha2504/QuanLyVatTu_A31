using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Admin
{
    public partial class usr_NhatKyHoatDong : UserControl
    {
        public usr_NhatKyHoatDong()
        {
            InitializeComponent();
            dataGridView_DSLog.RowTemplate.Height = 35;
            LoadLog();
        }

        private void LoadLog()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var lichsuhoatdong = dbContext.LichSuHoatDongs
                                   .OrderByDescending(ls => ls.thoigian)
                                   .Take(500)
                                   .ToList();
                for (int i = 0; i < lichsuhoatdong.Count; i++)
                {
                    int id = (int)lichsuhoatdong[i].id;
                    dataGridView_DSLog.Rows.Add();
                    dataGridView_DSLog.Rows[i].Cells["Column1"].Value = id;
                    dataGridView_DSLog.Rows[i].Cells["Column2"].Value = ((DateTime)lichsuhoatdong[i].thoigian).ToString("HH:mm:ss dd/MM/yyyy");
                    dataGridView_DSLog.Rows[i].Cells["Column3"].Value = (string)lichsuhoatdong[i].hoatdong;
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dataGridView_DSLog.Rows.Clear();
            LoadLog();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSLog.Rows.Count - 1; i++)
            {
                string tenNguoiDung = ((string)dataGridView_DSLog.Rows[i].Cells["Column3"].Value).ToLower();

                if (tenNguoiDung.Contains(searchText))
                {
                    dataGridView_DSLog.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSLog.Rows[i].Visible = false;
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSLog.Rows.Count; i++)
                {
                    dataGridView_DSLog.Rows[i].Visible = true;
                }
            }
        }

        private void usr_NhatKyHoatDong_Resize(object sender, EventArgs e)
        {
            dataGridView_DSLog.Columns["Column1"].Width = (int)(dataGridView_DSLog.Width * 3 / 24);
            dataGridView_DSLog.Columns["Column2"].Width = (int)(dataGridView_DSLog.Width * 10 / 24);
            dataGridView_DSLog.Columns["Column3"].Width = (int)(dataGridView_DSLog.Width * 10 / 24);
        }

        private void txtNoiDungTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
