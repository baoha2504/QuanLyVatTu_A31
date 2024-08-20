using OfficeOpenXml;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DanhSachVatTu : UserControl
    {
        Function function = new Function();
        List<VatTu> vatTus = new List<VatTu>();
        List<DanhMuc> danhMucs = new List<DanhMuc>();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        public usr_DanhSachVatTu()
        {
            InitializeComponent();
            dataGridView_DSVatTu.RowTemplate.Height = 35;
            LoadDSVatTu();
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

        private void LoadDSVatTu()
        {
            dataGridView_DSVatTu.Rows.Clear();
            danhMucs.Clear();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vattu = new List<VatTu>();
                //if(cbbDanhMuc.Text == "Tất cả danh mục") { }
                //else { }

                if (cbbDanhMuc.Text == "Tất cả danh mục")
                {
                    if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
                    {
                        vattu = dbContext.VatTus.Where(m => m.trangthai == 1).OrderBy(m => m.tenvattu).ToList();
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
                    {
                        vattu = dbContext.VatTus.Where(m => m.trangthai == 0).OrderBy(m => m.tenvattu).ToList();
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
                    {
                        vattu = dbContext.VatTus.Where(m => m.trangthai == 2).OrderBy(m => m.tenvattu).ToList();
                    }
                    else if (groupPanel1.Text == "Danh sách tất cả vật tư")
                    {
                        vattu = dbContext.VatTus.ToList();
                    }
                }
                else
                {
                    DanhMuc dm = listdanhmuc.SingleOrDefault(m => m.tendanhmuc == cbbDanhMuc.Text);
                    string madanhmuc = dm.madanhmuc;
                    if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
                    {
                        vattu = dbContext.VatTus.Where(m => m.trangthai == 1 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
                    {
                        vattu = dbContext.VatTus.Where(m => m.trangthai == 0 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
                    {
                        vattu = dbContext.VatTus.Where(m => m.trangthai == 2 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
                    }
                    else if (groupPanel1.Text == "Danh sách tất cả vật tư")
                    {
                        vattu = dbContext.VatTus.Where(m => m.madanhmuc == madanhmuc).ToList();
                    }
                }


                vatTus = vattu;
                for (int i = 0; i < vattu.Count; i++)
                {
                    string madanhmuc = (string)vattu[i].madanhmuc;
                    DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    danhMucs.Add(danhMuc);
                    dataGridView_DSVatTu.Rows.Add();
                    dataGridView_DSVatTu.Rows[i].Cells["Column1"].Value = madanhmuc + vattu[i].mavattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value = vattu[i].tenvattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column3"].Value = vattu[i].donvitinh;
                    string dongia = function.FormatDecimal((decimal)vattu[i].dongia);
                    dataGridView_DSVatTu.Rows[i].Cells["Column4"].Value = dongia;
                    dataGridView_DSVatTu.Rows[i].Cells["Column5"].Value = vattu[i].nguongoc;
                    if (vattu[i].trangthai == 0)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Dừng sử dụng";
                    }
                    else if (vattu[i].trangthai == 1)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Đang sử dụng";
                    }
                    else if (vattu[i].trangthai == 2)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Bị trùng";
                    }
                    dataGridView_DSVatTu.Rows[i].Cells["Column7"].Value = danhMuc.tendanhmuc;
                    dataGridView_DSVatTu.Rows[i].Cells["Column8"].Value = vattu[i].nguoisuacuoi;
                    dataGridView_DSVatTu.Rows[i].Cells["Column9"].Value = "Sửa ▼";
                }
            }
        }

        private void dataGridView_DSDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTu.Columns["Column9"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int rowID = e.RowIndex;
                    frmThemVatTu frmThemVatTu = new frmThemVatTu(vatTus[rowID]);
                    frmThemVatTu.Text = "Sửa thông tin vật tư";
                    frmThemVatTu.ShowDialog();
                    LoadDSVatTu();
                }
                catch { }
            }
        }

        private void btnThemVatTu_Click(object sender, EventArgs e)
        {
            frmThemVatTu frmThemVatTu = new frmThemVatTu();
            frmThemVatTu.ShowDialog();
            LoadDSVatTu();
        }

        private void btnVatTuDangSuDung_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách vật tư đang sử dụng")
            {
                groupPanel1.Text = "Danh sách vật tư đang sử dụng";
                btnHienThi.Text = "VT đang sử dụng";
                LoadDSVatTu();
            }
        }

        private void btnVatTuDungSuDung_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách vật tư dừng sử dụng")
            {
                groupPanel1.Text = "Danh sách vật tư dừng sử dụng";
                btnHienThi.Text = "VT dừng sử dụng";
                LoadDSVatTu();
            }
        }

        private void btnVatTuBiTrung_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách vật tư bị trùng")
            {
                groupPanel1.Text = "Danh sách vật tư bị trùng";
                btnHienThi.Text = "Vật tư bị trùng";
                LoadDSVatTu();
            }
        }

        private void btnTatCaVatTu_Click(object sender, EventArgs e)
        {
            if (groupPanel1.Text != "Danh sách tất cả vật tư")
            {
                groupPanel1.Text = "Danh sách tất cả vật tư";
                btnHienThi.Text = "Tất cả vật tư";
                LoadDSVatTu();
            }
        }

        private void btnXuatDanhSach_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu file";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = groupPanel1.Text + $"_{DateTime.Now.ToString("HH.mm.ss_dd.MM.yyyy")}.xlsx";
                    string filePath = Path.Combine(selectedPath, fileName);
                    ExportVatTusToExcel(vatTus, danhMucs, filePath);
                    MessageBox.Show("Xuất danh sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportVatTusToExcel(List<VatTu> vatTus, List<DanhMuc> danhMucs, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách vật tư");

                // Add header row
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã Vật Tư";
                worksheet.Cells[1, 3].Value = "Tên Vật Tư";
                worksheet.Cells[1, 4].Value = "Đơn Vị Tính";
                worksheet.Cells[1, 5].Value = "Đơn Giá";
                worksheet.Cells[1, 6].Value = "Nguồn Gốc";
                worksheet.Cells[1, 7].Value = "Trạng Thái";
                worksheet.Cells[1, 8].Value = "Thông Số Kỹ Thuật";
                worksheet.Cells[1, 9].Value = "Danh Mục";
                worksheet.Cells[1, 10].Value = "Ghi Chú";

                // Add data rows
                for (int i = 0; i < vatTus.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    worksheet.Cells[i + 2, 2].Value = vatTus[i].mavattu;
                    worksheet.Cells[i + 2, 3].Value = vatTus[i].tenvattu;
                    worksheet.Cells[i + 2, 4].Value = vatTus[i].donvitinh;
                    string dongia = function.FormatDecimal((decimal)vatTus[i].dongia);
                    worksheet.Cells[i + 2, 5].Value = dongia;
                    worksheet.Cells[i + 2, 6].Value = vatTus[i].nguongoc;
                    if (vatTus[i].trangthai == 0)
                    {
                        worksheet.Cells[i + 2, 7].Value = "Dừng sử dụng";
                    }
                    else if (vatTus[i].trangthai == 1)
                    {
                        worksheet.Cells[i + 2, 7].Value = "Đang sử dụng";
                    }
                    else if (vatTus[i].trangthai == 2)
                    {
                        worksheet.Cells[i + 2, 7].Value = "Bị trùng";
                    }
                    worksheet.Cells[i + 2, 8].Value = vatTus[i].thongsokythuat;
                    worksheet.Cells[i + 2, 9].Value = danhMucs[i].tendanhmuc;
                    worksheet.Cells[i + 2, 10].Value = vatTus[i].ghichu;
                }

                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cbbDanhMuc.Text = "Tất cả danh mục";
            LoadDSVatTu();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSVatTu.Rows.Count - 1; i++)
            {
                string tenNguoiDung = ((string)dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value).ToLower();

                if (tenNguoiDung.Contains(searchText))
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

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDSVatTu();
        }
    }
}
