using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_LocDanhSachVatTu : UserControl
    {
        Function function = new Function();
        List<SoSanhVatTu> soSanhVatTus = new List<SoSanhVatTu>();
        List<SoSanhVatTu> soSanhVatTus_DaLoc = new List<SoSanhVatTu>();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        double dogiongkhac = 60;

        public usr_LocDanhSachVatTu()
        {
            InitializeComponent();
            dataGridView_DSVatTu1.RowTemplate.Height = 35;
            dataGridView_DSVatTu2.RowTemplate.Height = 35;
            LoadDSVatTu_Loc();
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

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2);
            }
            catch { }
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

        // Hàm so sánh và đưa ra độ giống nhau giữa các phần tử trong List<VatTu>
        public void CompareVatTus(List<VatTu> vatTus)
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                for (int i = 0; i < vatTus.Count; i++)
                {
                    for (int j = i + 1; j < vatTus.Count; j++)
                    {
                        double similarity = CalculateCosineSimilarity(vatTus[i].tenvattu, vatTus[j].tenvattu);
                        similarity = similarity * 100;
                        if (similarity >= dogiongkhac)
                        {
                            SoSanhVatTu soSanhVatTu = new SoSanhVatTu();
                            // vật tư 1
                            string madanhmuc1 = (string)vatTus[i].madanhmuc;
                            var danhmuc1 = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc1);
                            if (danhmuc1 != null)
                            {
                                soSanhVatTu.danhmuc1 = danhmuc1.tendanhmuc;
                            }
                            soSanhVatTu.mavattu1 = vatTus[i].mavattu;
                            soSanhVatTu.tenvattu1 = vatTus[i].tenvattu;

                            // vật tư 2
                            string madanhmuc2 = (string)vatTus[j].madanhmuc;
                            var danhmuc2 = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc2);
                            if (danhmuc2 != null)
                            {
                                soSanhVatTu.danhmuc2 = danhmuc2.tendanhmuc;
                            }
                            soSanhVatTu.mavattu2 = vatTus[j].mavattu;
                            soSanhVatTu.tenvattu2 = vatTus[j].tenvattu;
                            soSanhVatTu.giongnhau = Math.Round(similarity, 2);
                            soSanhVatTus.Add(soSanhVatTu);
                        }
                    }
                }
            }
        }

        private void LoadDSVatTu_Loc()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vattu = new List<VatTu>();
                vattu = dbContext.VatTus.Where(m => m.trangthai == 1).ToList();
                soSanhVatTus.Clear();
                CompareVatTus(vattu);
                List<SoSanhVatTu> sortedList = soSanhVatTus.OrderByDescending(vt => vt.giongnhau).ToList();
                for (int i = 0; i < sortedList.Count; i++)
                {
                    dataGridView_DSVatTu1.Rows.Add();
                    dataGridView_DSVatTu1.Rows[i].Cells["Column1"].Value = "Xác định";
                    dataGridView_DSVatTu1.Rows[i].Cells["Column2"].Value = sortedList[i].danhmuc1;
                    dataGridView_DSVatTu1.Rows[i].Cells["Column3"].Value = sortedList[i].tenvattu1;
                    dataGridView_DSVatTu1.Rows[i].Cells["Column4"].Value = "-";
                    dataGridView_DSVatTu1.Rows[i].Cells["Column5"].Value = sortedList[i].danhmuc2;
                    dataGridView_DSVatTu1.Rows[i].Cells["Column6"].Value = sortedList[i].tenvattu2;
                    dataGridView_DSVatTu1.Rows[i].Cells["Column7"].Value = sortedList[i].giongnhau + "%";
                }
            }
        }

        private void dataGridView_DSVatTu1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTu1.Columns["Column1"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int rowID = e.RowIndex;
                    var cellValue = dataGridView_DSVatTu1.Rows[rowID].Cells["Column1"].Value?.ToString();

                    if (cellValue == "Trùng nhau")
                    {
                        dataGridView_DSVatTu1.Rows[rowID].Cells["Column1"].Value = "Không";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu1.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }
                        string tenvattu1 = dataGridView_DSVatTu1.Rows[rowID].Cells["Column3"].Value?.ToString();
                        string tenvattu2 = dataGridView_DSVatTu1.Rows[rowID].Cells["Column6"].Value?.ToString();
                        SoSanhVatTu soSanh = soSanhVatTus_DaLoc.SingleOrDefault(m => m.tenvattu1 == tenvattu1 && m.tenvattu2 == tenvattu2);
                        soSanhVatTus_DaLoc.Remove(soSanh);
                        HienThiDSVatTuDaLoc();
                    }
                    else if (cellValue == "Xác định" || cellValue == "Không")
                    {
                        dataGridView_DSVatTu1.Rows[rowID].Cells["Column1"].Value = "Trùng nhau";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu1.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.OrangeRed;
                        }
                        string tenvattu1 = dataGridView_DSVatTu1.Rows[rowID].Cells["Column3"].Value?.ToString();
                        string tenvattu2 = dataGridView_DSVatTu1.Rows[rowID].Cells["Column6"].Value?.ToString();
                        SoSanhVatTu soSanh = soSanhVatTus.SingleOrDefault(m => m.tenvattu1 == tenvattu1 && m.tenvattu2 == tenvattu2);
                        soSanhVatTus_DaLoc.Add(soSanh);
                        HienThiDSVatTuDaLoc();
                    }
                }
                catch { }
            }
        }

        private void ToMauTuDong(int rowID)
        {
            try
            {
                var cellValue = dataGridView_DSVatTu1.Rows[rowID].Cells["Column1"].Value?.ToString();
                dataGridView_DSVatTu1.Rows[rowID].Cells["Column1"].Value = "Trùng nhau";
                foreach (DataGridViewCell cell in dataGridView_DSVatTu1.Rows[rowID].Cells)
                {
                    cell.Style.BackColor = Color.OrangeRed;
                }
                string tenvattu1 = dataGridView_DSVatTu1.Rows[rowID].Cells["Column3"].Value?.ToString();
                string tenvattu2 = dataGridView_DSVatTu1.Rows[rowID].Cells["Column6"].Value?.ToString();
                SoSanhVatTu soSanh = soSanhVatTus.SingleOrDefault(m => m.tenvattu1 == tenvattu1 && m.tenvattu2 == tenvattu2);
                soSanhVatTus_DaLoc.Add(soSanh);
            }
            catch { }
        }

        private void btnTimKiemVatTu1_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem1.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);

            for (int i = 0; i < dataGridView_DSVatTu1.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column3"].Value).ToLower();
                    string tenVatTu2 = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column6"].Value).ToLower();

                    if (tenVatTu1.Contains(searchText) || tenVatTu2.Contains(searchText))
                    {
                        dataGridView_DSVatTu1.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTu1.Rows[i].Visible = false;
                    }
                }
                else
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column3"].Value).ToLower();
                    string tenVatTu2 = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column6"].Value).ToLower();

                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu1.Contains(liststring[j].Trim().ToLower()) || tenVatTu2.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_DSVatTu1.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_DSVatTu1.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem1_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem1.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTu1.Rows.Count; i++)
                {
                    dataGridView_DSVatTu1.Rows[i].Visible = true;
                }
                cbbDanhMuc_SelectedIndexChanged(sender, e);
            }
        }

        private void HienThiDSVatTuDaLoc()
        {
            dataGridView_DSVatTu2.Rows.Clear();
            for (int i = 0; i < soSanhVatTus_DaLoc.Count; i++)
            {
                dataGridView_DSVatTu2.Rows.Add();
                dataGridView_DSVatTu2.Rows[i].Cells["Column8"].Value = i + 1;
                dataGridView_DSVatTu2.Rows[i].Cells["Column9"].Value = soSanhVatTus_DaLoc[i].danhmuc1;
                dataGridView_DSVatTu2.Rows[i].Cells["Column10"].Value = soSanhVatTus_DaLoc[i].tenvattu1;
                dataGridView_DSVatTu2.Rows[i].Cells["Column11"].Value = "-";
                dataGridView_DSVatTu2.Rows[i].Cells["Column12"].Value = soSanhVatTus_DaLoc[i].danhmuc2;
                dataGridView_DSVatTu2.Rows[i].Cells["Column13"].Value = soSanhVatTus_DaLoc[i].tenvattu2;
                dataGridView_DSVatTu2.Rows[i].Cells["Column14"].Value = soSanhVatTus_DaLoc[i].giongnhau + "%";
            }
        }

        private void LocTuDong(double phantramgiongnhau)
        {
            for (int i = 0; i < dataGridView_DSVatTu1.Rows.Count - 1; i++)
            {
                string str_giongnhau = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column7"].Value).ToLower();
                str_giongnhau = str_giongnhau.Replace("%", "").Trim();
                double giongnhau = Double.Parse(str_giongnhau);

                if (giongnhau > phantramgiongnhau)
                {
                    ToMauTuDong(i);
                }
            }
            HienThiDSVatTuDaLoc();
        }

        private void btnLocTuDong90_Click(object sender, EventArgs e)
        {
            dogiongkhac = 90;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
            LocTuDong(dogiongkhac);
        }

        private void btnLocTuDong80_Click(object sender, EventArgs e)
        {
            dogiongkhac = 80;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
            LocTuDong(dogiongkhac);
        }

        private void btnLocTuDong70_Click(object sender, EventArgs e)
        {
            dogiongkhac = 70;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
            LocTuDong(dogiongkhac);
        }

        private void btnLocTuDong60_Click(object sender, EventArgs e)
        {
            dogiongkhac = 60;
            btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
            LamMoi();
            LocTuDong(dogiongkhac);
        }

        private void LamMoi()
        {
            txtNoiDungTimKiem1.Text = string.Empty;
            txtNoiDungTimKiem2.Text = string.Empty;
            cbbDanhMuc.Text = "Tất cả danh mục";
            soSanhVatTus.Clear();
            soSanhVatTus_DaLoc.Clear();
            dataGridView_DSVatTu1.Rows.Clear();
            dataGridView_DSVatTu2.Rows.Clear();
            LoadDSVatTu_Loc();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnLuuDanhSachDaLoc_Click(object sender, EventArgs e)
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                for (int i = 0; i < soSanhVatTus_DaLoc.Count; i++)
                {
                    int mavattu = (int)soSanhVatTus[i].mavattu2;
                    VatTu vatTu = dbContext.VatTus.SingleOrDefault(m => m.mavattu == mavattu);
                    vatTu.nguoisuacuoi = frmDangNhap.tennguoidung;
                    vatTu.thoigiansua = DateTime.Now;
                    vatTu.trangthai = 2;
                }
                dbContext.SaveChanges();

                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                lichSuHoatDong.thoigian = DateTime.Now;
                lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã lọc danh sách vật tư";
                lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                lichSuHoatDong.id = frmDangNhap.userID;
                dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Lọc danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LamMoi();
        }

        private void btnTimKiemVatTu2_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem2.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);

            for (int i = 0; i < dataGridView_DSVatTu2.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTu2.Rows[i].Cells["Column10"].Value).ToLower();
                    string tenVatTu2 = ((string)dataGridView_DSVatTu2.Rows[i].Cells["Column13"].Value).ToLower();

                    if (tenVatTu1.Contains(searchText) || tenVatTu2.Contains(searchText))
                    {
                        dataGridView_DSVatTu2.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTu2.Rows[i].Visible = false;
                    }
                }
                else
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTu2.Rows[i].Cells["Column10"].Value).ToLower();
                    string tenVatTu2 = ((string)dataGridView_DSVatTu2.Rows[i].Cells["Column13"].Value).ToLower();

                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu1.Contains(liststring[j].Trim().ToLower()) || tenVatTu2.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_DSVatTu2.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_DSVatTu2.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem2_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem2.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTu2.Rows.Count; i++)
                {
                    dataGridView_DSVatTu2.Rows[i].Visible = true;
                }
            }
        }

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDanhMuc.Text == "Tất cả danh mục")
            {
                for (int i = 0; i < dataGridView_DSVatTu1.Rows.Count; i++)
                {
                    dataGridView_DSVatTu1.Rows[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView_DSVatTu1.Rows.Count - 1; i++)
                {
                    string tenDanhMuc1 = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column2"].Value).ToLower();
                    string tenDanhMuc2 = ((string)dataGridView_DSVatTu1.Rows[i].Cells["Column5"].Value).ToLower();

                    if (tenDanhMuc1.Contains(cbbDanhMuc.Text.ToLower()))
                    {
                        dataGridView_DSVatTu1.Rows[i].Visible = true;
                    }
                    if (tenDanhMuc2.Contains(cbbDanhMuc.Text.ToLower()))
                    {
                        dataGridView_DSVatTu1.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTu1.Rows[i].Visible = false;
                    }
                }
            }
        }
    }
}