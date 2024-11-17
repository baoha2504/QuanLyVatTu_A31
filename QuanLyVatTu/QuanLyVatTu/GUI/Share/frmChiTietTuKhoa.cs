using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmChiTietTuKhoa : Form
    {
        public TuKhoaVatTu tkvt;
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        List<TuKhoaTrung> listtukhoatrung = new List<TuKhoaTrung>();
        List<TuKhoaTrung> listtukhoatrung_MoiThem = new List<TuKhoaTrung>();
        List<TuKhoaTrung> listtukhoatrung_DaXoa = new List<TuKhoaTrung>();
        int sochimuc = -1;
        int daload = 0;

        public frmChiTietTuKhoa(TuKhoaVatTu tkvt)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.tkvt = tkvt;
            dataGridView_DSTuKhoaTrung.RowTemplate.Height = 35;
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                listdanhmuc = dbContext.DanhMucs.OrderBy(m => m.tendanhmuc).ToList();
                foreach (var dm in listdanhmuc)
                {
                    cbbDanhMuc.Items.Add(dm.tendanhmuc);
                }
                cbbDanhMuc.Items.Add("Tất cả danh mục");
            }
            LoadData();
        }

        private void LoadData()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                txtTuKhoaChinh.Text = tkvt.tukhoachinh;

                if (!string.IsNullOrEmpty(tkvt.madanhmuc))
                {
                    DanhMuc danhmuc = listdanhmuc.FirstOrDefault(m => m.madanhmuc == tkvt.madanhmuc);
                    if (danhmuc != null)
                    {
                        cbbDanhMuc.Text = danhmuc.tendanhmuc;
                    }
                }

                txtNguoiSuaCuoi.Text = tkvt.nguoisuacuoi;
                txtThoiGianSua.Text = ((DateTime)tkvt.thoigiansua).ToString("HH:mm:ss dd/MM/yyyy");

                LoadData_GridView();
            }
        }

        private void LoadData_GridView()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                daload++;
                if (daload == 1)
                {
                    listtukhoatrung = dbContext.TuKhoaTrungs.Where(m => m.tukhoa_id == (int)tkvt.tukhoa_id).OrderBy(m => m.tukhoatrung).ToList();
                }
                dataGridView_DSTuKhoaTrung.Rows.Clear();
                for (int i = 0; i < listtukhoatrung.Count; i++)
                {
                    dataGridView_DSTuKhoaTrung.Rows.Add();
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column1"].Value = i + 1;
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column2"].Value = listtukhoatrung[i].tukhoatrung;
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column3"].Value = listtukhoatrung[i].nguoisuacuoi;
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column4"].Value = ((DateTime)listtukhoatrung[i].thoigiansua).ToString("HH:mm:ss dd/MM/yyyy");
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column5"].Value = "Xóa ▼";
                }
                for (int i = listtukhoatrung.Count; i < listtukhoatrung.Count + listtukhoatrung_MoiThem.Count; i++)
                {
                    int stt = i - listtukhoatrung.Count;
                    dataGridView_DSTuKhoaTrung.Rows.Add();
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column1"].Value = i + 1;
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column2"].Value = listtukhoatrung_MoiThem[stt].tukhoatrung;
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column3"].Value = listtukhoatrung_MoiThem[stt].nguoisuacuoi;
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column4"].Value = ((DateTime)listtukhoatrung_MoiThem[stt].thoigiansua).ToString("HH:mm:ss dd/MM/yyyy");
                    dataGridView_DSTuKhoaTrung.Rows[i].Cells["Column5"].Value = "Xóa ▼";
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmThemTuKhoaTrung frm = new frmThemTuKhoaTrung(tkvt.tukhoa_id);
            frm.ShowDialog();
            if (frm.tkt != null && frm.tkt.thoigiansua != null)
            {
                listtukhoatrung_MoiThem.Add(frm.tkt);
            }
            LoadData_GridView();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            daload = 0;
            LoadData();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                for (int i = 0; i < listtukhoatrung_DaXoa.Count; i++)
                {
                    int id = (int)listtukhoatrung_DaXoa[i].id;
                    TuKhoaTrung tkt = dbContext.TuKhoaTrungs.SingleOrDefault(m => m.id == id);
                    if (tkt != null)
                    {
                        dbContext.TuKhoaTrungs.Remove(tkt);
                    }
                }

                for (int i = 0; i < listtukhoatrung_MoiThem.Count; i++)
                {
                    TuKhoaTrung tkt = new TuKhoaTrung();
                    tkt.tukhoatrung = listtukhoatrung_MoiThem[i].tukhoatrung;
                    tkt.nguoisuacuoi = listtukhoatrung_MoiThem[i].nguoisuacuoi;
                    tkt.thoigiansua = listtukhoatrung_MoiThem[i].thoigiansua;
                    tkt.tukhoa_id = listtukhoatrung_MoiThem[i].tukhoa_id;
                    dbContext.TuKhoaTrungs.Add(tkt);
                }

                int id_tukhoavattu = (int)tkvt.tukhoa_id;
                TuKhoaVatTu tkvt_find = dbContext.TuKhoaVatTus.SingleOrDefault(m => m.tukhoa_id == id_tukhoavattu);
                tkvt_find.tukhoachinh = txtTuKhoaChinh.Text;
                tkvt_find.nguoisuacuoi = frmDangNhap.tennguoidung;
                tkvt_find.thoigiansua = DateTime.Now;
                tkvt_find.user_id = frmDangNhap.userID;
                if (sochimuc != -1)
                {
                    tkvt_find.madanhmuc = listdanhmuc[sochimuc].madanhmuc;
                }

                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                lichSuHoatDong.thoigian = DateTime.Now;
                lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã sửa thông tin từ khóa chính '{txtTuKhoaChinh.Text}'";
                lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                lichSuHoatDong.id = frmDangNhap.userID;
                dbContext.LichSuHoatDongs.Add(lichSuHoatDong);

                dbContext.SaveChanges();
                MessageBox.Show("Cập nhật thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listtukhoatrung_DaXoa.Clear();
                this.Close();
            }
        }

        private void dataGridView_DSTuKhoaTrung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSTuKhoaTrung.Columns["Column5"].Index && e.RowIndex >= 0)
            {
                try
                {
                    string str = (string)dataGridView_DSTuKhoaTrung.Rows[e.RowIndex].Cells["Column2"].Value;
                    DialogResult result = MessageBox.Show($"Xác nhận xóa từ khóa: '{str}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var check1 = listtukhoatrung.FirstOrDefault(m => m.tukhoatrung.Trim().ToLower() == str.Trim().ToLower());
                        var check2 = listtukhoatrung_MoiThem.FirstOrDefault(m => m.tukhoatrung.Trim().ToLower() == str.Trim().ToLower());
                        if (check1 != null)
                        {
                            listtukhoatrung_DaXoa.Add(check1);
                            listtukhoatrung.Remove(check1);
                        }
                        if (check2 != null)
                        {
                            listtukhoatrung_MoiThem.Remove(check2);
                        }
                        LoadData_GridView();
                    }
                }
                catch { }
            }
        }

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            sochimuc = cbbDanhMuc.SelectedIndex;
        }
    }
}
