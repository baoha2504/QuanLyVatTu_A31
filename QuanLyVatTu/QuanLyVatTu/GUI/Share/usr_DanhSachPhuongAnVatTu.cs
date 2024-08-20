using QuanLyVatTu.GUI.Admin;
using QuanLyVatTu.GUI.User;
using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DanhSachPhuongAnVatTu : UserControl
    {
        List<PhuongAnVatTu> DS_phuonganvattus = new List<PhuongAnVatTu>();
        private int phanquyen = 0;

        public usr_DanhSachPhuongAnVatTu(int phanquyen)
        {
            InitializeComponent();
            dataGridView_DS_PAVT.RowTemplate.Height = 35;
            this.phanquyen = phanquyen;
            LoadData();
        }

        private void LoadData()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                List<PhuongAnVatTu> phuonganvattu = new List<PhuongAnVatTu>();
                List<PhuongAnVatTu> phuonganvattu_type2 = new List<PhuongAnVatTu>();
                List<PhuongAnVatTu> phuonganvattu_type3 = new List<PhuongAnVatTu>();
                List<PhuongAnVatTu> phuonganvattu_type4 = new List<PhuongAnVatTu>();
                List<PhuongAnVatTu> phuonganvattu_type5 = new List<PhuongAnVatTu>();
                if (phanquyen == 1)
                {
                    phuonganvattu = dbContext.PhuongAnVatTus.OrderByDescending(p => p.thoigianlap).ToList();
                }
                else if (phanquyen == 2)
                {
                    int userid = frmDangNhap.userID;
                    phuonganvattu = dbContext.PhuongAnVatTus.Where(m=>m.user_id == userid).OrderByDescending(p => p.thoigianlap).ToList();
                }

                if (groupPanel1.Text == "Danh sách tất cả phương án vật tư")
                {
                    DS_phuonganvattus = phuonganvattu;
                }
                else if (groupPanel1.Text == "Danh sách phương án vật tư chưa duyệt")
                {
                    phuonganvattu_type2 = phuonganvattu.Where(p => p.nguoiduyet == null && p.thoigianduyet == null)
                             .OrderByDescending(p => p.thoigianlap)
                             .ToList();
                    DS_phuonganvattus = phuonganvattu_type2;
                }
                else if (groupPanel1.Text == "Danh sách phương án vật tư đã sửa nhưng chưa duyệt")
                {
                    phuonganvattu_type3 = phuonganvattu.Where(p => p.thoigianlap > p.thoigianduyet)
                             .OrderByDescending(p => p.thoigianlap)
                             .ToList();
                    DS_phuonganvattus = phuonganvattu_type3;
                }
                else if (groupPanel1.Text == "Danh sách phương án vật tư đã duyệt")
                {
                    phuonganvattu_type4 = phuonganvattu.Where(p => p.thoigianlap <= p.thoigianduyet)
                             .OrderByDescending(p => p.thoigianlap)
                             .ToList();
                    DS_phuonganvattus = phuonganvattu_type4;
                }

                else if (groupPanel1.Text == "Danh sách tất cả phương án đã hoàn thành")
                {
                    phuonganvattu_type5 = phuonganvattu.Where(p => p.hoanthanh == 1)
                             .OrderByDescending(p => p.thoigianlap)
                             .ToList();
                    DS_phuonganvattus = phuonganvattu_type5;
                }


                for (int i = 0; i < DS_phuonganvattus.Count; i++)
                {
                    dataGridView_DS_PAVT.Rows.Add();
                    dataGridView_DS_PAVT.Rows[i].Cells["Column1"].Value = DS_phuonganvattus[i].maphuongan;
                    dataGridView_DS_PAVT.Rows[i].Cells["Column2"].Value = DS_phuonganvattus[i].tenphuongan;
                    dataGridView_DS_PAVT.Rows[i].Cells["Column3"].Value = DS_phuonganvattus[i].nguoilap;
                    if (DS_phuonganvattus[i].thoigianlap != null)
                    {
                        DateTime dateTime1 = (DateTime)DS_phuonganvattus[i].thoigianlap;
                        dataGridView_DS_PAVT.Rows[i].Cells["Column4"].Value = dateTime1.ToString("HH:mm:ss dd/MM/yyyy");
                    }
                    dataGridView_DS_PAVT.Rows[i].Cells["Column5"].Value = DS_phuonganvattus[i].nguoiduyet;
                    if (DS_phuonganvattus[i].thoigianduyet != null)
                    {
                        DateTime dateTime2 = (DateTime)DS_phuonganvattus[i].thoigianduyet;
                        dataGridView_DS_PAVT.Rows[i].Cells["Column6"].Value = dateTime2.ToString("HH:mm:ss dd/MM/yyyy");
                    }
                    dataGridView_DS_PAVT.Rows[i].Cells["Column7"].Value = DS_phuonganvattus[i].noidungduyet;
                    dataGridView_DS_PAVT.Rows[i].Cells["Column8"].Value = "Xem";
                    if(DS_phuonganvattus[i].hoanthanh == 1)
                    {
                        dataGridView_DS_PAVT.Rows[i].Cells["Column9"].Value = "Hoàn thành";
                    }
                    else
                    {
                        dataGridView_DS_PAVT.Rows[i].Cells["Column9"].Value = "Chưa hoàn thành";
                    }
                }
            }
        }

        private void dataGridView_DS_PAVT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DS_PAVT.Columns["Column8"].Index && e.RowIndex >= 0)
            {
                int rowID = e.RowIndex;
                if (phanquyen == 1)
                {
                    frmChiTietPhuongAnVatTu_Admin frm = new frmChiTietPhuongAnVatTu_Admin(DS_phuonganvattus[rowID]);
                    frm.ShowDialog();
                    DS_phuonganvattus[rowID] = frm.pavt;
                    LamMoi();
                }
                else if (phanquyen == 2)
                {
                    frmChiTietPhuongAnVatTu_User frm = new frmChiTietPhuongAnVatTu_User(DS_phuonganvattus[rowID]);
                    frm.ShowDialog();
                    DS_phuonganvattus[rowID] = frm.pavt;
                    LamMoi();
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DS_PAVT.Rows.Count - 1; i++)
            {
                string tenphuongan = ((string)dataGridView_DS_PAVT.Rows[i].Cells["Column2"].Value).ToLower();

                if (tenphuongan.Contains(searchText))
                {
                    dataGridView_DS_PAVT.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DS_PAVT.Rows[i].Visible = false;
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DS_PAVT.Rows.Count; i++)
                {
                    dataGridView_DS_PAVT.Rows[i].Visible = true;
                }
            }
        }

        private void LamMoi()
        {
            txtNoiDungTimKiem.Text = string.Empty;
            dataGridView_DS_PAVT.Rows.Clear();
            DS_phuonganvattus.Clear();
            LoadData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnChuaDuyet_Click(object sender, EventArgs e)
        {
            btnHienThi.Text = "Hiển thị (1)";
            groupPanel1.Text = "Danh sách phương án vật tư chưa duyệt";
            LamMoi();
        }

        private void btnDaSuaChuaDuyet_Click(object sender, EventArgs e)
        {
            btnHienThi.Text = "Hiển thị (2)";
            groupPanel1.Text = "Danh sách phương án vật tư đã sửa nhưng chưa duyệt";
            LamMoi();
        }

        private void btnDaDuyet_Click(object sender, EventArgs e)
        {
            btnHienThi.Text = "Hiển thị (3)";
            groupPanel1.Text = "Danh sách phương án vật tư đã duyệt";
            LamMoi();
        }

        private void btnDaHoanThanh_Click(object sender, EventArgs e)
        {
            btnHienThi.Text = "Hiển thị (4)";
            groupPanel1.Text = "Danh sách tất cả phương án đã hoàn thành";
            LamMoi();
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            btnHienThi.Text = "Hiển thị (5)";
            groupPanel1.Text = "Danh sách tất cả phương án vật tư";
            LamMoi();
        }
    }
}
