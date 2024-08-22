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
    public partial class usr_LapPhuongAnVatTu : UserControl
    {
        Function function = new Function();
        List<VatTu> DS_vatTus = new List<VatTu>();
        List<VatTu> DS_vatTus_DaChon = new List<VatTu>();
        List<ThongTinPhuongAnVatTu> DS_thongTinPhuongAnVatTus = new List<ThongTinPhuongAnVatTu>();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();


        public usr_LapPhuongAnVatTu()
        {
            InitializeComponent();
            dataGridView_DSVatTu.RowTemplate.Height = 35;
            dataGridView_DSVatTuDaChon.RowTemplate.Height = 35;
            LoadData();
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
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2) - 50;
            }
            catch { }
        }

        private void LoadData()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var vatTus = dbContext.VatTus.Where(m => m.trangthai == 1).OrderBy(m => m.tenvattu).ToList();
                DS_vatTus = vatTus;
                for (int i = 0; i < vatTus.Count; i++)
                {
                    dataGridView_DSVatTu.Rows.Add();
                    dataGridView_DSVatTu.Rows[i].Cells["Column1"].Value = "Xác định";
                    dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value = vatTus[i].tenvattu;
                    dataGridView_DSVatTu.Rows[i].Cells["Column3"].Value = vatTus[i].donvitinh;
                    try
                    {
                        string dongia = function.FormatDecimal((decimal)vatTus[i].dongia);
                        dataGridView_DSVatTu.Rows[i].Cells["Column4"].Value = dongia;
                    }
                    catch
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column4"].Value = 0;
                    }
                    string madanhmuc = (string)vatTus[i].madanhmuc;
                    DanhMuc danhmuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == madanhmuc);
                    if (danhmuc != null)
                    {
                        dataGridView_DSVatTu.Rows[i].Cells["Column5"].Value = danhmuc.tendanhmuc;
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem1.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);
            for (int i = 0; i < dataGridView_DSVatTu.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu = ((string)dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value).ToLower();

                    if (tenVatTu.Contains(searchText))
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
                    string tenVatTu = ((string)dataGridView_DSVatTu.Rows[i].Cells["Column2"].Value).ToLower();
                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu.Contains(liststring[j].Trim().ToLower()))
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
            if (txtNoiDungTimKiem1.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTu.Rows.Count; i++)
                {
                    dataGridView_DSVatTu.Rows[i].Visible = true;
                }
                cbbDanhMuc_SelectedIndexChanged(sender, e);
            }
        }

        private void dataGridView_DSVatTu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSVatTu.Columns["Column1"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int rowID = e.RowIndex;
                    var cellValue = dataGridView_DSVatTu.Rows[rowID].Cells["Column1"].Value?.ToString();

                    if (cellValue == "Chọn")
                    {
                        dataGridView_DSVatTu.Rows[rowID].Cells["Column1"].Value = "Không";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.White;
                        }

                        string tvt = (string)dataGridView_DSVatTu.Rows[rowID].Cells["Column2"].Value;
                        int id = DS_vatTus.FindIndex(m => m.tenvattu == tvt);
                        int mavattu = DS_vatTus[id].mavattu;
                        
                        VatTu vt = DS_vatTus_DaChon.SingleOrDefault(m => m.mavattu == mavattu);
                        ThongTinPhuongAnVatTu ttpavt = DS_thongTinPhuongAnVatTus.SingleOrDefault(m => m.mavattu == mavattu);
                        if (vt != null && ttpavt != null)
                        {
                            DS_vatTus_DaChon.Remove(vt);
                            DS_thongTinPhuongAnVatTus.Remove(ttpavt);
                            Load_DSVatTu_DaChon();
                        }
                    }
                    else if (cellValue == "Xác định" || cellValue == "Không")
                    {
                        dataGridView_DSVatTu.Rows[rowID].Cells["Column1"].Value = "Chọn";
                        foreach (DataGridViewCell cell in dataGridView_DSVatTu.Rows[rowID].Cells)
                        {
                            cell.Style.BackColor = Color.LightGreen;
                        }
                        string tvt = (string)dataGridView_DSVatTu.Rows[rowID].Cells["Column2"].Value;
                        int id = DS_vatTus.FindIndex(m => m.tenvattu == tvt);
                        DS_vatTus_DaChon.Add(DS_vatTus[id]);
                        ThongTinPhuongAnVatTu ttpavt = new ThongTinPhuongAnVatTu();
                        ttpavt.mavattu = DS_vatTus[id].mavattu;
                        DS_thongTinPhuongAnVatTus.Add(ttpavt);
                        Load_DSVatTu_DaChon();
                    }
                }
                catch { }
            }
        }

        private void Load_DSVatTu_DaChon()
        {
            dataGridView_DSVatTuDaChon.Rows.Clear();
            for (int i = 0; i < DS_vatTus_DaChon.Count; i++)
            {
                dataGridView_DSVatTuDaChon.Rows.Add();
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column6"].Value = i + 1;
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column7"].Value = DS_vatTus_DaChon[i].tenvattu;
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column8"].Value = DS_vatTus_DaChon[i].donvitinh;
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column9"].Value = DS_thongTinPhuongAnVatTus[i].soluong;
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column10"].Value = DS_thongTinPhuongAnVatTus[i].doicu;
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column11"].Value = DS_thongTinPhuongAnVatTus[i].capmoi;
                dataGridView_DSVatTuDaChon.Rows[i].Cells["Column12"].Value = DS_thongTinPhuongAnVatTus[i].ghichu;
            }
        }

        private void dataGridView_DSVatTuDaChon_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
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
                        dataGridView_DSVatTu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;

                        // Hủy bỏ chỉnh sửa và giữ người dùng trong ô
                        e.Cancel = true;
                    }
                    else
                    {
                        int value = Int32.Parse(newValue);
                        DS_thongTinPhuongAnVatTus[rowID].soluong = value;
                    }
                }
                else if (e.ColumnIndex == 4)
                {
                    string newValue = e.FormattedValue.ToString();
                    if (!int.TryParse(newValue, out int intValue) || intValue < 0)
                    {
                        MessageBox.Show("Cột đổi cũ phải là số nguyên không âm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView_DSVatTu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;
                        e.Cancel = true;
                    }
                    else
                    {
                        int value = Int32.Parse(newValue);
                        DS_thongTinPhuongAnVatTus[rowID].doicu = value;
                    }
                }
                else if (e.ColumnIndex == 5)
                {
                    string newValue = e.FormattedValue.ToString();
                    if (!int.TryParse(newValue, out int intValue) || intValue < 0)
                    {
                        MessageBox.Show("Cột cấp mới phải là số nguyên không âm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView_DSVatTu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;
                        e.Cancel = true;
                    }
                    else
                    {
                        int value = Int32.Parse(newValue);
                        DS_thongTinPhuongAnVatTus[rowID].capmoi = value;
                    }
                }
                else if (e.ColumnIndex == 6)
                {
                    string newValue = e.FormattedValue.ToString();
                    DS_thongTinPhuongAnVatTus[rowID].ghichu = newValue;
                }
            }
            catch { }
        }

        private void btnTimKiem2_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem2.Text.Trim().ToLower();
            var liststring = function.GetList_VatTuTrung(searchText);

            for (int i = 0; i < dataGridView_DSVatTuDaChon.Rows.Count - 1; i++)
            {
                if (liststring == null)
                {
                    string tenVatTu = ((string)dataGridView_DSVatTuDaChon.Rows[i].Cells["Column7"].Value).ToLower();

                    if (tenVatTu.Contains(searchText))
                    {
                        dataGridView_DSVatTuDaChon.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView_DSVatTuDaChon.Rows[i].Visible = false;
                    }
                }
                else
                {
                    string tenVatTu = ((string)dataGridView_DSVatTuDaChon.Rows[i].Cells["Column7"].Value).ToLower();
                    for (int j = 0; j < liststring.Count; j++)
                    {
                        if (tenVatTu.Contains(liststring[j].Trim().ToLower()))
                        {
                            dataGridView_DSVatTuDaChon.Rows[i].Visible = true;
                            break;
                        }
                        else
                        {
                            dataGridView_DSVatTuDaChon.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void txtNoiDungTimKiem2_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem2.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSVatTuDaChon.Rows.Count; i++)
                {
                    dataGridView_DSVatTuDaChon.Rows[i].Visible = true;
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtNoiDungTimKiem1.Text = string.Empty;
            txtNoiDungTimKiem2.Text = string.Empty;
            txtTenPhuongAn.Text = string.Empty;
            cbbDanhMuc.Text = "Tất cả danh mục";
            dataGridView_DSVatTu.Rows.Clear();
            dataGridView_DSVatTuDaChon.Rows.Clear();
            DS_vatTus.Clear();
            DS_vatTus_DaChon.Clear();
            DS_thongTinPhuongAnVatTus.Clear();

            LoadData();
        }

        private decimal TinhTongTien()
        {
            decimal tongtien = 0;
            for (int i = 0; i < DS_vatTus_DaChon.Count; i++)
            {
                tongtien += (decimal)DS_vatTus_DaChon[i].dongia * (decimal)DS_thongTinPhuongAnVatTus[i].soluong;
            }
            return tongtien;
        }

        private bool KiemTraGiaTriCuaCot()
        {
            // Lặp qua tất cả các hàng của DataGridView
            foreach (DataGridViewRow row in dataGridView_DSVatTuDaChon.Rows)
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
                        MessageBox.Show("Có một hàng trong cột thứ 3 có giá trị bằng 0. Đây là một lỗi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Dừng kiểm tra sau khi phát hiện lỗi
                    }
                }
            }
            return true;
        }


        private void btnXacNhanPhuongAn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Xác nhận lập phương án vật tư mới?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (KiemTraGiaTriCuaCot())
                {
                    if (DS_vatTus_DaChon.Count > 0)
                    {
                        if (txtTenPhuongAn.Text != string.Empty)
                        {
                            using (var dbContext = new QuanLyVatTuDbContext())
                            {
                                PhuongAnVatTu pavt = new PhuongAnVatTu();
                                pavt.tenphuongan = txtTenPhuongAn.Text;
                                pavt.tongtien = TinhTongTien();
                                pavt.nguoilap = frmDangNhap.tennguoidung;
                                pavt.thoigianlap = DateTime.Now;
                                pavt.user_id = frmDangNhap.userID;
                                dbContext.PhuongAnVatTus.Add(pavt);
                                dbContext.SaveChanges();

                                for (int i = 0; i < DS_vatTus_DaChon.Count; i++)
                                {
                                    ChiTietPhuongAn ctpa = new ChiTietPhuongAn();
                                    ctpa.tenvattu = DS_vatTus_DaChon[i].tenvattu;
                                    ctpa.donvitinh = DS_vatTus_DaChon[i].donvitinh;
                                    ctpa.soluong = DS_thongTinPhuongAnVatTus[i].soluong;
                                    ctpa.doicu = DS_thongTinPhuongAnVatTus[i].doicu;
                                    ctpa.capmoi = DS_thongTinPhuongAnVatTus[i].capmoi;
                                    ctpa.dongia = DS_vatTus_DaChon[i].dongia;
                                    ctpa.thanhtien = DS_thongTinPhuongAnVatTus[i].soluong * DS_vatTus_DaChon[i].dongia;
                                    ctpa.ghichu = DS_thongTinPhuongAnVatTus[i].ghichu;
                                    ctpa.maphuongan = pavt.maphuongan;
                                    ctpa.mavattu = DS_vatTus_DaChon[i].mavattu;
                                    dbContext.ChiTietPhuongAns.Add(ctpa);
                                }
                                dbContext.SaveChanges();

                                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                                lichSuHoatDong.thoigian = DateTime.Now;
                                lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} lập phương án vật tư {txtTenPhuongAn.Text}";
                                lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                                lichSuHoatDong.id = frmDangNhap.userID;
                                dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                                dbContext.SaveChanges();
                            }
                            MessageBox.Show("Lập phương án vật tư thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnLamMoi_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Chưa nhập tên phương án", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lập phương án mà chưa chọn vật tư", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Còn vật tư có số lượng bằng 0", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    string tenDanhMuc = ((string)dataGridView_DSVatTu.Rows[i].Cells["Column5"].Value).ToLower();

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