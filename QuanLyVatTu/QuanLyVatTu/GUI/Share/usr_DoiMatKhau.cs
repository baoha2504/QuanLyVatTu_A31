using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DoiMatKhau : UserControl
    {
        Function function = new Function();
        public usr_DoiMatKhau()
        {
            InitializeComponent();
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (int)(panelEx1.Width / 4);
            panelRight.Width = (int)(panelEx1.Width / 4);
            panelTop.Height = (int)(panelEx1.Height / 4.5);
            panelBottom.Height = (int)(panelEx1.Height / 2.5);
            panelLeftText.Width = (int)(groupPanel1.Width / 3);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMatKhauCu.Text != string.Empty && txtMatKhauMoi.Text != string.Empty && txtNhapLaiMatKhauMoi.Text != string.Empty)
            {
                string hash = function.ComputeSHA256(txtMatKhauCu.Text);
                if (hash == frmDangNhap.hashPass)
                {
                    if (txtMatKhauCu.Text != txtMatKhauMoi.Text)
                    {
                        if (txtMatKhauMoi.Text == txtNhapLaiMatKhauMoi.Text)
                        {
                            //bắt đầu đổi mật khẩu
                            using (var dbContext = new QuanLyVatTuDbContext())
                            {
                                DangNhap dangNhap = dbContext.DangNhaps.SingleOrDefault(m => m.tentaikhoan == frmDangNhap.tentaikhoan);
                                dangNhap.matkhau = function.ComputeSHA256(txtMatKhauMoi.Text);
                                dbContext.SaveChanges();

                                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                                lichSuHoatDong.thoigian = DateTime.Now;
                                lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã đổi mật khẩu của họ";
                                lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                                lichSuHoatDong.id = frmDangNhap.userID;
                                dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                                dbContext.SaveChanges();

                                frmDangNhap.hashPass = function.ComputeSHA256(txtMatKhauMoi.Text);

                                MessageBox.Show("Đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            txtNhapLaiMatKhauMoi.Text = string.Empty;
                            MessageBox.Show("Nhập lại mật khẩu mới chưa đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        txtMatKhauCu.Text = string.Empty;
                        txtMatKhauMoi.Text = string.Empty;
                        txtNhapLaiMatKhauMoi.Text = string.Empty;
                        MessageBox.Show("Mật khẩu cũ phải khác mật khẩu mới", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    txtMatKhauCu.Text = string.Empty;
                    MessageBox.Show("Mật khẩu cũ không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Chưa nhập đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
