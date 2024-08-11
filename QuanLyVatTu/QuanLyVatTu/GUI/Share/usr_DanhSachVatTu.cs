using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DanhSachVatTu : UserControl
    {
        public usr_DanhSachVatTu()
        {
            InitializeComponent();
            dataGridView_DSVatTu.RowTemplate.Height = 35;
            LoadDSVatTu();
        }

        private void LoadDSVatTu()
        {
            dataGridView_DSVatTu.Rows.Clear();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vattu = new List<VatTu>();
                if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
                {
                    vattu = dbContext.VatTus.Where(m => m.trangthai == 1).ToList();
                }
                else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
                {
                    vattu = dbContext.VatTus.Where(m => m.trangthai == 0).ToList();
                }
                else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
                {
                    vattu = dbContext.VatTus.Where(m => m.trangthai == 2).ToList();
                }
                else if (groupPanel1.Text == "Danh sách tất cả vật tư")
                {
                    vattu = dbContext.VatTus.ToList();
                }

                for (int i = 0; i < vattu.Count; i++)
                {
                    string madanhmuc = (string)vattu[i].madanhmuc;
                    DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    dataGridView_DSVatTu.Rows.Add();
                    dataGridView_DSVatTu.Rows[i].Cells["Column1"].Value = vattu[i].mavattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value = vattu[i].tenvattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column3"].Value = vattu[i].donvitinh;
                    dataGridView_DSVatTu.Rows[i].Cells["Column4"].Value = vattu[i].dongia;
                    dataGridView_DSVatTu.Rows[i].Cells["Column5"].Value = vattu[i].nguongoc;
                    if(vattu[i].trangthai == 0)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Dừng sử dụng";
                    } else if (vattu[i].trangthai == 1)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Đang sử dụng";
                    }
                    else if (vattu[i].trangthai == 2)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Bị trùng";
                    }
                    dataGridView_DSVatTu.Rows[i].Cells["Column7"].Value = danhMuc.tendanhmuc;
                    dataGridView_DSVatTu.Rows[i].Cells["Column8"].Value = vattu[i].nguoisuacuoi;
                    dataGridView_DSVatTu.Rows[i].Cells["Column9"].Value = "Sửa ▼";

                }
            }
        }

        private void dataGridView_DSDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTu.Columns["Column9"].Index && e.RowIndex >= 0)
            {
                //// Lấy ID của dòng được chọn
                //int rowID = (int)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column1"].Value;
                //string tennguoidung = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column2"].Value;
                //string quanham = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column3"].Value;
                //string chucvu = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column4"].Value;
                //string tentaikhoan = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column5"].Value;
                //string trangthai = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column7"].Value;

                //frmThemNguoiDung frmThemNguoiDung = new frmThemNguoiDung(rowID, tennguoidung, quanham, chucvu, tentaikhoan, trangthai);
                //frmThemNguoiDung.Text = "Sửa thông tin người dùng";
                //frmThemNguoiDung.ShowDialog();
                //LoadDSVatTu();
            }
        }

        private void btnThemVatTu_Click(object sender, EventArgs e)
        {
            frmThemVatTu frmThemVatTu = new frmThemVatTu();
            frmThemVatTu.ShowDialog();
            LoadDSVatTu();
        }

        private void btnVatTuDangSuDung_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách vật tư đang sử dụng")
            {
                groupPanel1.Text = "Danh sách vật tư đang sử dụng";
                btnHienThi.Text = "VT đang sử dụng";
                LoadDSVatTu();
            }
        }

        private void btnVatTuDungSuDung_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách vật tư dừng sử dụng")
            {
                groupPanel1.Text = "Danh sách vật tư dừng sử dụng";
                btnHienThi.Text = "VT dừng sử dụng";
                LoadDSVatTu();
            }
        }

        private void btnVatTuBiTrung_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách vật tư bị trùng")
            {
                groupPanel1.Text = "Danh sách vật tư bị trùng";
                btnHienThi.Text = "Vật tư bị trùng";
                LoadDSVatTu();
            }
        }

        private void btnTatCaVatTu_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách tất cả vật tư")
            {
                groupPanel1.Text = "Danh sách tất cả vật tư";
                btnHienThi.Text = "Tất cả vật tư";
                LoadDSVatTu();
            }
        }

        private void btnXuatDanhSach_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDSVatTu();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSVatTu.Rows.Count - 1; i++)
            {
                string tenNguoiDung = ((string)dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value).ToLower();

                if (tenNguoiDung.Contains(searchText))
                {
                    dataGridView_DSVatTu.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSVatTu.Rows[i].Visible = false;
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTu.Rows.Count; i++)
                {
                    dataGridView_DSVatTu.Rows[i].Visible = true;
                }
            }
        }
    }
}
