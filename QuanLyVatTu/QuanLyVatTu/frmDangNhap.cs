using QuanLyVatTu.Class;
using QuanLyVatTu.GUI.Admin;
using QuanLyVatTu.GUI.User;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu
{
    public partial class frmDangNhap : Form
    {
        private ToolTip toolTip;
        Function function = new Function();

        public static int userID;
        public static string tentaikhoan;
        public static string hashPass;
        public static string tennguoidung;
        public static string capbac;
        public static string chucvu;

        public frmDangNhap()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            LoadMatKhau();
            toolTip = new ToolTip();
            toolTip.SetToolTip(btnHienThiMatKhau, "Nhấp để hiển thị/ẩn mật khẩu");

            userID = 0;
            tentaikhoan = string.Empty;
            hashPass = string.Empty;
            tennguoidung = string.Empty;
            capbac = string.Empty;
            chucvu = string.Empty;
        }

        private void LoadMatKhau()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            if (!function.IsFileExists(Path.Combine(projectDirectory, "config.json")))
            {
                projectDirectory = Directory.GetParent(sourceDirectory).FullName;
            }

            GetConfig getConfig = new GetConfig();
            Config config = getConfig.ReadConfig(projectDirectory);
            if (config.login.username != string.Empty && config.login.password != string.Empty)
            {
                txtTenTaiKhoan.Text = config.login.username;
                txtMatKhau.Text = config.login.password;
                btnHienThiMatKhau.Enabled = false;
            }
        }

        private void btnHienThiMatKhau_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '\0')
            {
                txtMatKhau.PasswordChar = '*';
            }
            else
            {
                txtMatKhau.PasswordChar = '\0';
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenTaiKhoan.Text != string.Empty && txtMatKhau.Text != string.Empty)
                {
                    using (var dbContext = new QuanLyVatTuDbContext())
                    {
                        string password = function.ComputeSHA256(txtMatKhau.Text.ToLower());
                        DangNhap dangNhap = dbContext.DangNhaps.FirstOrDefault(m => m.tentaikhoan == txtTenTaiKhoan.Text.ToLower() && m.matkhau == password);
                        if (dangNhap != null)
                        {
                            if (dangNhap.trangthai == 1)
                            {
                                // đăng nhập thành công
                                string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                                if (!function.IsFileExists(Path.Combine(projectDirectory, "config.json")))
                                {
                                    projectDirectory = Directory.GetParent(sourceDirectory).FullName;
                                }
                                GetConfig getConfig = new GetConfig();
                                if (checkBox.Checked == true)
                                {
                                    getConfig.UpdateLogin(projectDirectory, txtTenTaiKhoan.Text, txtMatKhau.Text);
                                }
                                else
                                {
                                    getConfig.UpdateLogin(projectDirectory, "", "");
                                }
                                NguoiDung nguoiDung = dbContext.NguoiDungs.FirstOrDefault(m => m.user_id == dangNhap.user_id);
                                userID = (int)dangNhap.user_id;
                                tentaikhoan = dangNhap.tentaikhoan;
                                hashPass = password;
                                tennguoidung = nguoiDung.tennguoidung;
                                capbac = nguoiDung.quanham;
                                chucvu = nguoiDung.chucvu;
                                if (dangNhap.phanquyen == 1)
                                {
                                    LichSuDangNhap lichSuDangNhap = new LichSuDangNhap();
                                    lichSuDangNhap.thoigian = DateTime.Now;
                                    lichSuDangNhap.trangthai = $"Tài khoản {nguoiDung.quanham} {tennguoidung} - {nguoiDung.chucvu} đăng nhập thành công";
                                    lichSuDangNhap.tennguoidung = tennguoidung;
                                    lichSuDangNhap.user_id = userID;
                                    dbContext.LichSuDangNhaps.Add(lichSuDangNhap); 
                                    dbContext.SaveChanges();

                                    frmAdmin frmAdmin = new frmAdmin();
                                    frmAdmin.lblNguoiDung.Caption = $"{nguoiDung.quanham} {nguoiDung.tennguoidung} - {nguoiDung.chucvu}";
                                    frmAdmin.Show();
                                    this.Hide();
                                }
                                else if (dangNhap.phanquyen == 2)
                                {
                                    LichSuDangNhap lichSuDangNhap = new LichSuDangNhap();
                                    lichSuDangNhap.thoigian = DateTime.Now;
                                    lichSuDangNhap.trangthai = $"Tài khoản {nguoiDung.quanham} {tennguoidung} - {nguoiDung.chucvu} đăng nhập thành công";
                                    lichSuDangNhap.tennguoidung = tennguoidung;
                                    lichSuDangNhap.user_id = userID;
                                    dbContext.LichSuDangNhaps.Add(lichSuDangNhap);
                                    dbContext.SaveChanges();

                                    frmUser frmUser = new frmUser();
                                    frmUser.lblNguoiDung.Caption = $"{nguoiDung.quanham} {nguoiDung.tennguoidung} - {nguoiDung.chucvu}";
                                    frmUser.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                // tài khoản bị khóa
                                LichSuDangNhap lichSuDangNhap = new LichSuDangNhap();
                                lichSuDangNhap.thoigian = DateTime.Now;
                                lichSuDangNhap.trangthai = $"Tài khoản {(int)dangNhap.user_id} đăng nhập không thành công";
                                lichSuDangNhap.user_id = (int)dangNhap.user_id;
                                dbContext.LichSuDangNhaps.Add(lichSuDangNhap);
                                dbContext.SaveChanges();

                                txtThongBao.Visible = true;
                                txtThongBao.ForeColor = Color.OrangeRed;
                                txtThongBao.Text = "Tài khoản đã bị khóa";
                            }

                        }
                        else
                        {
                            // đăng nhập không thành công
                            txtThongBao.Visible = true;
                            txtThongBao.ForeColor = Color.Red;
                            txtThongBao.Text = "Sai tên tài khoản hoặc mật khẩu";
                        }
                    }
                }
                else
                {
                    txtThongBao.Visible = true;
                    txtThongBao.ForeColor = Color.OrangeRed;
                    txtThongBao.Text = "Hãy nhập đầy đủ tài khoản và mật khẩu";
                }
            }
            catch (Exception ex)
            {
                txtThongBao.Visible = true;
                txtThongBao.ForeColor = Color.Red;
                txtThongBao.Text = $"Lỗi: {ex.ToString()}";
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnHienThiMatKhau_MouseEnter(object sender, EventArgs e)
        {
            btnHienThiMatKhau.BackColor = Color.DodgerBlue;
        }

        private void btnHienThiMatKhau_MouseLeave(object sender, EventArgs e)
        {
            btnHienThiMatKhau.BackColor = Color.White;
        }

        private void txtTenTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
            }
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
            }
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            if (txtMatKhau.Text == string.Empty)
            {
                btnHienThiMatKhau.Enabled = true;
            }
        }
    }
}