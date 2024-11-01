using DevExpress.XtraBars;
using QuanLyVatTu.GUI.Admin;
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
        //usr_LocDanhSachVatTu usr_LocDanhSachVatTu;
        usr_LocDanhSachVatTu2 usr_LocDanhSachVatTu2;
        usr_ImportDanhSachVatTu usr_ImportDanhSachVatTu;
        usr_LapPhuongAnVatTu usr_LapPhuongAnVatTu;
        usr_DanhSachPhuongAnVatTu usr_DanhSachPhuongAnVatTu;
        usr_DanhSachTuCungNghia usr_DanhSachTuCungNghia;
        private bool isLoggingOut = false;

        public frmUser()
        {
            InitializeComponent();
            accordionControl1.Width = 260;
        }

        private void HideControlInMainContainer()
        {
            foreach (Control control in mainContainer.Controls)
            {
                control.Visible = false;
            }
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            btnDanhSachPhuongAnVatTu_Click(sender, e);
        }

        private void btnLapPhuongAnVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Phương án vật tư";
            lblTieuDe2.Caption = "Lập phương án vật tư";
            HideControlInMainContainer();
            if (usr_LapPhuongAnVatTu == null)
            {
                usr_LapPhuongAnVatTu = new usr_LapPhuongAnVatTu();
                usr_LapPhuongAnVatTu.Dock = DockStyle.Fill;
                usr_LapPhuongAnVatTu.ButtonClicked += btnDanhSachPhuongAnVatTu_Click;
                mainContainer.Controls.Add(usr_LapPhuongAnVatTu);
                usr_LapPhuongAnVatTu.BringToFront();
            }
            else
            {
                usr_LapPhuongAnVatTu.Visible = true;
                usr_LapPhuongAnVatTu.BringToFront();
            }
        }

        private void btnDanhSachPhuongAnVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Phương án vật tư";
            lblTieuDe2.Caption = "Danh sách phương án vật tư";
            HideControlInMainContainer();
            if (usr_DanhSachPhuongAnVatTu == null)
            {
                usr_DanhSachPhuongAnVatTu = new usr_DanhSachPhuongAnVatTu(2);
                usr_DanhSachPhuongAnVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhSachPhuongAnVatTu);
                usr_DanhSachPhuongAnVatTu.BringToFront();
            }
            else
            {
                usr_DanhSachPhuongAnVatTu.Visible = true;
                usr_DanhSachPhuongAnVatTu.BringToFront();
                usr_DanhSachPhuongAnVatTu.LamMoi();
            }
        }

        private void btnDanhSachVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Danh sách vật tư";
            HideControlInMainContainer();
            if (usr_DanhSachVatTu == null)
            {
                usr_DanhSachVatTu = new usr_DanhSachVatTu();
                usr_DanhSachVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhSachVatTu);
                usr_DanhSachVatTu.BringToFront();
            }
            else
            {
                usr_DanhSachVatTu.Visible = true;
                usr_DanhSachVatTu.BringToFront();
            }
        }

        private void btnDanhMucVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Danh mục vật tư";
            HideControlInMainContainer();
            if (usr_DanhMucVatTu == null)
            {
                usr_DanhMucVatTu = new usr_DanhMucVatTu();
                usr_DanhMucVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhMucVatTu);
                usr_DanhMucVatTu.BringToFront();
            }
            else
            {
                usr_DanhMucVatTu.Visible = true;
                usr_DanhMucVatTu.BringToFront();
            }
        }

        private void btnLocDanhSachVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Chuẩn hóa danh sách vật tư";
            HideControlInMainContainer();
            //if (usr_LocDanhSachVatTu == null)
            //{
            //    usr_LocDanhSachVatTu = new usr_LocDanhSachVatTu();
            //    usr_LocDanhSachVatTu.Dock = DockStyle.Fill;
            //    mainContainer.Controls.Add(usr_LocDanhSachVatTu);
            //    usr_LocDanhSachVatTu.BringToFront();
            //}
            //else
            //{
            //    usr_LocDanhSachVatTu.BringToFront();
            //}

            if (usr_LocDanhSachVatTu2 == null)
            {
                usr_LocDanhSachVatTu2 = new usr_LocDanhSachVatTu2();
                usr_LocDanhSachVatTu2.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_LocDanhSachVatTu2);
                usr_LocDanhSachVatTu2.BringToFront();
            }
            else
            {
                usr_LocDanhSachVatTu2.Visible = true;
                usr_LocDanhSachVatTu2.BringToFront();
            }
        }

        private void btnImportDanhSachVatTu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Nhập file danh sách vật tư";
            HideControlInMainContainer();
            if (usr_ImportDanhSachVatTu == null)
            {
                usr_ImportDanhSachVatTu = new usr_ImportDanhSachVatTu();
                usr_ImportDanhSachVatTu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_ImportDanhSachVatTu);
                usr_ImportDanhSachVatTu.BringToFront();
            }
            else
            {
                usr_ImportDanhSachVatTu.Visible = true;
                usr_ImportDanhSachVatTu.BringToFront();
            }
        }

        private void btnTuKhoaCungNghia_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Quản lý vật tư";
            lblTieuDe2.Caption = "Từ khóa cùng nghĩa";
            HideControlInMainContainer();
            if (usr_DanhSachTuCungNghia == null)
            {
                usr_DanhSachTuCungNghia = new usr_DanhSachTuCungNghia();
                usr_DanhSachTuCungNghia.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DanhSachTuCungNghia);
                usr_DanhSachTuCungNghia.BringToFront();
            }
            else
            {
                usr_DanhSachTuCungNghia.Visible = true;
                usr_DanhSachTuCungNghia.BringToFront();
            }
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Trợ giúp";
            lblTieuDe2.Caption = "Đổi mật khẩu";
            HideControlInMainContainer();
            if (usr_DoiMatKhau == null)
            {
                usr_DoiMatKhau = new usr_DoiMatKhau();
                usr_DoiMatKhau.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DoiMatKhau);
                usr_DoiMatKhau.BringToFront();
            }
            else
            {
                usr_DoiMatKhau.Visible = true;
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

        private void frmUser_FormClosing(object sender, FormClosingEventArgs e)
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
