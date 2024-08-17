using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.User
{
    public partial class frmChiTietPhuongAnVatTu_User : Form
    {
        public PhuongAnVatTu pavt = new PhuongAnVatTu();
        List<ChiTietPhuongAn> DS_chitietphuonganvattus = new List<ChiTietPhuongAn>();
        List<ThongTinPhuongAnVatTu> DS_chitietphuonganvattus_Moi = new List<ThongTinPhuongAnVatTu>();
        List<VatTu> DS_vatTus = new List<VatTu>();
        Function function = new Function();
        int sokhoan = 0;

        public frmChiTietPhuongAnVatTu_User(PhuongAnVatTu pavt)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.pavt = pavt;
            dataGridView_DS_CTPAVT.RowTemplate.Height = 30;
            dataGridView_DSVatTu.RowTemplate.Height = 30;
            LoadData();
            LoadData_DSVatTu();
        }

        private void LoadData()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                int maphuongan = pavt.maphuongan;
                var chitiet_pavts = dbContext.ChiTietPhuongAns.Where(m => m.maphuongan == maphuongan).ToList();
                if (chitiet_pavts != null)
                {
                    sokhoan = chitiet_pavts.Count;
                    DS_chitietphuonganvattus = chitiet_pavts;
                    for (int i = 0; i < chitiet_pavts.Count; i++)
                    {
                        dataGridView_DS_CTPAVT.Rows.Add();
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column1"].Value = i + 1;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column2"].Value = chitiet_pavts[i].tenvattu;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column3"].Value = chitiet_pavts[i].donvitinh;
                        string soluong = function.FormatDecimal((Decimal)chitiet_pavts[i].soluong);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column4"].Value = soluong;
                        string doicu = function.FormatDecimal((Decimal)chitiet_pavts[i].doicu);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column5"].Value = doicu;
                        string capmoi = function.FormatDecimal((Decimal)chitiet_pavts[i].capmoi);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column6"].Value = capmoi;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column7"].Value = chitiet_pavts[i].ghichu;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column8"].Value = chitiet_pavts[i].binhluan;
                    }
                }
            }


            txtMaPhuongAn.Text = pavt.maphuongan.ToString();
            txtTenPhuongAn.Text = pavt.tenphuongan;
            txtNguoiLapPhuongAn.Text = pavt.nguoilap;
            try
            {
                DateTime thoigianlap = (DateTime)pavt.thoigianlap;
                txtLanSuaCuoi.Text = thoigianlap.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch { }
            txtTongSoKhoan.Text = sokhoan.ToString();
            txtNguoiDuyetCuoi.Text = pavt.nguoiduyet;
            try
            {
                DateTime thoigianduyet = (DateTime)pavt.thoigianduyet;
                txtThoiGianDuyet.Text = thoigianduyet.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch { }
            txtNoiDungDuyet.Text = pavt.noidungduyet;
        }

        private void LoadData_DSVatTu()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vatTus = dbContext.VatTus.Where(m => m.trangthai == 1).ToList();
                DS_vatTus = vatTus;
                for (int i = 0; i < vatTus.Count; i++)
                {
                    dataGridView_DSVatTu.Rows.Add();
                    dataGridView_DSVatTu.Rows[i].Cells["dtgv2_Column1"].Value = "Xác định";
                    dataGridView_DSVatTu.Rows[i].Cells["dtgv2_Column2"].Value = vatTus[i].tenvattu;
                    dataGridView_DSVatTu.Rows[i].Cells["dtgv2_Column3"].Value = vatTus[i].donvitinh;
                    string madanhmuc = (string)vatTus[i].madanhmuc;
                    DanhMuc danhmuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    if (danhmuc != null)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["dtgv2_Column4"].Value = danhmuc.tendanhmuc;
                    }
                    for (int j = 0; j < DS_chitietphuonganvattus.Count; j++)
                    {
                        if (vatTus[i].mavattu == DS_chitietphuonganvattus[j].mavattu)
                        {
                            ToMauTheoDong(i);
                        }
                    }
                }
            }
        }

        private void dataGridView_DS_CTPAVT_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                int rowID = e.RowIndex;
                // Kiểm tra nếu giá trị của cột cần kiểm tra là số
                if (e.ColumnIndex == 3)
                {
                    // Lấy giá trị mới mà người dùng nhập
                    string newValue = e.FormattedValue.ToString();

                    // Kiểm tra nếu giá trị không phải là số hoặc là số nhưng bé hơn 0
                    if (!int.TryParse(newValue, out int intValue) || intValue < 0)
                    {
                        // Hiển thị thông báo lỗi
                        MessageBox.Show("Cột số lượng phải là số nguyên lớn hơn 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Chuyển ô hiện tại về giá trị trống
                        dataGridView_DS_CTPAVT.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;

                        // Hủy bỏ chỉnh sửa và giữ người dùng trong ô
                        e.Cancel = true;
                    }
                    else
                    {
                        int value = Int32.Parse(newValue);
                        DS_chitietphuonganvattus[rowID].soluong = value;
                    }
                }
                else if (e.ColumnIndex == 4)
                {
                    string newValue = e.FormattedValue.ToString();
                    if (!int.TryParse(newValue, out int intValue) || intValue < 0)
                    {
                        MessageBox.Show("Cột đổi cũ phải là số nguyên không âm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView_DS_CTPAVT.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;
                        e.Cancel = true;
                    }
                    else
                    {
                        int value = Int32.Parse(newValue);
                        DS_chitietphuonganvattus[rowID].doicu = value;
                    }
                }
                else if (e.ColumnIndex == 5)
                {
                    string newValue = e.FormattedValue.ToString();
                    if (!int.TryParse(newValue, out int intValue) || intValue < 0)
                    {
                        MessageBox.Show("Cột cấp mới phải là số nguyên không âm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView_DS_CTPAVT.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;
                        e.Cancel = true;
                    }
                    else
                    {
                        int value = Int32.Parse(newValue);
                        DS_chitietphuonganvattus[rowID].capmoi = value;
                    }
                }
                else if (e.ColumnIndex == 6)
                {
                    string newValue = e.FormattedValue.ToString();
                    DS_chitietphuonganvattus[rowID].ghichu = newValue;
                }
            }
            catch { }
        }

        private void ToMauTheoDong(int rowID)
        {
            try
            {
                var cellValue = dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value?.ToString();

                if (cellValue == "Chọn")
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value = "Không";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }
                    }
                }
                else if (cellValue == "Xác định" || cellValue == "Không")
                {
                    dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value = "Chọn";
                    foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                    {
                        cell.Style.BackColor = Color.LightGreen;
                    }
                }
            }
            catch { }
        }

        private void dataGridView_DSVatTu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTu.Columns["dtgv2_Column1"].Index && e.RowIndex >= 0)
            {
                int rowID = e.RowIndex;
                ToMauTheoDong(rowID);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSVatTu.Rows.Count - 1; i++)
            {
                string tenVatTu = ((string)dataGridView_DSVatTu.Rows[i].Cells["dtgv2_Column2"].Value).ToLower();

                if (tenVatTu.Contains(searchText))
                {
                    dataGridView_DSVatTu.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSVatTu.Rows[i].Visible = false;
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTu.Rows.Count; i++)
                {
                    dataGridView_DSVatTu.Rows[i].Visible = true;
                }
            }
        }
    }
}
