using OfficeOpenXml;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_DanhSachVatTu : UserControl
    {
        Function function = new Function();
        List<VatTu> vatTus = new List<VatTu>();
        List<DanhMuc> danhMucs = new List<DanhMuc>();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        private int currentPage = 1;
        private int pageSize = 200; // Number of items per page
        private int totalPages = 0;
        frmLoad frmLoad;
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

        //private void LoadDSVatTu()
        //{
        //    dataGridView_DSVatTu.Rows.Clear();
        //    danhMucs.Clear();
        //    using (var dbContext = new QuanLyVatTuDbContext())
        //    {
        //        var vattu = new List<VatTu>();

        //        if (cbbDanhMuc.Text == "Tất cả danh mục")
        //        {
        //            if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.trangthai == 1).OrderBy(m => m.tenvattu).ToList();
        //            }
        //            else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.trangthai == 0).OrderBy(m => m.tenvattu).ToList();
        //            }
        //            else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.trangthai == 2).OrderBy(m => m.tenvattu).ToList();
        //            }
        //            else if (groupPanel1.Text == "Danh sách tất cả vật tư")
        //            {
        //                vattu = dbContext.VatTus.ToList();
        //            }
        //        }
        //        else
        //        {
        //            DanhMuc dm = listdanhmuc.SingleOrDefault(m => m.tendanhmuc == cbbDanhMuc.Text);
        //            string madanhmuc = dm.madanhmuc;
        //            if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.trangthai == 1 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
        //            }
        //            else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.trangthai == 0 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
        //            }
        //            else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.trangthai == 2 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
        //            }
        //            else if (groupPanel1.Text == "Danh sách tất cả vật tư")
        //            {
        //                vattu = dbContext.VatTus.Where(m => m.madanhmuc == madanhmuc).ToList();
        //            }
        //        }


        //        vatTus = vattu;
        //        for (int i = 0; i < vattu.Count; i++)
        //        {
        //            string madanhmuc = (string)vattu[i].madanhmuc;
        //            DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
        //            danhMucs.Add(danhMuc);
        //            dataGridView_DSVatTu.Rows.Add();
        //            dataGridView_DSVatTu.Rows[i].Cells["Column1"].Value = madanhmuc + vattu[i].mavattu;
        //            dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value = vattu[i].tenvattu;
        //            dataGridView_DSVatTu.Rows[i].Cells["Column3"].Value = vattu[i].donvitinh;
        //            string dongia = function.FormatDecimal((decimal)vattu[i].dongia);
        //            dataGridView_DSVatTu.Rows[i].Cells["Column4"].Value = dongia;
        //            dataGridView_DSVatTu.Rows[i].Cells["Column5"].Value = vattu[i].nguongoc;
        //            if (vattu[i].trangthai == 0)
        //            {
        //                dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Dừng sử dụng";
        //            }
        //            else if (vattu[i].trangthai == 1)
        //            {
        //                dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Đang sử dụng";
        //            }
        //            else if (vattu[i].trangthai == 2)
        //            {
        //                dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Bị trùng";
        //            }
        //            if(danhMuc != null)
        //            {
        //                dataGridView_DSVatTu.Rows[i].Cells["Column7"].Value = danhMuc.tendanhmuc;
        //            }
        //            dataGridView_DSVatTu.Rows[i].Cells["Column8"].Value = vattu[i].nguoisuacuoi;
        //            dataGridView_DSVatTu.Rows[i].Cells["Column9"].Value = "Sửa ▼";
        //        }
        //    }
        //}

        private void LoadDSVatTu()
        {
            this.Cursor = Cursors.WaitCursor;
            dataGridView_DSVatTu.Rows.Clear();
            danhMucs.Clear();

            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vattuQuery = new List<VatTu>().AsQueryable();

                // Handle category selection
                if (cbbDanhMuc.Text == "Tất cả danh mục")
                {
                    if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.trangthai == 1);
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.trangthai == 0);
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.trangthai == 2);
                    }
                    else if (groupPanel1.Text == "Danh sách tất cả vật tư")
                    {
                        vattuQuery = dbContext.VatTus;
                    }
                }
                else
                {
                    DanhMuc dm = listdanhmuc.SingleOrDefault(m => m.tendanhmuc == cbbDanhMuc.Text);
                    string madanhmuc = dm.madanhmuc;

                    if (groupPanel1.Text == "Danh sách vật tư đang sử dụng")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.trangthai == 1 && m.madanhmuc == madanhmuc);
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư dừng sử dụng")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.trangthai == 0 && m.madanhmuc == madanhmuc);
                    }
                    else if (groupPanel1.Text == "Danh sách vật tư bị trùng")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.trangthai == 2 && m.madanhmuc == madanhmuc);
                    }
                    else if (groupPanel1.Text == "Danh sách tất cả vật tư")
                    {
                        vattuQuery = dbContext.VatTus.Where(m => m.madanhmuc == madanhmuc);
                    }
                }

                // Pagination
                vatTus = vattuQuery.OrderBy(m => m.tenvattu).ToList();
                totalPages = (int)Math.Ceiling((double)vatTus.Count / pageSize);
                var pagedVatTus = vatTus.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Display data for the current page
                for (int i = 0; i < pagedVatTus.Count; i++)
                {
                    string madanhmuc = (string)pagedVatTus[i].madanhmuc;
                    DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    danhMucs.Add(danhMuc);
                    dataGridView_DSVatTu.Rows.Add();
                    dataGridView_DSVatTu.Rows[i].Cells["Column1"].Value = madanhmuc + pagedVatTus[i].mavattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column10"].Value = pagedVatTus[i].mavattu_hethong;
                    dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value = pagedVatTus[i].tenvattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column3"].Value = pagedVatTus[i].donvitinh;
                    string dongia = function.FormatDecimal((decimal)pagedVatTus[i].dongia);
                    dataGridView_DSVatTu.Rows[i].Cells["Column4"].Value = dongia;
                    dataGridView_DSVatTu.Rows[i].Cells["Column5"].Value = pagedVatTus[i].nguongoc;

                    if (pagedVatTus[i].trangthai == 0)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Dừng sử dụng";
                    }
                    else if (pagedVatTus[i].trangthai == 1)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Đang sử dụng";
                    }
                    else if (pagedVatTus[i].trangthai == 2)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column6"].Value = "Bị trùng";
                    }

                    if (danhMuc != null)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column7"].Value = danhMuc.tendanhmuc;
                    }

                    dataGridView_DSVatTu.Rows[i].Cells["Column8"].Value = pagedVatTus[i].nguoisuacuoi;
                    dataGridView_DSVatTu.Rows[i].Cells["Column9"].Value = "Sửa ▼";
                }
                lblPageNumber.Text = $"Trang {currentPage}/{totalPages}";
            }
            this.Cursor = Cursors.Default;
        }


        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDSVatTu();
            }
        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDSVatTu();
            }
        }

        private void dataGridView_DSDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTu.Columns["Column9"].Index && e.RowIndex >= 0)
            {
                try
                {
                    string mvt = (string)dataGridView_DSVatTu.Rows[e.RowIndex].Cells["Column1"].Value;
                    mvt = Regex.Replace(mvt, @"[^\d]", "");
                    int mapvattu = Int32.Parse(mvt);
                    int rowID = vatTus.FindIndex(m => m.mavattu == mapvattu);
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
                    try
                    {
                        worksheet.Cells[i + 2, 9].Value = danhMucs[i].tendanhmuc;
                    }
                    catch
                    {
                        worksheet.Cells[i + 2, 9].Value = "";
                    }

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
            currentPage = 1;
            pageSize = 10000;
            LoadDSVatTu();
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);
            for (int i = 0; i < dataGridView_DSVatTu.Rows.Count - 1; i++)
            {
                if (liststring == null)
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
                else
                {
                    string tenNguoiDung = ((string)dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value).ToLower();
                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenNguoiDung.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_DSVatTu.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_DSVatTu.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                currentPage = 1;
                pageSize = 200;
                LoadDSVatTu();
            }
        }

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            pageSize = 200;
            LoadDSVatTu();
        }

        private void usr_DanhSachVatTu_Resize(object sender, EventArgs e)
        {
            dataGridView_DSVatTu.Columns["Column1"].Width = (int)(dataGridView_DSVatTu.Width * 2 / 27);
            dataGridView_DSVatTu.Columns["Column10"].Width = (int)(dataGridView_DSVatTu.Width * 2 / 27);
            dataGridView_DSVatTu.Columns["Column2"].Width = (int)(dataGridView_DSVatTu.Width * 5 / 27);
            dataGridView_DSVatTu.Columns["Column3"].Width = (int)(dataGridView_DSVatTu.Width * 2.4 / 27);
            dataGridView_DSVatTu.Columns["Column4"].Width = (int)(dataGridView_DSVatTu.Width * 2.3 / 27);
            dataGridView_DSVatTu.Columns["Column5"].Width = (int)(dataGridView_DSVatTu.Width * 2.4 / 27);
            dataGridView_DSVatTu.Columns["Column6"].Width = (int)(dataGridView_DSVatTu.Width * 3 / 27);
            dataGridView_DSVatTu.Columns["Column7"].Width = (int)(dataGridView_DSVatTu.Width * 3 / 27);
            dataGridView_DSVatTu.Columns["Column8"].Width = (int)(dataGridView_DSVatTu.Width * 3 / 27);
            dataGridView_DSVatTu.Columns["Column9"].Width = (int)(dataGridView_DSVatTu.Width * 2 / 27);
        }

        private void txtNoiDungTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
