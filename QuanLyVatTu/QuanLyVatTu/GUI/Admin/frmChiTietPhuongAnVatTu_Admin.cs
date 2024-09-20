using MiniSoftware;
using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Admin
{
    public partial class frmChiTietPhuongAnVatTu_Admin : Form
    {
        public PhuongAnVatTu pavt = new PhuongAnVatTu();
        List<ChiTietPhuongAn> DS_chitietphuonganvattus = new List<ChiTietPhuongAn>();
        Function function = new Function();
        int sokhoan = 0;

        public frmChiTietPhuongAnVatTu_Admin(PhuongAnVatTu pavt)
        {
            InitializeComponent();
            this.pavt = pavt;
            dataGridView_DS_CTPAVT.RowTemplate.Height = 35;
            LoadData();
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
            if (pavt.hoanthanh == 1)
            {
                swTrangThaiPhuongAn.IsOn = true;
            }
            else
            {
                swTrangThaiPhuongAn.IsOn = false;
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
                else if (e.ColumnIndex == 7)
                {
                    string newValue = e.FormattedValue.ToString();
                    DS_chitietphuonganvattus[rowID].binhluan = newValue;
                }
            }
            catch { }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    int maphuongan = Int32.Parse(txtMaPhuongAn.Text);
                    PhuongAnVatTu p = dbContext.PhuongAnVatTus.SingleOrDefault(m => m.maphuongan == maphuongan);
                    p.nguoiduyet = frmDangNhap.tennguoidung;
                    p.thoigianduyet = DateTime.Now;
                    p.noidungduyet = txtNoiDungDuyet.Text;
                    if (swTrangThaiPhuongAn.IsOn == true)
                    {
                        p.hoanthanh = 1;
                    }
                    else
                    {
                        p.hoanthanh = 0;
                    }

                    var chitiet_pavts = dbContext.ChiTietPhuongAns.Where(m => m.maphuongan == maphuongan).ToList();
                    if (chitiet_pavts != null)
                    {
                        for (int i = 0; i < chitiet_pavts.Count; i++)
                        {
                            chitiet_pavts[i].soluong = DS_chitietphuonganvattus[i].soluong;
                            chitiet_pavts[i].doicu = DS_chitietphuonganvattus[i].doicu;
                            chitiet_pavts[i].capmoi = DS_chitietphuonganvattus[i].capmoi;
                            chitiet_pavts[i].ghichu = DS_chitietphuonganvattus[i].ghichu;
                            chitiet_pavts[i].binhluan = DS_chitietphuonganvattus[i].binhluan;
                        }
                    }

                    LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                    lichSuHoatDong.thoigian = DateTime.Now;
                    lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} duyệt phương án vật tư #{maphuongan}";
                    lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                    lichSuHoatDong.id = frmDangNhap.userID;
                    dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                    dbContext.SaveChanges();
                }
                MessageBox.Show("Cập nhật nội dung duyệt thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (!function.IsFileExists(Path.Combine(projectDirectory, "template.docx")))
                    {
                        projectDirectory = Directory.GetParent(sourceDirectory).FullName;
                    }
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "template.docx");
                    //string PATH_TEMPLATE = Application.StartupPath + "\\mauword.docx";


                    if (File.Exists(PATH_TEMPLATE))
                    {
                        List<ChiTietPhuongAnVatTu_Print> chitiets = new List<ChiTietPhuongAnVatTu_Print>();
                        for (int i = 0; i < DS_chitietphuonganvattus.Count; i++)
                        {
                            int dem = 0;
                            ChiTietPhuongAnVatTu_Print ct = new ChiTietPhuongAnVatTu_Print();
                            ct.stt = (i + 1).ToString();
                            ct.tenvattu = DS_chitietphuonganvattus[i].tenvattu;
                            ct.donvitinh = DS_chitietphuonganvattus[i].donvitinh;
                            if ((decimal)DS_chitietphuonganvattus[i].soluong != 0)
                            {
                                ct.soluong = ((decimal)DS_chitietphuonganvattus[i].soluong).ToString("0");
                            }
                            else { dem++; }
                            if ((decimal)DS_chitietphuonganvattus[i].doicu != 0)
                            {
                                ct.doicu = ((decimal)DS_chitietphuonganvattus[i].doicu).ToString("0");
                            }
                            else { dem++; }
                            if ((decimal)DS_chitietphuonganvattus[i].capmoi != 0)
                            {
                                ct.capmoi = ((decimal)DS_chitietphuonganvattus[i].capmoi).ToString("0");
                            }
                            else { dem++; }
                            ct.ghichu = DS_chitietphuonganvattus[i].ghichu;
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
                            capbac = "",
                            hoten = "",
                            sokhoan = DS_chitietphuonganvattus.Count(),
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

        private void frmChiTietPhuongAnVatTu_Admin_Resize(object sender, EventArgs e)
        {
            panel4.Width = (int)(panel2.Width / 2);
            panel3.Height = panel1.Height - panel2.Height - 60;
            panel32.Width = (int)(panel7.Width / 3.5);

            dataGridView_DS_CTPAVT.Columns["Column1"].Width = (int)(dataGridView_DS_CTPAVT.Width * 1 / 26);
            dataGridView_DS_CTPAVT.Columns["Column2"].Width = (int)(dataGridView_DS_CTPAVT.Width * 6 / 26);
            dataGridView_DS_CTPAVT.Columns["Column3"].Width = (int)(dataGridView_DS_CTPAVT.Width * 3 / 26);
            dataGridView_DS_CTPAVT.Columns["Column4"].Width = (int)(dataGridView_DS_CTPAVT.Width * 2 / 26);
            dataGridView_DS_CTPAVT.Columns["Column5"].Width = (int)(dataGridView_DS_CTPAVT.Width * 2 / 26);
            dataGridView_DS_CTPAVT.Columns["Column6"].Width = (int)(dataGridView_DS_CTPAVT.Width * 2 / 26);
            dataGridView_DS_CTPAVT.Columns["Column7"].Width = (int)(dataGridView_DS_CTPAVT.Width * 4 / 26);
            dataGridView_DS_CTPAVT.Columns["Column8"].Width = (int)(dataGridView_DS_CTPAVT.Width * 5 / 26);
        }
    }
}
