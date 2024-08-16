using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmChiTietVatTuMoi : Form
    {
        public frmChiTietVatTuMoi()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        public NewVatTuFromExcel vtmExcel = new NewVatTuFromExcel();
        Function function = new Function();
        private System.Timers.Timer typingTimer;
        private const int TypingDelay = 1000;

        public frmChiTietVatTuMoi(NewVatTuFromExcel vatTuMoiExcel)
        {
            InitializeComponent();
            this.MaximizeBox = false;

            typingTimer = new System.Timers.Timer(TypingDelay);
            typingTimer.Elapsed += OnTypingTimerElapsed;
            typingTimer.AutoReset = false;

            vtmExcel = vatTuMoiExcel;
            LoadData();
        }

        private void LoadData()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var danhMucs = dbContext.DanhMucs.ToList();
                foreach (var dm in danhMucs)
                {
                    txtDanhMuc.Items.Add(dm.tendanhmuc);
                }
            }

            txtTenVatTu.Text = vtmExcel.tenvattu;
            txtDonViTinh.Text = vtmExcel.donvitinh;
            string dongia = function.FormatDecimal(vtmExcel.dongia);
            txtDonGia.Text = dongia;
            txtNguonGoc.Text = vtmExcel.nguongoc;
            txtDanhMuc.Text = vtmExcel.tendanhmuc;
            txtGhiChu.Text = vtmExcel.ghichu;
            txtNguoiSuaCuoi.Text = frmDangNhap.tennguoidung;
            txtThoiGianSua.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtThongSoKyThuat.Text = vtmExcel.thongsokythuat;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            vtmExcel.tenvattu = txtTenVatTu.Text;
            vtmExcel.donvitinh = txtDonViTinh.Text;
            try
            {
                decimal dongia = function.ConvertStringToDecimal(txtDonGia.Text);
                vtmExcel.dongia = dongia;
            }
            catch { vtmExcel.dongia = 0; }
            vtmExcel.nguongoc = txtNguonGoc.Text;
            vtmExcel.tendanhmuc = txtDanhMuc.Text;
            vtmExcel.ghichu = txtGhiChu.Text;
            txtThongSoKyThuat.Text = vtmExcel.thongsokythuat;

            //cập nhật xong thì đóng form
            this.Close();
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {
            typingTimer.Stop();
            typingTimer.Start();
        }

        private void OnTypingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(ExecuteFunctionAfterTyping));
        }

        private void ExecuteFunctionAfterTyping()
        {
            try
            {
                txtDonGia.Text = function.FormatDecimal(Decimal.Parse(txtDonGia.Text));
            }
            catch { }
        }
    }
}
