using QuanLyVatTu.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmThemDanhMuc : Form
    {
        public frmThemDanhMuc()
        {
            InitializeComponent();
            txtThoiGian.Text = DateTime.Now.ToString("dd/MM/yyyy"); txtThoiGian.Enabled = false;
        }

        public frmThemDanhMuc(string madanhmuc, string tendanhmuc)
        {
            InitializeComponent();
            txtMaDanhMuc.Text = madanhmuc; txtMaDanhMuc.Enabled = false;
            txtTenDanhMuc.Text = tendanhmuc;
            txtThoiGian.Text = DateTime.Now.ToString("dd/MM/yyyy"); txtThoiGian.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (this.Text == "Sửa thông tin danh mục")
            {
                // sửa danh mục
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == txtMaDanhMuc.Text);
                    danhMuc.tendanhmuc = txtTenDanhMuc.Text;
                    danhMuc.nguoisuacuoi = frmDangNhap.tennguoidung;
                    danhMuc.thoigiansua = DateTime.Now;
                    danhMuc.user_id = frmDangNhap.userID;
                    dbContext.SaveChanges();
                    MessageBox.Show("Sửa thông tin danh mục thành công");

                    LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                    lichSuHoatDong.thoigian = DateTime.Now;
                    lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã sửa thông tin danh mục {txtMaDanhMuc.Text}";
                    lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                    lichSuHoatDong.id = frmDangNhap.userID;
                    dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                    dbContext.SaveChanges();
                }
            }
            else
            {
                // thêm danh mục
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc.ToLower() == txtMaDanhMuc.Text.ToLower());
                    if (danhMuc == null)
                    {
                        DanhMuc dm = new DanhMuc();
                        dm.madanhmuc = txtMaDanhMuc.Text.ToUpper();
                        dm.tendanhmuc = txtTenDanhMuc.Text;
                        dm.nguoisuacuoi = frmDangNhap.tennguoidung;
                        dm.thoigiansua = DateTime.Now;
                        dm.user_id = frmDangNhap.userID;
                        dbContext.DanhMucs.Add(dm);
                        dbContext.SaveChanges();
                        MessageBox.Show("Thêm danh mục thành công");

                        LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                        lichSuHoatDong.thoigian = DateTime.Now;
                        lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã thêm thông tin danh mục {txtMaDanhMuc.Text}";
                        lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                        lichSuHoatDong.id = frmDangNhap.userID;
                        dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Mã danh mục đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
