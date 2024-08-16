using DevExpress.XtraBars;
using QuanLyVatTu.GUI.Share;
using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.User
{
    public partial class frmUser : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        usr_DoiMatKhau usr_DoiMatKhau;
        usr_DanhMucVatTu usr_DanhMucVatTu;
        usr_DanhSachVatTu usr_DanhSachVatTu;
        usr_LocDanhSachVatTu usr_LocDanhSachVatTu;
        usr_ImportDanhSachVatTu usr_ImportDanhSachVatTu;

        public frmUser()
        {
            InitializeComponent();
            accordionControl1.Width = 260;
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            btnDoiMatKhau_Click(sender, e);
        }

        private void btnLapPhuongAnVatTu_Click(object sender, EventArgs e)
        {

        }

        private void btnDanhSachPhuongAnVatTu_Click(object sender, EventArgs e)
        {

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
            lblTieuDe2.Caption = "Lọc danh sách vật tư";
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

                    this.Hide();
                    frmDangNhap frmDangNhap = new frmDangNhap();
                    frmDangNhap.ShowDialog();
                    this.Close();
                }
            }
        }
    }
}
