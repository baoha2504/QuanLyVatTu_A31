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

        }

        private void btnDanhMucVatTu_Click(object sender, EventArgs e)
        {

        }

        private void btnLocDanhSachVatTu_Click(object sender, EventArgs e)
        {

        }

        private void btnImportDanhSachVatTu_Click(object sender, EventArgs e)
        {

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
