using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class usr_LocDanhSachVatTu2 : UserControl
    {
        Function function = new Function();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        List<VatTu> vatTuHoatDongs = new List<VatTu>();
        List<VatTuTrung> vatTuBiTrungs = new List<VatTuTrung>();
        VatTu vattu_duocchon = new VatTu();
        int line_choising = -1;
        double dogiongkhac = 70;
        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 200;    // Số dòng trên mỗi trang
        private int totalPages = 0;    // Tổng số trang

        public usr_LocDanhSachVatTu2()
        {
            InitializeComponent();
            dataGridView_DSVatTuHoatDong.RowTemplate.Height = 35;
            dataGridView_DSVatTuTrung.RowTemplate.Height = 35;
            dataGridView_DSVatTuTrung.DefaultCellStyle.BackColor = Color.Moccasin;

            using (var dbContext = new QuanLyVatTuDbContext())
            {
                listdanhmuc = dbContext.DanhMucs.OrderBy(m => m.tendanhmuc).ToList();
                foreach (var dm in listdanhmuc)
                {
                    cbbDanhMuc.Items.Add(dm.tendanhmuc);
                }
                cbbDanhMuc.Items.Add("Tất cả danh mục");
            }

            LoadDSVatTu();
            groupPanel2.Text = $"Danh sách dự đoán là trùng nhau {dogiongkhac}%";
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2);
            }
            catch { }
        }

        private void LoadDSVatTu()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vattu = new List<VatTu>();

                if (cbbDanhMuc.Text == "Tất cả danh mục")
                {
                    vattu = dbContext.VatTus.Where(m => m.trangthai == 1).OrderBy(m => m.tenvattu).ToList();
                }
                else
                {
                    DanhMuc dm = listdanhmuc.SingleOrDefault(m => m.tendanhmuc == cbbDanhMuc.Text);
                    string madanhmuc = dm.madanhmuc;
                    vattu = dbContext.VatTus.Where(m => m.trangthai == 1 && m.madanhmuc == madanhmuc).OrderBy(m => m.tenvattu).ToList();
                }

                vatTuHoatDongs = vattu; // Lưu dữ liệu
                totalPages = (vatTuHoatDongs.Count + pageSize - 1) / pageSize;
                
                DisplayCurrentPage();    // Hiển thị trang hiện tại
            }
        }

        private void DisplayCurrentPage()
        {
            dataGridView_DSVatTuHoatDong.Rows.Clear();
            lblPageNumber.Text = $"Trang {currentPage}/{totalPages}";      // Cập nhật số trang

            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, vatTuHoatDongs.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                string madanhmuc = vatTuHoatDongs[i].madanhmuc;
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    dataGridView_DSVatTuHoatDong.Rows.Add();
                    dataGridView_DSVatTuHoatDong.Rows[i - startIndex].Cells["Column1"].Value = "Lọc";
                    dataGridView_DSVatTuHoatDong.Rows[i - startIndex].Cells["Column2"].Value = vatTuHoatDongs[i].tenvattu;
                    dataGridView_DSVatTuHoatDong.Rows[i - startIndex].Cells["Column3"].Value = vatTuHoatDongs[i].donvitinh;
                    dataGridView_DSVatTuHoatDong.Rows[i - startIndex].Cells["Column4"].Value = vatTuHoatDongs[i].nguongoc;
                    if (danhMuc != null)
                    {
                        dataGridView_DSVatTuHoatDong.Rows[i - startIndex].Cells["Column5"].Value = danhMuc.tendanhmuc;
                    }
                    else
                    {
                        dataGridView_DSVatTuHoatDong.Rows[i - startIndex].Cells["Column5"].Value = "";
                    }
                }
            }
        }


        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayCurrentPage();
            }
        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayCurrentPage();
            }
        }

        private void dataGridView_DSVatTuHoatDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTuHoatDong.Columns["Column1"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int rowID = e.RowIndex;
                    var cellValue = dataGridView_DSVatTuHoatDong.Rows[rowID].Cells["Column1"].Value?.ToString();
                    var tenvattu = dataGridView_DSVatTuHoatDong.Rows[rowID].Cells["Column2"].Value?.ToString();
                    HienThiTenVatTu(tenvattu);

                    if (cellValue == "Lọc")
                    {
                        dataGridView_DSVatTuHoatDong.Rows[rowID].Cells["Column1"].Value = "Đã chọn";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTuHoatDong.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.LightGreen;
                        }
                        if (line_choising != -1)
                        {
                            dataGridView_DSVatTuHoatDong.Rows[line_choising].Cells["Column1"].Value = "Lọc";
                            foreach (DataGridViewCell cell in dataGridView_DSVatTuHoatDong.Rows[line_choising].Cells)
                            {
                                cell.Style.BackColor = Color.White;
                            }
                        }
                        int index = vatTuHoatDongs.FindIndex(m => m.tenvattu == tenvattu);
                        vattu_duocchon = vatTuHoatDongs[index];
                        line_choising = index;
                    }
                    else if (cellValue == "Đã chọn")
                    {
                        dataGridView_DSVatTuHoatDong.Rows[rowID].Cells["Column1"].Value = "Lọc";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTuHoatDong.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }
                    }
                    ChuanHoaVaHienThi();

                }
                catch { }
            }
        }

        private void HienThiTenVatTu(string tenvattu)
        {
            lblTenVatTu.Visible = true;
            labelTenVatTu.Visible = true;
            lblTenVatTu.Text = tenvattu;
        }

        private void AnTenVatTu()
        {
            lblTenVatTu.Visible = false;
            labelTenVatTu.Visible = false;
            lblTenVatTu.Text = "Tên vật tư";
        }

        private void btnTimKiemVatTu1_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            pageSize = 10000;
            LoadDSVatTu();
            string searchText = txtNoiDungTimKiem1.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);

            for (int i = 0; i < dataGridView_DSVatTuHoatDong.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTuHoatDong.Rows[i].Cells["Column2"].Value).ToLower();

                    if (tenVatTu1.Contains(searchText))
                    {
                        dataGridView_DSVatTuHoatDong.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTuHoatDong.Rows[i].Visible = false;
                    }
                }
                else
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTuHoatDong.Rows[i].Cells["Column2"].Value).ToLower();

                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu1.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_DSVatTuHoatDong.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_DSVatTuHoatDong.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem1_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem1.Text == string.Empty)
            {
                currentPage = 1;
                pageSize = 200;
                LoadDSVatTu();
                cbbDanhMuc_SelectedIndexChanged(sender, e);
            }
        }

        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            pageSize = 200;
            LoadDSVatTu();
            if (cbbDanhMuc.Text == "Tất cả danh mục")
            {
                for (int i = 0; i < dataGridView_DSVatTuHoatDong.Rows.Count; i++)
                {
                    dataGridView_DSVatTuHoatDong.Rows[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView_DSVatTuHoatDong.Rows.Count - 1; i++)
                {
                    string tenDanhMuc1 = ((string)dataGridView_DSVatTuHoatDong.Rows[i].Cells["Column5"].Value).ToLower();

                    if (tenDanhMuc1.Contains(cbbDanhMuc.Text.ToLower()))
                    {
                        dataGridView_DSVatTuHoatDong.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTuHoatDong.Rows[i].Visible = false;
                    }
                }
            }
        }

        private void ChuanHoaVaHienThi()
        {
            dataGridView_DSVatTuTrung.Rows.Clear();
            vatTuBiTrungs.Clear();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                for (int i = 0; i < vatTuHoatDongs.Count; i++)
                {
                    if (i != line_choising)
                    {
                        double similarity = function.CalculateCosineSimilarity(vattu_duocchon.tenvattu, vatTuHoatDongs[i].tenvattu);
                        similarity = similarity * 100;
                        if (similarity >= dogiongkhac)
                        {
                            VatTuTrung vtt = new VatTuTrung();
                            vtt.mavattu = vatTuHoatDongs[i].mavattu;
                            vtt.tenvattu = vatTuHoatDongs[i].tenvattu;
                            vtt.nguongoc = vatTuHoatDongs[i].nguongoc;
                            vtt.madanhmuc = vatTuHoatDongs[i].madanhmuc;
                            vtt.giongnhau = similarity;
                            vtt.tinhtrangxacnhan = 1;
                            vatTuBiTrungs.Add(vtt);
                        }
                        else // check theo từ khóa trùng nhau
                        {
                            var liststring = function.GetList_TuKhoVatTu_VatTuTrung();
                            if (liststring != null)
                            {
                                int dem = 0;
                                for (int k = 0; k < liststring.Count; k++)
                                {
                                    if (vattu_duocchon.tenvattu.Contains(liststring[k].tentukhoa.Trim()) || vatTuHoatDongs[i].tenvattu.Contains(liststring[k].tentukhoa.Trim()))
                                    {
                                        dem++;
                                    }
                                }
                                if (dem >= 2)
                                {
                                    VatTuTrung vtt = new VatTuTrung();
                                    vtt.mavattu = vatTuHoatDongs[i].mavattu;
                                    vtt.tenvattu = vatTuHoatDongs[i].tenvattu;
                                    vtt.nguongoc = vatTuHoatDongs[i].nguongoc;
                                    vtt.madanhmuc = vatTuHoatDongs[i].madanhmuc;
                                    vtt.madanhmuc = vatTuHoatDongs[i].madanhmuc;
                                    vtt.giongnhau = similarity;
                                    vtt.tinhtrangxacnhan = 1;
                                    vatTuBiTrungs.Add(vtt);
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < vatTuBiTrungs.Count; i++)
                {
                    // thêm vào datagridview
                    dataGridView_DSVatTuTrung.Rows.Add();
                    dataGridView_DSVatTuTrung.Rows[i].Cells["Column6"].Value = "Trùng";
                    dataGridView_DSVatTuTrung.Rows[i].Cells["Column7"].Value = vatTuBiTrungs[i].tenvattu;
                    dataGridView_DSVatTuTrung.Rows[i].Cells["Column8"].Value = vatTuBiTrungs[i].nguongoc;
                    string madanhmuc = (string)vatTuBiTrungs[i].madanhmuc;
                    var danhmuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    if (danhmuc != null)
                    {
                        dataGridView_DSVatTuTrung.Rows[i].Cells["Column9"].Value = danhmuc.tendanhmuc;
                    }
                    if (vatTuBiTrungs[i].giongnhau >= dogiongkhac)
                    {
                        dataGridView_DSVatTuTrung.Rows[i].Cells["Column10"].Value = Math.Round(vatTuBiTrungs[i].giongnhau, 2) + "%";
                    }
                    else
                    {
                        dataGridView_DSVatTuTrung.Rows[i].Cells["Column10"].Value = "Cùng nghĩa";
                    }
                }
            }
        }

        private void btnTimKiemVatTu2_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem2.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);

            for (int i = 0; i < dataGridView_DSVatTuTrung.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTuTrung.Rows[i].Cells["Column7"].Value).ToLower();

                    if (tenVatTu1.Contains(searchText))
                    {
                        dataGridView_DSVatTuTrung.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTuTrung.Rows[i].Visible = false;
                    }
                }
                else
                {
                    string tenVatTu1 = ((string)dataGridView_DSVatTuTrung.Rows[i].Cells["Column7"].Value).ToLower();

                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu1.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_DSVatTuTrung.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_DSVatTuTrung.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem2_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem2.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTuTrung.Rows.Count; i++)
                {
                    dataGridView_DSVatTuTrung.Rows[i].Visible = true;
                }
            }
        }

        private void dataGridView_DSVatTuTrung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTuTrung.Columns["Column6"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int rowID = e.RowIndex;
                    var cellValue = dataGridView_DSVatTuTrung.Rows[rowID].Cells["Column6"].Value?.ToString();
                    var tenvattu = dataGridView_DSVatTuTrung.Rows[rowID].Cells["Column7"].Value?.ToString();
                    var nguongoc = dataGridView_DSVatTuTrung.Rows[rowID].Cells["Column8"].Value?.ToString();

                    if (cellValue == "Trùng")
                    {
                        dataGridView_DSVatTuTrung.Rows[rowID].Cells["Column6"].Value = "Không trùng";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTuTrung.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }
                        var vattutrung = vatTuBiTrungs.FirstOrDefault(m => m.tenvattu == tenvattu && m.nguongoc == nguongoc);
                        vattutrung.tinhtrangxacnhan = 0;
                    }
                    else if (cellValue == "Không trùng")
                    {
                        dataGridView_DSVatTuTrung.Rows[rowID].Cells["Column6"].Value = "Trùng";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTuTrung.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.Moccasin;
                        }
                        var vattutrung = vatTuBiTrungs.FirstOrDefault(m => m.tenvattu == tenvattu && m.nguongoc == nguongoc);
                        vattutrung.tinhtrangxacnhan = 1;
                    }
                }
                catch { }
            }
        }

        private void LamMoi()
        {
            dataGridView_DSVatTuHoatDong.Rows.Clear();
            dataGridView_DSVatTuTrung.Rows.Clear();
            txtNoiDungTimKiem1.Text = string.Empty;
            txtNoiDungTimKiem2.Text = string.Empty;
            btnLocTuDong.Text = "Lọc tự động";
            cbbDanhMuc.Text = "Tất cả danh mục";
            vatTuBiTrungs.Clear();
            LoadDSVatTu();
            AnTenVatTu();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnLocTuDong90_Click(object sender, EventArgs e)
        {
            if (lblTenVatTu.Text != "Tên vật tư")
            {
                dogiongkhac = 90;
                btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
                groupPanel2.Text = $"Danh sách dự đoán là trùng nhau {dogiongkhac}%";
                ChuanHoaVaHienThi();
            }
        }

        private void btnLocTuDong80_Click(object sender, EventArgs e)
        {
            if (lblTenVatTu.Text != "Tên vật tư")
            {
                dogiongkhac = 80;
                btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
                groupPanel2.Text = $"Danh sách dự đoán là trùng nhau {dogiongkhac}%";
                ChuanHoaVaHienThi();
            }
        }

        private void btnLocTuDong70_Click(object sender, EventArgs e)
        {
            if (lblTenVatTu.Text != "Tên vật tư")
            {
                dogiongkhac = 70;
                btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
                groupPanel2.Text = $"Danh sách dự đoán là trùng nhau {dogiongkhac}%";
                ChuanHoaVaHienThi();
            }
        }

        private void btnLocTuDong60_Click(object sender, EventArgs e)
        {
            if (lblTenVatTu.Text != "Tên vật tư")
            {
                dogiongkhac = 60;
                btnLocTuDong.Text = $"Lọc tự động ({dogiongkhac}%)";
                groupPanel2.Text = $"Danh sách dự đoán là trùng nhau {dogiongkhac}%";
                ChuanHoaVaHienThi();
            }
        }

        private void btnLuuDanhSachDaLoc_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Lưu danh sách đã lọc", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    for (int i = 0; i < vatTuBiTrungs.Count; i++)
                    {
                        if (vatTuBiTrungs[i].tinhtrangxacnhan == 1)
                        {
                            int mavattu = (int)vatTuBiTrungs[i].mavattu;
                            var vattu = dbContext.VatTus.FirstOrDefault(m => m.mavattu == mavattu);
                            if (vattu != null)
                            {
                                vattu.nguoisuacuoi = frmDangNhap.tennguoidung;
                                vattu.thoigiansua = DateTime.Now;
                                vattu.trangthai = 2;
                            }
                        }
                    }

                    LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                    lichSuHoatDong.thoigian = DateTime.Now;
                    lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} đã lọc danh sách vật tư";
                    lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                    lichSuHoatDong.id = frmDangNhap.userID;
                    dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                    dbContext.SaveChanges();
                }
                MessageBox.Show("Lọc danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void groupPanel2_Resize(object sender, EventArgs e)
        {
            dataGridView_DSVatTuHoatDong.Columns["Column1"].Width = (int)(dataGridView_DSVatTuHoatDong.Width * 1 / 10);
            dataGridView_DSVatTuHoatDong.Columns["Column2"].Width = (int)(dataGridView_DSVatTuHoatDong.Width * 3 / 10);
            dataGridView_DSVatTuHoatDong.Columns["Column3"].Width = (int)(dataGridView_DSVatTuHoatDong.Width * 2 / 10);
            dataGridView_DSVatTuHoatDong.Columns["Column4"].Width = (int)(dataGridView_DSVatTuHoatDong.Width * 2 / 10);
            dataGridView_DSVatTuHoatDong.Columns["Column5"].Width = (int)(dataGridView_DSVatTuHoatDong.Width * 2 / 10);

            dataGridView_DSVatTuTrung.Columns["Column6"].Width = (int)(dataGridView_DSVatTuTrung.Width * 1 / 10);
            dataGridView_DSVatTuTrung.Columns["Column7"].Width = (int)(dataGridView_DSVatTuTrung.Width * 3 / 10);
            dataGridView_DSVatTuTrung.Columns["Column8"].Width = (int)(dataGridView_DSVatTuTrung.Width * 2 / 10);
            dataGridView_DSVatTuTrung.Columns["Column9"].Width = (int)(dataGridView_DSVatTuTrung.Width * 2 / 10);
            dataGridView_DSVatTuTrung.Columns["Column10"].Width = (int)(dataGridView_DSVatTuTrung.Width * 2 / 10);
        }

    }
}
