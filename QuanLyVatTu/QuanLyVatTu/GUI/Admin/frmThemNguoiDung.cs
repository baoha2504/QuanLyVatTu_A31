using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Admin
{
    public partial class frmThemNguoiDung : Form
    {
        Function function = new Function();

        public frmThemNguoiDung()
        {
            InitializeComponent();
            txtID.Enabled = false;
            txtMatKhau.Text = string.Empty;
            txtNgaySua.Text = DateTime.Now.ToString("dd/MM/yyyy"); txtNgaySua.ReadOnly = true;
        }

        public frmThemNguoiDung(int userID, string tennguoidung, string quanham, string chucvu, string tentaikhoan, string trangthai)
        {
            InitializeComponent();
            txtID.Text = userID.ToString(); txtID.ReadOnly = true;
            txtTenNguoiDung.Text = tennguoidung;
            txtQuanHam.Text = quanham;
            txtChucVu.Text = chucvu;
            txtTenTaiKhoan.Text = tentaikhoan; txtTenTaiKhoan.ReadOnly = true;
            txtTrangThai.Text = trangthai;
            txtNgaySua.Text = DateTime.Now.ToString("dd/MM/yyyy"); txtNgaySua.ReadOnly = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    if (this.Text == "Sửa thông tin người dùng")
                    {
                        if (txtMatKhau.Text.Contains("*"))
                        {
                            // không đổi mật khẩu
                            DangNhap dangNhap = dbContext.DangNhaps.SingleOrDefault(m => m.tentaikhoan == txtTenTaiKhoan.Text);
                            if (txtChucVu.Text == "Trưởng phòng" || txtChucVu.Text == "Phó Trưởng phòng")
                            {
                                dangNhap.phanquyen = 1;
                            }
                            else
                            {
                                dangNhap.phanquyen = 2;
                            }

                            if (txtTrangThai.Text == "Hoạt động")
                            {
                                dangNhap.trangthai = 1;
                            }
                            else
                            {
                                dangNhap.trangthai = 0;
                            }
                            dangNhap.nguoisuacuoi = frmDangNhap.tennguoidung;
                            dbContext.SaveChanges();

                            NguoiDung nguoiDung = dbContext.NguoiDungs.SingleOrDefault(m => m.user_id.ToString() == txtID.Text.Trim());
                            nguoiDung.tennguoidung = txtTenNguoiDung.Text;
                            nguoiDung.quanham = txtQuanHam.Text;
                            nguoiDung.chucvu = txtChucVu.Text;
                            nguoiDung.nguoisuacuoi = frmDangNhap.tennguoidung;
                            nguoiDung.thoigiansua = DateTime.Now;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            // đổi mật khẩu
                            DangNhap dangNhap = dbContext.DangNhaps.SingleOrDefault(m => m.tentaikhoan == txtTenTaiKhoan.Text);
                            if (txtChucVu.Text == "Trưởng phòng" || txtChucVu.Text == "Phó Trưởng phòng")
                            {
                                dangNhap.phanquyen = 1;
                            }
                            else
                            {
                                dangNhap.phanquyen = 2;
                            }

                            if (txtTrangThai.Text == "Hoạt động")
                            {
                                dangNhap.trangthai = 1;
                            }
                            else
                            {
                                dangNhap.trangthai = 0;
                            }
                            dangNhap.matkhau = function.ComputeSHA256(txtMatKhau.Text);
                            dangNhap.nguoisuacuoi = frmDangNhap.tennguoidung;
                            dbContext.SaveChanges();

                            NguoiDung nguoiDung = dbContext.NguoiDungs.SingleOrDefault(m => m.user_id.ToString() == txtID.Text.Trim());
                            nguoiDung.tennguoidung = txtTenNguoiDung.Text;
                            nguoiDung.quanham = txtQuanHam.Text;
                            nguoiDung.chucvu = txtChucVu.Text;
                            nguoiDung.nguoisuacuoi = frmDangNhap.tennguoidung;
                            nguoiDung.thoigiansua = DateTime.Now;
                            dbContext.SaveChanges();
                        }

                        LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                        lichSuHoatDong.thoigian = DateTime.Now;
                        lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã sửa thông tin của {txtTenNguoiDung.Text}";
                        lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                        lichSuHoatDong.id = frmDangNhap.userID;
                        dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                        dbContext.SaveChanges();

                        MessageBox.Show("Sửa thông tin người dùng thành công");
                    }
                    else // thêm người dùng
                    {
                        DangNhap dangNhap = dbContext.DangNhaps.SingleOrDefault(m => m.tentaikhoan.ToLower() == txtTenTaiKhoan.Text.ToLower());
                        if (dangNhap == null)
                        {
                            NguoiDung nguoiDung = new NguoiDung();
                            nguoiDung.tennguoidung = txtTenNguoiDung.Text;
                            nguoiDung.quanham = txtQuanHam.Text;
                            nguoiDung.chucvu = txtChucVu.Text;
                            nguoiDung.nguoisuacuoi = frmDangNhap.tennguoidung;
                            nguoiDung.thoigiansua = DateTime.Now;
                            dbContext.NguoiDungs.Add(nguoiDung);
                            dbContext.SaveChanges();

                            DangNhap dn = new DangNhap();
                            dn.tentaikhoan = txtTenTaiKhoan.Text;
                            dn.matkhau = function.ComputeSHA256(txtMatKhau.Text);
                            if (txtChucVu.Text == "Trưởng phòng" || txtChucVu.Text == "Phó Trưởng phòng")
                            {
                                dn.phanquyen = 1;
                            }
                            else
                            {
                                dn.phanquyen = 2;
                            }

                            if (txtTrangThai.Text == "Hoạt động")
                            {
                                dn.trangthai = 1;
                            }
                            else
                            {
                                dn.trangthai = 0;
                            }
                            dn.nguoisuacuoi = frmDangNhap.tennguoidung;
                            dn.user_id = nguoiDung.user_id;
                            dbContext.DangNhaps.Add(dn);
                            dbContext.SaveChanges();

                            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                            lichSuHoatDong.thoigian = DateTime.Now;
                            lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã thêm tài khoản {txtTenNguoiDung.Text}";
                            lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                            lichSuHoatDong.id = frmDangNhap.userID;
                            dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                            dbContext.SaveChanges();

                            MessageBox.Show("Thêm người dùng thành công");
                        }
                        else
                        {
                            MessageBox.Show("Tên tài khoản đã tồn tại");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}");
            }
        }
    }
}
