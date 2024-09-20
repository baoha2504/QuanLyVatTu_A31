using OfficeOpenXml;
using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_ImportDanhSachVatTu : UserControl
    {
        Function function = new Function();
        List<NewVatTuFromExcel> vatTuMoiExcels = new List<NewVatTuFromExcel>();
        List<NewVatTuFromExcel> vatTuMoiExcels_Giong = new List<NewVatTuFromExcel>();
        List<NewVatTuFromExcel> vatTuMoiExcels_Khac = new List<NewVatTuFromExcel>();
        double dogiongkhac = 70;

        public usr_ImportDanhSachVatTu()
        {
            InitializeComponent();
            dataGridView_CanXacNhan.RowTemplate.Height = 35;
            dataGridView_VatTuMoi.RowTemplate.Height = 35;
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2) - 50;
            }
            catch { }
        }

        private void btnChonFileExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file Excel";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathFileExcel.Text = openFileDialog.FileName;
                    LoadDS_Excel(openFileDialog.FileName);
                    LoadVatTuMoiThem();
                }
            }
        }

        private void LoadDS_Excel(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                // Fully qualify the LicenseContext to resolve the ambiguity
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 2].Text))
                        {
                            NewVatTuFromExcel vatTuMoiExcel = new NewVatTuFromExcel();
                            vatTuMoiExcel.tenvattu = worksheet.Cells[row, 2].Text;
                            vatTuMoiExcel.donvitinh = worksheet.Cells[row, 3].Text;
                            try
                            {
                                decimal dongia = function.ConvertStringToDecimal(worksheet.Cells[row, 4].Text);
                                vatTuMoiExcel.dongia = dongia;
                            }
                            catch
                            {
                                vatTuMoiExcel.dongia = 0;
                            }
                            vatTuMoiExcel.nguongoc = worksheet.Cells[row, 5].Text;
                            vatTuMoiExcel.thongsokythuat = worksheet.Cells[row, 6].Text;
                            vatTuMoiExcel.tendanhmuc = worksheet.Cells[row, 7].Text;
                            vatTuMoiExcel.ghichu = worksheet.Cells[row, 8].Text;

                            vatTuMoiExcels.Add(vatTuMoiExcel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm chuẩn hóa chuỗi (loại bỏ khoảng trắng thừa và ký tự vô nghĩa)
        private string NormalizeString(string input)
        {
            input = input.ToLower();
            input = Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
            input = Regex.Replace(input, @"\s+", " ").Trim();
            return input;
        }

        // Hàm tính Cosine Similarity giữa hai chuỗi
        public double CalculateCosineSimilarity(string a, string b)
        {
            a = NormalizeString(a);
            b = NormalizeString(b);

            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                return 0.0;
            }

            // Tách chuỗi thành các từ
            var wordsA = a.Split(' ');
            var wordsB = b.Split(' ');

            // Tính toán số lần xuất hiện của mỗi từ trong mỗi chuỗi
            var wordFrequencyA = wordsA.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
            var wordFrequencyB = wordsB.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());

            // Xác định tập hợp từ chung
            var allWords = wordFrequencyA.Keys.Union(wordFrequencyB.Keys).Distinct();

            // Tạo vector cho mỗi chuỗi
            var vectorA = allWords.Select(word => wordFrequencyA.ContainsKey(word) ? wordFrequencyA[word] : 0).ToArray();
            var vectorB = allWords.Select(word => wordFrequencyB.ContainsKey(word) ? wordFrequencyB[word] : 0).ToArray();

            // Tính toán dot product
            double dotProduct = vectorA.Zip(vectorB, (x, y) => x * y).Sum();

            // Tính toán độ dài vector
            double magnitudeA = Math.Sqrt(vectorA.Select(x => x * x).Sum());
            double magnitudeB = Math.Sqrt(vectorB.Select(x => x * x).Sum());

            // Trả về Cosine Similarity
            return dotProduct / (magnitudeA * magnitudeB);
        }

        // Hàm so sánh và đưa ra độ giống nhau giữa các phần tử trong List<VatTu> và List<NewVatTuFromExcel>
        public void CompareVatTus(List<VatTu> vatTus, List<NewVatTuFromExcel> vatTuMoiExcels_Input)
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                foreach (var vatTuMoi in vatTuMoiExcels_Input)
                {
                    double highestSimilarity = 0;
                    VatTu bestMatchVatTu = null;

                    foreach (var vatTu in vatTus)
                    {
                        double similarity = CalculateCosineSimilarity(vatTuMoi.tenvattu, vatTu.tenvattu) * 100;

                        if (similarity > highestSimilarity)
                        {
                            highestSimilarity = similarity;
                            bestMatchVatTu = vatTu;
                        }
                    }

                    vatTuMoi.tenvattudaco = bestMatchVatTu?.tenvattu ?? string.Empty;
                    vatTuMoi.giongnhau = Math.Round(highestSimilarity, 2);

                    if (highestSimilarity >= dogiongkhac)
                    {
                        vatTuMoiExcels_Giong.Add(vatTuMoi);
                    }
                    else
                    {
                        vatTuMoiExcels_Khac.Add(vatTuMoi);
                    }
                }
            }
        }


        private void LoadVatTuMoiThem()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vattu = new List<VatTu>();
                vattu = dbContext.VatTus.Where(m => m.trangthai == 1).ToList();
                CompareVatTus(vattu, vatTuMoiExcels);
            }
            LoadVatTuGiong();
            LoadVatTuKhac();
        }

        private void LoadVatTuMoiThem_KhiClick()
        {
            dataGridView_VatTuMoi.Rows.Clear();
            LoadVatTuKhac();
        }

        private void LoadVatTuGiong()
        {
            //giống nhau lớn hơn bằng 60 %
            List<NewVatTuFromExcel> sortedList = vatTuMoiExcels_Giong.OrderByDescending(vtm => vtm.giongnhau).ToList();
            for (int i = 0; i < sortedList.Count; i++)
            {
                dataGridView_CanXacNhan.Rows.Add();
                dataGridView_CanXacNhan.Rows[i].Cells["Column1"].Value = "Xác định";
                dataGridView_CanXacNhan.Rows[i].Cells["Column2"].Value = sortedList[i].tenvattu;
                dataGridView_CanXacNhan.Rows[i].Cells["Column3"].Value = sortedList[i].tenvattudaco;
                dataGridView_CanXacNhan.Rows[i].Cells["Column4"].Value = sortedList[i].giongnhau + "%";
            }
        }

        private void LoadVatTuKhac()
        {
            // giống nhau nhỏ hơn 60%
            for (int i = 0; i < vatTuMoiExcels_Khac.Count; i++)
            {
                dataGridView_VatTuMoi.Rows.Add();
                dataGridView_VatTuMoi.Rows[i].Cells["Column5"].Value = "Xem";
                dataGridView_VatTuMoi.Rows[i].Cells["Column6"].Value = i + 1;
                dataGridView_VatTuMoi.Rows[i].Cells["Column7"].Value = vatTuMoiExcels_Khac[i].tenvattu;
                dataGridView_VatTuMoi.Rows[i].Cells["Column8"].Value = vatTuMoiExcels_Khac[i].donvitinh;
                string dongia = function.FormatDecimal(vatTuMoiExcels_Khac[i].dongia);
                dataGridView_VatTuMoi.Rows[i].Cells["Column9"].Value = dongia;
                dataGridView_VatTuMoi.Rows[i].Cells["Column10"].Value = vatTuMoiExcels_Khac[i].nguongoc;
                dataGridView_VatTuMoi.Rows[i].Cells["Column11"].Value = vatTuMoiExcels_Khac[i].tendanhmuc;
            }
        }

        private void dataGridView_CanXacNhan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_CanXacNhan.Columns["Column1"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int rowID = e.RowIndex;
                    var cellValue = dataGridView_CanXacNhan.Rows[rowID].Cells["Column1"].Value?.ToString();
                    var tenvattu = dataGridView_CanXacNhan.Rows[rowID].Cells["Column2"].Value?.ToString();
                    var tenvattudaco = dataGridView_CanXacNhan.Rows[rowID].Cells["Column3"].Value?.ToString();

                    if (cellValue == "Trùng nhau" || cellValue == "Xác định")
                    {
                        dataGridView_CanXacNhan.Rows[rowID].Cells["Column1"].Value = "VT mới";
                        foreach (DataGridViewCell cell in dataGridView_CanXacNhan.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.LightGreen;
                        }
                        int id = vatTuMoiExcels_Giong.FindIndex(m => m.tenvattu == tenvattu);
                        NewVatTuFromExcel newVatTu = new NewVatTuFromExcel();
                        newVatTu = vatTuMoiExcels_Giong[id];
                        vatTuMoiExcels_Khac.Add(newVatTu);

                        LoadVatTuMoiThem_KhiClick();
                    }
                    else if (cellValue == "VT mới")
                    {
                        dataGridView_CanXacNhan.Rows[rowID].Cells["Column1"].Value = "Trùng nhau";
                        foreach (DataGridViewCell cell in dataGridView_CanXacNhan.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }

                        var newVatTu = vatTuMoiExcels_Khac.SingleOrDefault(m => m.tenvattu == tenvattu && m.tenvattudaco == tenvattudaco);
                        if (newVatTu != null)
                        {
                            vatTuMoiExcels_Khac.Remove(newVatTu);
                        }
                        LoadVatTuMoiThem_KhiClick();
                    }
                }
                catch { }
            }
        }

        private void dataGridView_VatTuMoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_VatTuMoi.Columns["Column5"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int stt = (int)dataGridView_VatTuMoi.Rows[e.RowIndex].Cells["Column6"].Value;
                    int rowID = stt - 1;
                    frmChiTietVatTuMoi frm = new frmChiTietVatTuMoi(vatTuMoiExcels_Khac[rowID]);
                    frm.ShowDialog();
                    vatTuMoiExcels_Khac[rowID] = frm.vtmExcel;
                    LoadVatTuMoiThem_KhiClick();
                }
                catch { }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);

            for (int i = 0; i < dataGridView_CanXacNhan.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu1 = ((string)dataGridView_CanXacNhan.Rows[i].Cells["Column2"].Value).ToLower();
                    string tenVatTu2 = ((string)dataGridView_CanXacNhan.Rows[i].Cells["Column3"].Value).ToLower();

                    if (tenVatTu1.Contains(searchText) || tenVatTu2.Contains(searchText))
                    {
                        dataGridView_CanXacNhan.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_CanXacNhan.Rows[i].Visible = false;
                    }
                }
                else
                {
                    string tenVatTu1 = ((string)dataGridView_CanXacNhan.Rows[i].Cells["Column2"].Value).ToLower();
                    string tenVatTu2 = ((string)dataGridView_CanXacNhan.Rows[i].Cells["Column3"].Value).ToLower();

                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu1.Contains(liststring[j].Trim().ToLower()) || tenVatTu2.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_CanXacNhan.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_CanXacNhan.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_CanXacNhan.Rows.Count; i++)
                {
                    dataGridView_CanXacNhan.Rows[i].Visible = true;
                }
            }
        }

        private void btnLocTuDong90_Click(object sender, EventArgs e)
        {
            dogiongkhac = 90;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
        }

        private void btnLocTuDong80_Click(object sender, EventArgs e)
        {
            dogiongkhac = 80;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
        }

        private void btnLocTuDong70_Click(object sender, EventArgs e)
        {
            dogiongkhac = 70;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
        }

        private void btnLocTuDong60_Click(object sender, EventArgs e)
        {
            dogiongkhac = 60;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
        }

        private void LamMoi()
        {
            txtNoiDungTimKiem.Text = string.Empty;
            dataGridView_CanXacNhan.Rows.Clear();
            dataGridView_VatTuMoi.Rows.Clear();
            vatTuMoiExcels_Giong.Clear();
            vatTuMoiExcels_Khac.Clear();

            LoadVatTuMoiThem();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnLuuDanhSachDaLoc_Click(object sender, EventArgs e)
        {
            
            if (dataGridView_VatTuMoi.Rows.Count >= 1)
            {
                DialogResult result = MessageBox.Show("Xác nhận lưu danh vật tư mới?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (var dbContext = new QuanLyVatTuDbContext())
                    {
                        for (int i = 0; i < vatTuMoiExcels_Khac.Count; i++)
                        {
                            VatTu vatTu = new VatTu();
                            vatTu.tenvattu = vatTuMoiExcels_Khac[i].tenvattu;
                            vatTu.donvitinh = vatTuMoiExcels_Khac[i].donvitinh;
                            vatTu.dongia = vatTuMoiExcels_Khac[i].dongia;
                            vatTu.nguongoc = vatTuMoiExcels_Khac[i].nguongoc;
                            vatTu.thongsokythuat = vatTuMoiExcels_Khac[i].thongsokythuat;
                            vatTu.trangthai = 1;
                            vatTu.ghichu = vatTuMoiExcels_Khac[i].ghichu;
                            vatTu.nguoisuacuoi = frmDangNhap.tennguoidung;
                            vatTu.thoigiansua = DateTime.Now;
                            vatTu.user_id = frmDangNhap.userID;
                            string tendanhmuc = vatTuMoiExcels_Khac[i].tendanhmuc;
                            var danhmuc = dbContext.DanhMucs.SingleOrDefault(m => m.tendanhmuc.Trim().ToLower() == tendanhmuc.Trim().ToLower());
                            if (danhmuc != null)
                            {
                                vatTu.madanhmuc = danhmuc.madanhmuc;
                            }
                            else
                            {
                                vatTu.madanhmuc = "KHAC";
                            }
                            dbContext.VatTus.Add(vatTu);
                        }

                        LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                        lichSuHoatDong.thoigian = DateTime.Now;
                        lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã nhập file vật tư {Path.GetFileName(txtPathFileExcel.Text)}";
                        lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                        lichSuHoatDong.id = frmDangNhap.userID;
                        dbContext.LichSuHoatDongs.Add(lichSuHoatDong);

                        dbContext.SaveChanges();
                    }
                    MessageBox.Show("Thêm danh sách mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LamMoi();
                }
            }
            else
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupPanel2_Resize(object sender, EventArgs e)
        {
            dataGridView_CanXacNhan.Columns["Column1"].Width = (int)(dataGridView_CanXacNhan.Width * 2 / 13);
            dataGridView_CanXacNhan.Columns["Column2"].Width = (int)(dataGridView_CanXacNhan.Width * 4 / 13);
            dataGridView_CanXacNhan.Columns["Column3"].Width = (int)(dataGridView_CanXacNhan.Width * 4 / 13);
            dataGridView_CanXacNhan.Columns["Column4"].Width = (int)(dataGridView_CanXacNhan.Width * 2 / 13);

            dataGridView_VatTuMoi.Columns["Column5"].Width = (int)(dataGridView_VatTuMoi.Width * 2 / 28);
            dataGridView_VatTuMoi.Columns["Column6"].Width = (int)(dataGridView_VatTuMoi.Width * 2 / 28);
            dataGridView_VatTuMoi.Columns["Column7"].Width = (int)(dataGridView_VatTuMoi.Width * 6 / 28);
            dataGridView_VatTuMoi.Columns["Column8"].Width = (int)(dataGridView_VatTuMoi.Width * 4 / 28);
            dataGridView_VatTuMoi.Columns["Column9"].Width = (int)(dataGridView_VatTuMoi.Width * 4 / 28);
            dataGridView_VatTuMoi.Columns["Column10"].Width = (int)(dataGridView_VatTuMoi.Width * 4 / 28);
            dataGridView_VatTuMoi.Columns["Column11"].Width = (int)(dataGridView_VatTuMoi.Width * 4 / 28);
        }
    }
}
