using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DanhSachTuCungNghia : UserControl
    {
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        List<TuKhoaVatTu> tuKhoas = new List<TuKhoaVatTu>();

        public usr_DanhSachTuCungNghia()
        {
            InitializeComponent();
            dataGridView_DSTuKhoa.RowTemplate.Height = 35;
            LoadDSTuKhoa();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                listdanhmuc = dbContext.DanhMucs.OrderBy(m => m.tendanhmuc).ToList();
                foreach (var dm in listdanhmuc)
                {
                    cbbDanhMuc.Items.Add(dm.tendanhmuc);
                }
                cbbDanhMuc.Items.Add("Tất cả danh mục");
            }
        }

        private void LoadDSTuKhoa()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var tukhoa = dbContext.TuKhoaVatTus.OrderBy(m => m.tukhoachinh).ToList();
                dataGridView_DSTuKhoa.Rows.Clear();
                if (tukhoa != null)
                {
                    tuKhoas = tukhoa;
                    for (int i = 0; i < tukhoa.Count; i++)
                    {
                        dataGridView_DSTuKhoa.Rows.Add();
                        dataGridView_DSTuKhoa.Rows[i].Cells["Column1"].Value = i + 1;
                        dataGridView_DSTuKhoa.Rows[i].Cells["Column2"].Value = tukhoa[i].tukhoachinh;

                        string madanhmuc = (string)tukhoa[i].madanhmuc;
                        DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                        if (danhMuc != null)
                        {
                            dataGridView_DSTuKhoa.Rows[i].Cells["Column3"].Value = danhMuc.tendanhmuc;
                        }
                        dataGridView_DSTuKhoa.Rows[i].Cells["Column4"].Value = tukhoa[i].nguoisuacuoi;
                        dataGridView_DSTuKhoa.Rows[i].Cells["Column5"].Value = ((DateTime)tukhoa[i].thoigiansua).ToString("HH:mm:ss dd/MM/yyyy");
                        dataGridView_DSTuKhoa.Rows[i].Cells["Column6"].Value = "Sửa ▼";
                        dataGridView_DSTuKhoa.Rows[i].Cells["Column7"].Value = "Xóa ▼";
                    }
                }
            }
        }

        private void dataGridView_DSTuKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSTuKhoa.Columns["Column6"].Index && e.RowIndex >= 0)
            {
                try
                {
                    string str = (string)dataGridView_DSTuKhoa.Rows[e.RowIndex].Cells["Column2"].Value;
                    TuKhoaVatTu tukhoa = tuKhoas.FirstOrDefault(m => m.tukhoachinh == str);
                    if (tukhoa != null)
                    {
                        frmChiTietTuKhoa frmChiTietTuKhoa = new frmChiTietTuKhoa(tukhoa);
                        frmChiTietTuKhoa.Show();
                    }
                }
                catch { }
            }
            if (e.ColumnIndex == dataGridView_DSTuKhoa.Columns["Column7"].Index && e.RowIndex >= 0)
            {
                try
                {
                    string str = (string)dataGridView_DSTuKhoa.Rows[e.RowIndex].Cells["Column2"].Value;
                    DialogResult result = MessageBox.Show($"Xác nhận xóa từ khóa: '{str}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        using (var dbContext = new QuanLyVatTuDbContext())
                        {
                            var tukhoavattu = dbContext.TuKhoaVatTus.FirstOrDefault(m => m.tukhoachinh == str);
                            var listtukhoatrung = dbContext.TuKhoaTrungs.Where(m => m.tukhoa_id == tukhoavattu.tukhoa_id).ToList();
                            if (listtukhoatrung != null)
                            {
                                for (int i = 0; i < listtukhoatrung.Count; i++)
                                {
                                    dbContext.TuKhoaTrungs.Remove(listtukhoatrung[i]);
                                }
                            }
                            dbContext.TuKhoaVatTus.Remove(tukhoavattu);

                            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                            lichSuHoatDong.thoigian = DateTime.Now;
                            lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã xóa từ khóa vật tư '{str}'";
                            lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                            lichSuHoatDong.id = frmDangNhap.userID;
                            dbContext.LichSuHoatDongs.Add(lichSuHoatDong);

                            dbContext.SaveChanges();
                        }
                        LoadDSTuKhoa();
                    }
                }
                catch { }
            }
        }

        private void btnThemTuKhoa_Click(object sender, EventArgs e)
        {
            frmThemTuKhoa frm = new frmThemTuKhoa();
            frm.ShowDialog();
            LoadDSTuKhoa();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cbbDanhMuc.Text = "Tất cả danh mục";
            txtNoiDungTimKiem.Text = string.Empty;
            LoadDSTuKhoa();
        }

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDanhMuc.Text == "Tất cả danh mục")
            {
                for (int i = 0; i < dataGridView_DSTuKhoa.Rows.Count; i++)
                {
                    dataGridView_DSTuKhoa.Rows[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView_DSTuKhoa.Rows.Count - 1; i++)
                {
                    try
                    {
                        string tenTuKhoa = ((string)dataGridView_DSTuKhoa.Rows[i].Cells["Column3"].Value).ToLower();

                        if (tenTuKhoa.Contains(cbbDanhMuc.Text.ToLower()))
                        {
                            dataGridView_DSTuKhoa.Rows[i].Visible = true;
                        }
                        else
                        {
                            dataGridView_DSTuKhoa.Rows[i].Visible = false;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSTuKhoa.Rows.Count - 1; i++)
            {
                string tenTuKhoa = ((string)dataGridView_DSTuKhoa.Rows[i].Cells["Column2"].Value).ToLower();

                if (tenTuKhoa.Contains(searchText))
                {
                    dataGridView_DSTuKhoa.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSTuKhoa.Rows[i].Visible = false;
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSTuKhoa.Rows.Count; i++)
                {
                    dataGridView_DSTuKhoa.Rows[i].Visible = true;
                }
            }
        }

        private void usr_DanhSachTuCungNghia_Resize(object sender, EventArgs e)
        {
            dataGridView_DSTuKhoa.Columns["Column1"].Width = (int)(dataGridView_DSTuKhoa.Width * 1 / 20);
            dataGridView_DSTuKhoa.Columns["Column2"].Width = (int)(dataGridView_DSTuKhoa.Width * 4 / 20);
            dataGridView_DSTuKhoa.Columns["Column3"].Width = (int)(dataGridView_DSTuKhoa.Width * 4 / 20);
            dataGridView_DSTuKhoa.Columns["Column4"].Width = (int)(dataGridView_DSTuKhoa.Width * 3 / 20);
            dataGridView_DSTuKhoa.Columns["Column5"].Width = (int)(dataGridView_DSTuKhoa.Width * 3 / 20);
            dataGridView_DSTuKhoa.Columns["Column6"].Width = (int)(dataGridView_DSTuKhoa.Width * 2 / 20);
            dataGridView_DSTuKhoa.Columns["Column7"].Width = (int)(dataGridView_DSTuKhoa.Width * 2 / 20);
        }
    }
}
