using MiniSoftware;
using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.User
{
    public partial class frmChiTietPhuongAnVatTu_User : Form
    {
        public PhuongAnVatTu pavt = new PhuongAnVatTu();
        List<VatTu> DS_vattus = new List<VatTu>();
        List<ChiTietPhuongAn> DS_chitietphuonganvattus_BanDau = new List<ChiTietPhuongAn>();
        List<ChiTietPhuongAn> DS_chitietphuonganvattus = new List<ChiTietPhuongAn>(); // dùng cái này để lưu
        List<ChiTietPhuongAn> DS_chitietphuonganvattus_DaXoa = new List<ChiTietPhuongAn>(); // dùng cái này để lưu
        List<ThongTinPhuongAnVatTu> DS_thongtinphuonganvattus_MoiThem = new List<ThongTinPhuongAnVatTu>(); // dùng cái này để lưu
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        Function function = new Function();
        int sokhoan = 0;
        int solanload = 0;

        public frmChiTietPhuongAnVatTu_User(PhuongAnVatTu pavt)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.pavt = pavt;
            dataGridView_DS_CTPAVT.RowTemplate.Height = 30;
            dataGridView_DSVatTu.RowTemplate.Height = 30;
            LoadData();
            LoadData_DSVatTu();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                listdanhmuc = dbContext.DanhMucs.OrderBy(m => m.tendanhmuc).ToList();
                foreach (var dm in listdanhmuc)
                {
                    cbbDanhMuc.Items.Add(dm.tendanhmuc);
                }
                cbbDanhMuc.Items.Add("Tất cả danh mục");
            }
        }

        private void LoadData()
        {
            dataGridView_DS_CTPAVT.Rows.Clear();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                int maphuongan = pavt.maphuongan;
                var chitiet_pavts = dbContext.ChiTietPhuongAns.Where(m => m.maphuongan == maphuongan).OrderBy(m => m.tenvattu).ToList();
                if (chitiet_pavts != null)
                {
                    solanload++;
                    DS_chitietphuonganvattus_BanDau = chitiet_pavts;
                    if (solanload == 1)
                    {
                        DS_chitietphuonganvattus = chitiet_pavts;
                    }
                    sokhoan = DS_chitietphuonganvattus.Count + DS_thongtinphuonganvattus_MoiThem.Count;

                    for (int i = 0; i < DS_chitietphuonganvattus.Count; i++)
                    {
                        dataGridView_DS_CTPAVT.Rows.Add();
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column1"].Value = i + 1;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column2"].Value = DS_chitietphuonganvattus[i].tenvattu;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column3"].Value = DS_chitietphuonganvattus[i].donvitinh;
                        string soluong = function.FormatDecimal((Decimal)DS_chitietphuonganvattus[i].soluong);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column4"].Value = soluong;
                        string doicu = function.FormatDecimal((Decimal)DS_chitietphuonganvattus[i].doicu);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column5"].Value = doicu;
                        string capmoi = function.FormatDecimal((Decimal)DS_chitietphuonganvattus[i].capmoi);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column6"].Value = capmoi;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column7"].Value = DS_chitietphuonganvattus[i].ghichu;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column8"].Value = DS_chitietphuonganvattus[i].binhluan;
                    }
                    for (int i = DS_chitietphuonganvattus.Count; i < (DS_chitietphuonganvattus.Count + DS_thongtinphuonganvattus_MoiThem.Count); i++)
                    {
                        int j = i - DS_chitietphuonganvattus.Count;
                        dataGridView_DS_CTPAVT.Rows.Add();
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column1"].Value = i + 1;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column2"].Value = DS_thongtinphuonganvattus_MoiThem[j].tenvattu;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column3"].Value = DS_thongtinphuonganvattus_MoiThem[j].donvitinh;
                        string soluong = function.FormatDecimal((Decimal)DS_thongtinphuonganvattus_MoiThem[j].soluong);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column4"].Value = soluong;
                        string doicu = function.FormatDecimal((Decimal)DS_thongtinphuonganvattus_MoiThem[j].doicu);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column5"].Value = doicu;
                        string capmoi = function.FormatDecimal((Decimal)DS_thongtinphuonganvattus_MoiThem[j].capmoi);
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column6"].Value = capmoi;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column7"].Value = DS_thongtinphuonganvattus_MoiThem[j].ghichu;
                        dataGridView_DS_CTPAVT.Rows[i].Cells["Column8"].Value = DS_thongtinphuonganvattus_MoiThem[j].binhluan;
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
            if (pavt.hoanthanh == 1)
            {
                swTrangThaiPhuongAn.IsOn = true;
            }
            else
            {
                swTrangThaiPhuongAn.IsOn = false;
            }
        }

        private void LoadData_DSVatTu()
        {
            dataGridView_DSVatTu.Rows.Clear();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vatTus = dbContext.VatTus.Where(m => m.trangthai == 1).OrderBy(m => m.tenvattu).ToList();
                DS_vattus = vatTus;
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
                            ToMauTheoDong(i, "bandau");
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
                        if (rowID < DS_chitietphuonganvattus.Count)
                        {
                            DS_chitietphuonganvattus[rowID].soluong = value;
                        }
                        else
                        {
                            int newID = rowID - DS_chitietphuonganvattus.Count;
                            DS_thongtinphuonganvattus_MoiThem[newID].soluong = value;
                        }
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
                        if (rowID < DS_chitietphuonganvattus.Count)
                        {
                            DS_chitietphuonganvattus[rowID].doicu = value;
                        }
                        else
                        {
                            int newID = rowID - DS_chitietphuonganvattus.Count;
                            DS_thongtinphuonganvattus_MoiThem[newID].doicu = value;
                        }
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
                        if (rowID < DS_chitietphuonganvattus.Count)
                        {
                            DS_chitietphuonganvattus[rowID].capmoi = value;
                        }
                        else
                        {
                            int newID = rowID - DS_chitietphuonganvattus.Count;
                            DS_thongtinphuonganvattus_MoiThem[newID].capmoi = value;
                        }
                    }
                }
                else if (e.ColumnIndex == 6)
                {
                    string newValue = e.FormattedValue.ToString();
                    if (rowID < DS_chitietphuonganvattus.Count)
                    {
                        DS_chitietphuonganvattus[rowID].ghichu = newValue;
                    }
                    else
                    {
                        int newID = rowID - DS_chitietphuonganvattus.Count;
                        DS_thongtinphuonganvattus_MoiThem[newID].ghichu = newValue;
                    }
                }
            }
            catch { }
        }

        private void ToMauTheoDong(int rowID, string loai)
        {
            try
            {
                var cellValue = dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value?.ToString();

                if (cellValue == "Chọn")
                {
                    if (dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Style.BackColor == Color.ForestGreen)
                    {
                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value = "Không";
                            foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                            {
                                cell.Style.BackColor = Color.White;
                            }
                            int mavattu = DS_vattus[rowID].mavattu;
                            ChiTietPhuongAn ctpavt = DS_chitietphuonganvattus.SingleOrDefault(m => m.mavattu == mavattu);
                            if (ctpavt != null)
                            {
                                DS_chitietphuonganvattus_DaXoa.Add(ctpavt);
                                DS_chitietphuonganvattus.Remove(ctpavt);
                            }
                        }
                    }
                    else
                    {
                        dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value = "Không";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }
                        int mavattu = DS_vattus[rowID].mavattu;
                        ThongTinPhuongAnVatTu ttpa = DS_thongtinphuonganvattus_MoiThem.SingleOrDefault(m => m.mavattu == mavattu);
                        if (ttpa != null)
                        {
                            DS_thongtinphuonganvattus_MoiThem.Remove(ttpa);
                        }
                    }
                }
                else if (cellValue == "Xác định" || cellValue == "Không")
                {
                    dataGridView_DSVatTu.Rows[rowID].Cells["dtgv2_Column1"].Value = "Chọn";
                    if (loai == "bandau")
                    {
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.ForestGreen;
                        }
                    }
                    else
                    {
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.LightGreen;
                        }
                        ThongTinPhuongAnVatTu ttpa = new ThongTinPhuongAnVatTu();
                        ttpa.mavattu = DS_vattus[rowID].mavattu;
                        ttpa.tenvattu = DS_vattus[rowID].tenvattu;
                        ttpa.donvitinh = DS_vattus[rowID].donvitinh;
                        ttpa.soluong = 0;
                        ttpa.doicu = 0;
                        ttpa.capmoi = 0;
                        ttpa.ghichu = "";
                        ttpa.binhluan = "";

                        DS_thongtinphuonganvattus_MoiThem.Add(ttpa);
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
                ToMauTheoDong(rowID, "");
                LoadData();
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
                cbbDanhMuc_SelectedIndexChanged(sender, e);
            }
        }

        private bool KiemTraGiaTriCuaCot()
        {
            // Lặp qua tất cả các hàng của DataGridView
            foreach (DataGridViewRow row in dataGridView_DS_CTPAVT.Rows)
            {
                if (!row.IsNewRow) // Bỏ qua hàng trống cuối cùng
                {
                    // Lấy giá trị trong cột có ColumnIndex = 3
                    var cellValue = row.Cells[3].Value;

                    // Kiểm tra nếu giá trị là null, rỗng hoặc bằng 0
                    if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString())
                        && int.TryParse(cellValue.ToString(), out int value) && value == 0)
                    {
                        // Hiển thị thông báo lỗi
                        MessageBox.Show("Có một hàng trong cột thứ 3 của bảng phương án vật tư có giá trị bằng 0. Hãy kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Dừng kiểm tra sau khi phát hiện lỗi
                    }
                }
            }
            return true;
        }
        private void LamMoi()
        {
            solanload = 0;
            DS_chitietphuonganvattus.Clear();
            DS_chitietphuonganvattus_DaXoa.Clear();
            DS_thongtinphuonganvattus_MoiThem.Clear();
            LoadData();
            LoadData_DSVatTu();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (KiemTraGiaTriCuaCot())
            {
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    // vật tư phương án bị sửa ==> cập nhật
                    for (int i = 0; i < DS_chitietphuonganvattus.Count; i++)
                    {
                        int id = (int)DS_chitietphuonganvattus[i].id;
                        ChiTietPhuongAn ct = dbContext.ChiTietPhuongAns.SingleOrDefault(m => m.id == id);
                        if (ct != null)
                        {
                            ct.soluong = DS_chitietphuonganvattus[i].soluong;
                            ct.doicu = DS_chitietphuonganvattus[i].doicu;
                            ct.capmoi = DS_chitietphuonganvattus[i].capmoi;
                            ct.ghichu = DS_chitietphuonganvattus[i].ghichu;
                        }
                    }

                    for (int i = 0; i < DS_chitietphuonganvattus_DaXoa.Count; i++)
                    {
                        int id = (int)DS_chitietphuonganvattus_DaXoa[i].id;
                        ChiTietPhuongAn ct = dbContext.ChiTietPhuongAns.SingleOrDefault(m => m.id == id);
                        if (ct != null)
                        {
                            dbContext.ChiTietPhuongAns.Remove(ct);
                        }
                    }

                    //vật tư phương án mới được thêm ==> thêm
                    for (int i = 0; i < DS_thongtinphuonganvattus_MoiThem.Count; i++)
                    {
                        ChiTietPhuongAn ct = new ChiTietPhuongAn();
                        ct.mavattu = DS_thongtinphuonganvattus_MoiThem[i].mavattu;
                        ct.tenvattu = DS_thongtinphuonganvattus_MoiThem[i].tenvattu;
                        ct.donvitinh = DS_thongtinphuonganvattus_MoiThem[i].donvitinh;
                        ct.soluong = DS_thongtinphuonganvattus_MoiThem[i].soluong;
                        ct.doicu = DS_thongtinphuonganvattus_MoiThem[i].doicu;
                        ct.capmoi = DS_thongtinphuonganvattus_MoiThem[i].capmoi;
                        ct.ghichu = DS_thongtinphuonganvattus_MoiThem[i].ghichu;
                        ct.maphuongan = Int32.Parse(txtMaPhuongAn.Text);
                        dbContext.ChiTietPhuongAns.Add(ct);
                    }

                    int maphuongan = Int32.Parse(txtMaPhuongAn.Text);
                    PhuongAnVatTu p = dbContext.PhuongAnVatTus.SingleOrDefault(m => m.maphuongan == maphuongan);
                    p.thoigianlap = DateTime.Now;
                    if(swTrangThaiPhuongAn.IsOn == true)
                    {
                        p.hoanthanh = 1;
                    }
                    else
                    {
                        p.hoanthanh = 0;
                    }

                    LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                    lichSuHoatDong.thoigian = DateTime.Now;
                    lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã sửa phương án {txtMaPhuongAn.Text}";
                    lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                    lichSuHoatDong.id = frmDangNhap.userID;
                    dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                    dbContext.SaveChanges();

                    MessageBox.Show("Lưu thay đổi thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLanSuaCuoi.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                    LamMoi();
                }
            }
        }

        private void btnXuatFileWord_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu file phương án vật tư";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = $"Phương án vật tư #{txtMaPhuongAn.Text}_{DateTime.Now.ToString("HHmmssddMMyyyy")}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "template.docx");
                    //string PATH_TEMPLATE = Application.StartupPath + "\\mauword.docx";


                    if (File.Exists(PATH_TEMPLATE))
                    {
                        List<ChiTietPhuongAnVatTu_Print> chitiets = new List<ChiTietPhuongAnVatTu_Print>();
                        for (int i = 0; i < DS_chitietphuonganvattus_BanDau.Count; i++)
                        {
                            int dem = 0;
                            ChiTietPhuongAnVatTu_Print ct = new ChiTietPhuongAnVatTu_Print();
                            ct.stt = (i + 1).ToString();
                            ct.tenvattu = DS_chitietphuonganvattus_BanDau[i].tenvattu;
                            ct.donvitinh = DS_chitietphuonganvattus_BanDau[i].donvitinh;
                            if ((decimal)DS_chitietphuonganvattus_BanDau[i].soluong != 0)
                            {
                                ct.soluong = ((decimal)DS_chitietphuonganvattus_BanDau[i].soluong).ToString("0");
                            }
                            else { dem++; }
                            if ((decimal)DS_chitietphuonganvattus_BanDau[i].doicu != 0)
                            {
                                ct.doicu = ((decimal)DS_chitietphuonganvattus_BanDau[i].doicu).ToString("0");
                            }
                            else { dem++; }
                            if ((decimal)DS_chitietphuonganvattus_BanDau[i].capmoi != 0)
                            {
                                ct.capmoi = ((decimal)DS_chitietphuonganvattus_BanDau[i].capmoi).ToString("0");
                            }
                            else { dem++; }
                            ct.ghichu = DS_chitietphuonganvattus_BanDau[i].ghichu;
                            if (dem != 3)
                            {
                                chitiets.Add(ct);
                            }
                        }

                        ChiTietPhuongAnVatTu_Print ctcuoi = new ChiTietPhuongAnVatTu_Print();
                        ctcuoi.stt = "*";
                        ctcuoi.tenvattu = $"Tổng {chitiets.Count} khoản";
                        chitiets.Add(ctcuoi);

                        var value = new
                        {
                            ct = chitiets,
                            ngay = DateTime.Now.ToString("dd"),
                            thang = DateTime.Now.ToString("MM"),
                            nam = DateTime.Now.ToString("yyyy"),
                            capbac = frmDangNhap.capbac,
                            hoten = frmDangNhap.tennguoidung,
                            sokhoan = DS_chitietphuonganvattus_BanDau.Count(),
                        };

                        MiniWord.SaveAsByTemplate(PATH_EXPORT, PATH_TEMPLATE, value);

                        Process.Start(PATH_EXPORT);
                        MessageBox.Show("Xuất file phương án vật tư thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("File không tồn tại: " + PATH_EXPORT, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDanhMuc.Text == "Tất cả danh mục")
            {
                for (int i = 0; i < dataGridView_DSVatTu.Rows.Count; i++)
                {
                    dataGridView_DSVatTu.Rows[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView_DSVatTu.Rows.Count - 1; i++)
                {
                    string tenDanhMuc = ((string)dataGridView_DSVatTu.Rows[i].Cells["dtgv2_Column4"].Value).ToLower();

                    if (tenDanhMuc.Contains(cbbDanhMuc.Text.ToLower()))
                    {
                        dataGridView_DSVatTu.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTu.Rows[i].Visible = false;
                    }
                }
            }
        }
    }
}
