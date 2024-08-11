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

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DanhMucVatTu : UserControl
    {
        public usr_DanhMucVatTu()
        {
            InitializeComponent();
            dataGridView_DSDanhMuc.RowTemplate.Height = 35;
            LoadDanhMuc();
        }

        private void LoadDanhMuc()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var danhmuc = dbContext.DanhMucs.ToList();
                for (int i = 0; i < danhmuc.Count; i++)
                {
                    dataGridView_DSDanhMuc.Rows.Add();
                    dataGridView_DSDanhMuc.Rows[i].Cells["Column1"].Value = danhmuc[i].madanhmuc;
                    dataGridView_DSDanhMuc.Rows[i].Cells["Column2"].Value = danhmuc[i].tendanhmuc;
                    dataGridView_DSDanhMuc.Rows[i].Cells["Column3"].Value = danhmuc[i].nguoisuacuoi;
                    dataGridView_DSDanhMuc.Rows[i].Cells["Column4"].Value = ((DateTime)danhmuc[i].thoigiansua).ToString("HH:mm:ss dd/MM/yyyy");
                    dataGridView_DSDanhMuc.Rows[i].Cells["Column5"].Value = "Sửa ▼";
                }
            }
        }

        private void dataGridView_DSDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSDanhMuc.Columns["Column5"].Index && e.RowIndex >= 0)
            {
                string madanhmuc = (string)dataGridView_DSDanhMuc.Rows[e.RowIndex].Cells["Column1"].Value;
                string tendanhmuc = (string)dataGridView_DSDanhMuc.Rows[e.RowIndex].Cells["Column2"].Value;

                frmThemDanhMuc frmThemDanhMuc = new frmThemDanhMuc(madanhmuc, tendanhmuc);
                frmThemDanhMuc.Text = "Sửa thông tin danh mục";
                frmThemDanhMuc.ShowDialog();
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnThemDanhMuc_Click(object sender, EventArgs e)
        {
            frmThemDanhMuc frmThemDanhMuc = new frmThemDanhMuc();
            frmThemDanhMuc.ShowDialog();
            btnLamMoi_Click(sender, e);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dataGridView_DSDanhMuc.Rows.Clear();
            LoadDanhMuc();
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSDanhMuc.Rows.Count - 1; i++)
            {
                string tenNguoiDung = ((string)dataGridView_DSDanhMuc.Rows[i].Cells["Column2"].Value).ToLower();

                if (tenNguoiDung.Contains(searchText))
                {
                    dataGridView_DSDanhMuc.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSDanhMuc.Rows[i].Visible = false;
                }
            }

        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSDanhMuc.Rows.Count; i++)
                {
                    dataGridView_DSDanhMuc.Rows[i].Visible = true;
                }
            }
        }
    }
}
