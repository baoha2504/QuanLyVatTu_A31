using QuanLyVatTu.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Admin
{
    public partial class usr_DanhSachTaiKhoan : UserControl
    {
        public usr_DanhSachTaiKhoan()
        {
            InitializeComponent();
            dataGridView_DSTaiKhoan.RowTemplate.Height = 35;
            LoadTaiKhoan();
        }

        private void LoadTaiKhoan()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var dangnhap = dbContext.DangNhaps.ToList();
                for (int i = 0; i < dangnhap.Count; i++)
                {
                    int id = (int)dangnhap[i].user_id;
                    NguoiDung nguoiDung = dbContext.NguoiDungs.SingleOrDefault(m => m.user_id == id);
                    dataGridView_DSTaiKhoan.Rows.Add();
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column1"].Value = dangnhap[i].user_id;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column2"].Value = nguoiDung.tennguoidung;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column3"].Value = nguoiDung.quanham;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column4"].Value = nguoiDung.chucvu;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column5"].Value = dangnhap[i].tentaikhoan;
                    if (dangnhap[i].phanquyen == 1)
                    {
                        dataGridView_DSTaiKhoan.Rows[i].Cells["Column6"].Value = "Chỉ huy phòng";
                    }
                    else
                    {
                        dataGridView_DSTaiKhoan.Rows[i].Cells["Column6"].Value = "Trợ lý";
                    }

                    if (dangnhap[i].trangthai == 1)
                    {
                        dataGridView_DSTaiKhoan.Rows[i].Cells["Column7"].Value = "Hoạt động";
                    }
                    else
                    {
                        dataGridView_DSTaiKhoan.Rows[i].Cells["Column7"].Value = "Khóa";
                    }
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column8"].Value = nguoiDung.nguoisuacuoi;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column9"].Value = "Sửa ▼";
                }
            }
        }

        private void dataGridView_DSTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSTaiKhoan.Columns["Column9"].Index && e.RowIndex >= 0)
            {
                try
                {
                    // Lấy ID của dòng được chọn
                    int rowID = (int)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column1"].Value;
                    string tennguoidung = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column2"].Value;
                    string quanham = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column3"].Value;
                    string chucvu = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column4"].Value;
                    string tentaikhoan = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column5"].Value;
                    string trangthai = (string)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column7"].Value;

                    frmThemNguoiDung frmThemNguoiDung = new frmThemNguoiDung(rowID, tennguoidung, quanham, chucvu, tentaikhoan, trangthai);
                    frmThemNguoiDung.Text = "Sửa thông tin người dùng";
                    frmThemNguoiDung.ShowDialog();
                    btnLamMoi_Click(sender, e);
                }
                catch { }
            }
        }

        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            frmThemNguoiDung frmThemNguoiDung = new frmThemNguoiDung();
            frmThemNguoiDung.ShowDialog();
            btnLamMoi_Click(sender, e);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dataGridView_DSTaiKhoan.Rows.Clear();
            LoadTaiKhoan();
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ txtNoiDungTimKiem
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            // Lặp qua tất cả các dòng trong DataGridView
            for (int i = 0; i < dataGridView_DSTaiKhoan.Rows.Count - 1; i++)
            {
                // Lấy giá trị của cột "Tên người dùng"
                string tenNguoiDung = ((string)dataGridView_DSTaiKhoan.Rows[i].Cells["Column2"].Value).ToLower();

                // Kiểm tra nếu tên người dùng chứa ký tự tìm kiếm
                if (tenNguoiDung.Contains(searchText))
                {
                    // Hiện dòng nếu tên người dùng chứa ký tự tìm kiếm
                    dataGridView_DSTaiKhoan.Rows[i].Visible = true;
                }
                else
                {
                    // Ẩn dòng nếu tên người dùng không chứa ký tự tìm kiếm
                    dataGridView_DSTaiKhoan.Rows[i].Visible = false;
                }
            }
        }


        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSTaiKhoan.Rows.Count; i++)
                {
                    dataGridView_DSTaiKhoan.Rows[i].Visible = true;
                }
            }
        }

        private void usr_DanhSachTaiKhoan_Resize(object sender, EventArgs e)
        {
            dataGridView_DSTaiKhoan.Columns["Column1"].Width = (int)(dataGridView_DSTaiKhoan.Width * 2 / 43);
            dataGridView_DSTaiKhoan.Columns["Column2"].Width = (int)(dataGridView_DSTaiKhoan.Width * 7 / 43);
            dataGridView_DSTaiKhoan.Columns["Column3"].Width = (int)(dataGridView_DSTaiKhoan.Width * 5 / 43);
            dataGridView_DSTaiKhoan.Columns["Column4"].Width = (int)(dataGridView_DSTaiKhoan.Width * 5 / 43);
            dataGridView_DSTaiKhoan.Columns["Column5"].Width = (int)(dataGridView_DSTaiKhoan.Width * 5 / 43);
            dataGridView_DSTaiKhoan.Columns["Column6"].Width = (int)(dataGridView_DSTaiKhoan.Width * 4 / 43);
            dataGridView_DSTaiKhoan.Columns["Column7"].Width = (int)(dataGridView_DSTaiKhoan.Width * 4 / 43);
            dataGridView_DSTaiKhoan.Columns["Column8"].Width = (int)(dataGridView_DSTaiKhoan.Width * 5 / 43);
            dataGridView_DSTaiKhoan.Columns["Column9"].Width = (int)(dataGridView_DSTaiKhoan.Width * 4 / 43);
        }
    }
}
