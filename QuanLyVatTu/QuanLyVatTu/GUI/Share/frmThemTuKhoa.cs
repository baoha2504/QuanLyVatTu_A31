using DevExpress.XtraPrinting;
using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmThemTuKhoa : Form
    {
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        int stt_danhmuc = 0;
        public frmThemTuKhoa()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                listdanhmuc = dbContext.DanhMucs.OrderBy(m => m.tendanhmuc).ToList();
                foreach (var dm in listdanhmuc)
                {
                    cbbDanhMuc.Items.Add(dm.tendanhmuc);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTuKhoa.Text))
            {
                MessageBox.Show("Tên từ khóa đang trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(cbbDanhMuc.Text))
            {
                MessageBox.Show("Tên danh mục đang trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dbContext = new QuanLyVatTuDbContext())
            {
                string tukhoa = txtTenTuKhoa.Text.Trim().ToLower();

                // Kiểm tra từ khóa chính
                bool isTukhoachinhExists = dbContext.TuKhoaVatTus.Any(tk => tk.tukhoachinh.ToLower() == tukhoa);
                if (isTukhoachinhExists)
                {
                    MessageBox.Show("Tên từ khóa vật tư đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra từ khóa trùng
                bool isTukhoatrungExists = dbContext.TuKhoaTrungs.Any(tk => tk.tukhoatrung.ToLower() == tukhoa);
                if (isTukhoatrungExists)
                {
                    MessageBox.Show("Tên từ khóa vật tư đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Thêm từ khóa mới
                TuKhoaVatTu tkvt = new TuKhoaVatTu
                {
                    tukhoachinh = txtTenTuKhoa.Text,
                    nguoisuacuoi = frmDangNhap.tennguoidung,
                    thoigiansua = DateTime.Now,
                    user_id = frmDangNhap.userID,
                    madanhmuc = listdanhmuc[stt_danhmuc].madanhmuc
                };

                dbContext.TuKhoaVatTus.Add(tkvt);

                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                lichSuHoatDong.thoigian = DateTime.Now;
                lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã thêm từ khóa vật tư '{txtTenTuKhoa.Text}'";
                lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                lichSuHoatDong.id = frmDangNhap.userID;
                dbContext.LichSuHoatDongs.Add(lichSuHoatDong);

                dbContext.SaveChanges();

                MessageBox.Show("Thêm từ khóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            stt_danhmuc = cbbDanhMuc.SelectedIndex;
        }
    }
}
