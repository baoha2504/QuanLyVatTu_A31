using QuanLyVatTu.GUI.Share;
using QuanLyVatTu.Model;
using System;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Admin
{
    public partial class frmAdmin : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        usr_DanhSachTaiKhoan usr_DanhSachTaiKhoan;
        usr_DoiMatKhau usr_DoiMatKhau;
        usr_NhatKyDangNhap usr_NhatKyDangNhap;
        usr_NhatKyHoatDong usr_NhatKyHoatDong;
        usr_DanhMucVatTu usr_DanhMucVatTu;
        usr_DanhSachVatTu usr_DanhSachVatTu;
        usr_LocDanhSachVatTu usr_LocDanhSachVatTu;
        usr_ImportDanhSachVatTu usr_ImportDanhSachVatTu;
        usr_DanhSachPhuongAnVatTu usr_DanhSachPhuongAnVatTu;
        private bool isLoggingOut = false;

        public frmAdmin()
        {
            InitializeComponent();
            accordionControl1.Width = 260;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            btnDanhSachPhuongAnVatTu_Click(sender, e);
        }

        private void btnDanhSachPhuongAnVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Phương án vật tư";
            lblTieuDe2.Caption = "Danh sách phương án vật tư";
            if (usr_DanhSachPhuongAnVatTu == null)
            {
                usr_DanhSachPhuongAnVatTu = new usr_DanhSachPhuongAnVatTu(1);
                usr_DanhSachPhuongAnVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhSachPhuongAnVatTu);
                usr_DanhSachPhuongAnVatTu.BringToFront();
            }
            else
            {
                usr_DanhSachPhuongAnVatTu.BringToFront();
            }
        }

        private void btnDanhSachVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Danh sách vật tư";
            if (usr_DanhSachVatTu == null)
            {
                usr_DanhSachVatTu = new usr_DanhSachVatTu();
                usr_DanhSachVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhSachVatTu);
                usr_DanhSachVatTu.BringToFront();
            }
            else
            {
                usr_DanhSachVatTu.BringToFront();
            }
        }

        private void btnDanhMucVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Danh mục vật tư";
            if (usr_DanhMucVatTu == null)
            {
                usr_DanhMucVatTu = new usr_DanhMucVatTu();
                usr_DanhMucVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhMucVatTu);
                usr_DanhMucVatTu.BringToFront();
            }
            else
            {
                usr_DanhMucVatTu.BringToFront();
            }
        }

        private void btnLocDanhSachVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Chuấn hóa danh sách vật tư";
            if (usr_LocDanhSachVatTu == null)
            {
                usr_LocDanhSachVatTu = new usr_LocDanhSachVatTu();
                usr_LocDanhSachVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_LocDanhSachVatTu);
                usr_LocDanhSachVatTu.BringToFront();
            }
            else
            {
                usr_LocDanhSachVatTu.BringToFront();
            }
        }

        private void btnImportDanhSachVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Nhập file danh sách vật tư";
            if (usr_ImportDanhSachVatTu == null)
            {
                usr_ImportDanhSachVatTu = new usr_ImportDanhSachVatTu();
                usr_ImportDanhSachVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_ImportDanhSachVatTu);
                usr_ImportDanhSachVatTu.BringToFront();
            }
            else
            {
                usr_ImportDanhSachVatTu.BringToFront();
            }
        }

        private void btnDanhSachTaiKhoan_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Tài khoản hệ thông";
            lblTieuDe2.Caption = "Danh sách tài khoản";
            if (usr_DanhSachTaiKhoan == null)
            {
                usr_DanhSachTaiKhoan = new usr_DanhSachTaiKhoan();
                usr_DanhSachTaiKhoan.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhSachTaiKhoan);
                usr_DanhSachTaiKhoan.BringToFront();
            }
            else
            {
                usr_DanhSachTaiKhoan.BringToFront();
            }
        }

        private void btnNhatKyHoatDong_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Nhật ký hệ thống";
            lblTieuDe2.Caption = "Nhật ký hoạt động";
            if (usr_NhatKyHoatDong == null)
            {
                usr_NhatKyHoatDong = new usr_NhatKyHoatDong();
                usr_NhatKyHoatDong.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_NhatKyHoatDong);
                usr_NhatKyHoatDong.BringToFront();
            }
            else
            {
                usr_NhatKyHoatDong.BringToFront();
            }
        }

        private void btnNhatKyDangNhap_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Nhật ký hệ thống";
            lblTieuDe2.Caption = "Nhật ký đăng nhập";
            if (usr_NhatKyDangNhap == null)
            {
                usr_NhatKyDangNhap = new usr_NhatKyDangNhap();
                usr_NhatKyDangNhap.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_NhatKyDangNhap);
                usr_NhatKyDangNhap.BringToFront();
            }
            else
            {
                usr_NhatKyDangNhap.BringToFront();
            }
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Trợ giúp";
            lblTieuDe2.Caption = "Đổi mật khẩu";
            if (usr_DoiMatKhau == null)
            {
                usr_DoiMatKhau = new usr_DoiMatKhau();
                usr_DoiMatKhau.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DoiMatKhau);
                usr_DoiMatKhau.BringToFront();
            }
            else
            {
                usr_DoiMatKhau.BringToFront();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    LichSuDangNhap lichSuDangNhap = new LichSuDangNhap();
                    lichSuDangNhap.thoigian = DateTime.Now;
                    lichSuDangNhap.trangthai = $"Tài khoản {lblNguoiDung.Caption} đăng xuất thành công";
                    dbContext.LichSuDangNhaps.Add(lichSuDangNhap);
                    dbContext.SaveChanges();

                    // Đặt cờ đăng xuất để ngăn sự kiện FormClosing
                    isLoggingOut = true;

                    // Ẩn form và mở lại form đăng nhập
                    this.Hide();
                    frmDangNhap frmDangNhap = new frmDangNhap();
                    frmDangNhap.ShowDialog();

                    // Đóng form sau khi đăng nhập thành công
                    this.Close();
                }
            }
        }

        private void frmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLoggingOut)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng form không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    using (var dbContext = new QuanLyVatTuDbContext())
                    {
                        LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                        lichSuHoatDong.thoigian = DateTime.Now;
                        lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã đăng xuất thành công";
                        lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                        lichSuHoatDong.id = frmDangNhap.userID;
                        dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                        dbContext.SaveChanges();
                    }
                }
            }
        }
    }
}
