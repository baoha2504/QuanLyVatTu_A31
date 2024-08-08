using QuanLyVatTu.Class;
using QuanLyVatTu.Support;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyVatTu
{
    public partial class frmDangNhap : Form
    {
        private ToolTip toolTip;

        public frmDangNhap()
        {
            InitializeComponent();
            LoadMatKhau();
            toolTip = new ToolTip();
            toolTip.SetToolTip(btnHienThiMatKhau, "Nhấp để hiển thị/ẩn mật khẩu");
        }

        private void LoadMatKhau()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
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
            // đăng nhập thành công
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            GetConfig getConfig = new GetConfig();
            if (checkBox.Checked == true)
            {
                getConfig.UpdateLogin(projectDirectory, txtTenTaiKhoan.Text, txtMatKhau.Text);
            }
            else
            {
                getConfig.UpdateLogin(projectDirectory, "", "");
            }

            // đăng nhập không thành công
        }

        private void btnHienThiMatKhau_MouseEnter(object sender, EventArgs e)
        {
            btnHienThiMatKhau.BackColor = Color.DodgerBlue;
        }

        private void btnHienThiMatKhau_MouseLeave(object sender, EventArgs e)
        {
            btnHienThiMatKhau.BackColor = Color.White;
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

        private void ThongBaoDangNhap(string loai, string text)
        {
            if (loai == "warning")
            {
                txtThongBao.Visible = true;
                txtThongBao.Text = text;
            }
            else if (loai == "error")
            {
                txtThongBao.Visible = true;
                txtThongBao.Text = text;
            }
        }
    }
}