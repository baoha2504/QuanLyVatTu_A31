using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmThemVatTu : Form
    {
        Function function = new Function();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        private System.Timers.Timer typingTimer;
        private const int TypingDelay = 1000;

        public frmThemVatTu()
        {
            InitializeComponent();
            this.MaximizeBox = false;

            typingTimer = new System.Timers.Timer(TypingDelay);
            typingTimer.Elapsed += OnTypingTimerElapsed;
            typingTimer.AutoReset = false;

            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var danhMucs = dbContext.DanhMucs.ToList();
                listdanhmuc = danhMucs;
                txtDanhMuc.Items.Clear();
                foreach (var dm in danhMucs)
                {
                    txtDanhMuc.Items.Add(dm.tendanhmuc);
                }
            }
            txtTinhTrang.Text = "Đang sử dụng";
            txtNguoiSuaCuoi.Text = frmDangNhap.tennguoidung;
            txtThoiGianSua.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public frmThemVatTu(VatTu vatTu)
        {
            InitializeComponent();
            this.MaximizeBox = false;

            typingTimer = new System.Timers.Timer(TypingDelay);
            typingTimer.Elapsed += OnTypingTimerElapsed;
            typingTimer.AutoReset = false;

            txtMaVatTu.Text = vatTu.mavattu.ToString();
            txtTenVatTu.Text = vatTu.tenvattu;
            txtDonViTinh.Text = vatTu.donvitinh;
            txtDonGia.Text = function.FormatDecimal((decimal)vatTu.dongia);
            txtNguonGoc.Text = vatTu.nguongoc;
            txtThongSoKyThuat.Text = vatTu.thongsokythuat;
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var danhMucs = dbContext.DanhMucs.ToList();
                listdanhmuc = danhMucs;
                txtDanhMuc.Items.Clear();
                foreach (var dm in danhMucs)
                {
                    txtDanhMuc.Items.Add(dm.tendanhmuc);
                }

                DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == vatTu.madanhmuc);
                txtDanhMuc.Text = danhMuc.tendanhmuc;
            }
            if (vatTu.trangthai == 0)
            {
                txtTinhTrang.Text = "Dừng sử dụng";
            }
            else if (vatTu.trangthai == 1)
            {
                txtTinhTrang.Text = "Đang sử dụng";
            }
            else if (vatTu.trangthai == 2)
            {
                txtTinhTrang.Text = "Bị trùng";
            }

            txtGhiChu.Text = vatTu.ghichu;
            txtNguoiSuaCuoi.Text = frmDangNhap.tennguoidung;
            txtThoiGianSua.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private bool CheckEmptyInput()
        {
            return !string.IsNullOrWhiteSpace(txtTenVatTu.Text) &&
                   !string.IsNullOrWhiteSpace(txtDonViTinh.Text) &&
                   !string.IsNullOrWhiteSpace(txtDanhMuc.Text) &&
                   !string.IsNullOrWhiteSpace(txtTinhTrang.Text);
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (this.Text != "Sửa thông tin vật tư")
            {
                if (CheckEmptyInput())
                {
                    using (var dbContext = new QuanLyVatTuDbContext())
                    {
                        // Thêm thông tin vật tư
                        VatTu vt = new VatTu();
                        vt.tenvattu = txtTenVatTu.Text;
                        vt.donvitinh = txtDonViTinh.Text;
                        try
                        {
                            vt.dongia = function.ConvertStringToDecimal(txtDonGia.Text);
                        }
                        catch
                        {
                            vt.dongia = 0;
                        }

                        vt.nguongoc = txtNguonGoc.Text;
                        vt.thongsokythuat = txtThongSoKyThuat.Text;
                        if (txtTinhTrang.Text == "Đang sử dụng")
                        {
                            vt.trangthai = 1;
                        }
                        else if (txtTinhTrang.Text == "Dừng sử dụng")
                        {
                            vt.trangthai = 0;
                        }
                        else if (txtTinhTrang.Text == "Bị trùng")
                        {
                            vt.trangthai = 2;
                        }
                        vt.ghichu = txtGhiChu.Text;
                        vt.nguoisuacuoi = frmDangNhap.tennguoidung;
                        vt.thoigiansua = DateTime.Now;
                        vt.user_id = frmDangNhap.userID;
                        DanhMuc dm = dbContext.DanhMucs.SingleOrDefault(m => m.tendanhmuc == txtDanhMuc.Text);
                        vt.madanhmuc = dm.madanhmuc;
                        dbContext.VatTus.Add(vt);
                        dbContext.SaveChanges();

                        LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                        lichSuHoatDong.thoigian = DateTime.Now;
                        lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} thêm vật tư mới {vt.mavattu} - {vt.tenvattu}";
                        lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                        lichSuHoatDong.id = frmDangNhap.userID;
                        dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                        dbContext.SaveChanges();

                        MessageBox.Show("Thêm vật tư mới thành công");
                    }
                }
                else
                {
                    MessageBox.Show("Hãy nhập đầy đủ các thông tin bắt buộc đánh dấu '*'", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Sửa thông tin vật tư
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    int mavattu = Int32.Parse(txtMaVatTu.Text);
                    VatTu vt = dbContext.VatTus.SingleOrDefault(m => m.mavattu == mavattu);
                    vt.tenvattu = txtTenVatTu.Text;
                    vt.donvitinh = txtDonViTinh.Text;
                    try
                    {
                        vt.dongia = function.ConvertStringToDecimal(txtDonGia.Text);
                    }
                    catch
                    {
                        vt.dongia = 0;
                    }

                    vt.nguongoc = txtNguonGoc.Text;
                    vt.thongsokythuat = txtThongSoKyThuat.Text;
                    if (txtTinhTrang.Text == "Đang sử dụng")
                    {
                        vt.trangthai = 1;
                    }
                    else if (txtTinhTrang.Text == "Dừng sử dụng")
                    {
                        vt.trangthai = 0;
                    }
                    else if (txtTinhTrang.Text == "Bị trùng")
                    {
                        vt.trangthai = 2;
                    }
                    vt.ghichu = txtGhiChu.Text;
                    vt.nguoisuacuoi = frmDangNhap.tennguoidung;
                    vt.thoigiansua = DateTime.Now;
                    vt.user_id = frmDangNhap.userID;
                    DanhMuc dm = dbContext.DanhMucs.SingleOrDefault(m => m.tendanhmuc == txtDanhMuc.Text);
                    vt.madanhmuc = dm.madanhmuc;
                    dbContext.SaveChanges();

                    LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                    lichSuHoatDong.thoigian = DateTime.Now;
                    lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} sửa thông tin vật tư {vt.mavattu} - {vt.tenvattu}";
                    lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                    lichSuHoatDong.id = frmDangNhap.userID;
                    dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                    dbContext.SaveChanges();

                    MessageBox.Show("Sửa thông tin vật tư thành công");
                }

            }
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
