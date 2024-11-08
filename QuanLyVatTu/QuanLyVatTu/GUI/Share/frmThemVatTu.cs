using Newtonsoft.Json;
using QuanLyVatTu.Class;
using QuanLyVatTu.Model;
using QuanLyVatTu.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace QuanLyVatTu.GUI.Share
{
    public partial class frmThemVatTu : Form
    {
        Function function = new Function();
        List<DanhMuc> listdanhmuc = new List<DanhMuc>();
        VatTu vt_save = new VatTu();
        private System.Timers.Timer typingTimer;
        private System.Timers.Timer typingTimer1;
        private const int TypingDelay = 1000;
        private const int TypingDelay1 = 2000;
        private string tenvattu_bandau = "";
        ToolTip toolTip = new ToolTip();
        string path_anh1 = "";
        string path_anh2 = "";
        string path_anh3 = "";
        bool checkModifyImage1 = false;
        bool checkModifyImage2 = false;
        bool checkModifyImage3 = false;

        private static readonly HttpClient client = new HttpClient();

        public frmThemVatTu()
        {
            InitializeComponent();
            btnAnhVatTu.Enabled = false;

            typingTimer = new System.Timers.Timer(TypingDelay);
            typingTimer.Elapsed += OnTypingTimerElapsed;
            typingTimer.AutoReset = false;

            typingTimer1 = new System.Timers.Timer(TypingDelay1);
            typingTimer1.Elapsed += OnTypingTimerElapsed1;
            typingTimer1.AutoReset = false;

            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var danhMucs = dbContext.DanhMucs.ToList();
                listdanhmuc = danhMucs;
                txtDanhMuc.Items.Clear();
                foreach (var dm in danhMucs)
                {
                    txtDanhMuc.Items.Add(dm.tendanhmuc);
                }
            }
            txtTinhTrang.Text = "Đang sử dụng";
            txtNguoiSuaCuoi.Text = frmDangNhap.tennguoidung;
            txtThoiGianSua.Text = DateTime.Now.ToString("dd/MM/yyyy");
            AddToolTip();
        }

        public frmThemVatTu(VatTu vt)
        {
            InitializeComponent();
            vt_save = vt;

            VatTu vatTu = new VatTu();
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                vatTu = dbContext.VatTus.FirstOrDefault(m => m.mavattu == vt.mavattu);
            }

            typingTimer = new System.Timers.Timer(TypingDelay);
            typingTimer.Elapsed += OnTypingTimerElapsed;
            typingTimer.AutoReset = false;

            txtMaVatTu.Text = vatTu.mavattu_hethong;
            txtTenVatTu.Text = vatTu.tenvattu;
            tenvattu_bandau = vatTu.tenvattu;
            txtDonViTinh.Text = vatTu.donvitinh;
            txtDonGia.Text = function.FormatDecimal((decimal)vatTu.dongia);
            txtNguonGoc.Text = vatTu.nguongoc;
            txtThongSoKyThuat.Text = vatTu.thongsokythuat;
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var danhMucs = dbContext.DanhMucs.ToList();
                listdanhmuc = danhMucs;
                txtDanhMuc.Items.Clear();
                foreach (var dm in danhMucs)
                {
                    txtDanhMuc.Items.Add(dm.tendanhmuc);
                }

                DanhMuc danhMuc = dbContext.DanhMucs.SingleOrDefault(m => m.madanhmuc == vatTu.madanhmuc);
                if (danhMuc != null)
                {
                    txtDanhMuc.Text = danhMuc.tendanhmuc;
                }
            }
            if (vatTu.trangthai == 0)
            {
                txtTinhTrang.Text = "Dừng sử dụng";
            }
            else if (vatTu.trangthai == 1)
            {
                txtTinhTrang.Text = "Đang sử dụng";
            }
            else if (vatTu.trangthai == 2)
            {
                txtTinhTrang.Text = "Bị trùng";
            }

            txtGhiChu.Text = vatTu.ghichu;
            txtNguoiSuaCuoi.Text = frmDangNhap.tennguoidung;
            txtThoiGianSua.Text = DateTime.Now.ToString("dd/MM/yyyy");
            AddToolTip();
            _ = LoadImageAsync();
        }

        private void AddToolTip()
        {
            toolTip.SetToolTip(pictureBox1, "Kích đúp chuột vào để thay đổi ảnh mới");
            toolTip.SetToolTip(pictureBox2, "Kích đúp chuột vào để thay đổi ảnh mới");
            toolTip.SetToolTip(pictureBox3, "Kích đúp chuột vào để thay đổi ảnh mới");
        }

        private bool CheckEmptyInput()
        {
            return !string.IsNullOrWhiteSpace(txtTenVatTu.Text) &&
                   !string.IsNullOrWhiteSpace(txtDonViTinh.Text) &&
                   !string.IsNullOrWhiteSpace(txtDanhMuc.Text) &&
                   !string.IsNullOrWhiteSpace(txtTinhTrang.Text);
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (this.Text != "Sửa thông tin vật tư")
            {
                if (CheckEmptyInput())
                {
                    if (CheckDistance())
                    {
                        using (var dbContext = new QuanLyVatTuDbContext())
                        {
                            // Thêm thông tin vật tư
                            if (IsValidVarchar255(txtMaVatTu))
                            {
                                bool check = false;
                                var vt_find = dbContext.VatTus.FirstOrDefault(m => m.mavattu_hethong == txtMaVatTu.Text);
                                if (string.IsNullOrEmpty(txtMaVatTu.Text))
                                {
                                    check = true;
                                }
                                if (vt_find == null || check == true)
                                {
                                    VatTu vt = new VatTu();
                                    if (check != true)
                                    {
                                        vt.mavattu_hethong = txtMaVatTu.Text;
                                    }
                                    vt.tenvattu = txtTenVatTu.Text;
                                    vt.donvitinh = txtDonViTinh.Text;
                                    try
                                    {
                                        vt.dongia = function.ConvertStringToDecimal(txtDonGia.Text);
                                    }
                                    catch
                                    {
                                        vt.dongia = 0;
                                    }

                                    vt.nguongoc = txtNguonGoc.Text;
                                    vt.thongsokythuat = txtThongSoKyThuat.Text;
                                    if (txtTinhTrang.Text == "Đang sử dụng")
                                    {
                                        vt.trangthai = 1;
                                    }
                                    else if (txtTinhTrang.Text == "Dừng sử dụng")
                                    {
                                        vt.trangthai = 0;
                                    }
                                    else if (txtTinhTrang.Text == "Bị trùng")
                                    {
                                        vt.trangthai = 2;
                                    }
                                    vt.ghichu = txtGhiChu.Text;
                                    vt.nguoisuacuoi = frmDangNhap.tennguoidung;
                                    vt.thoigiansua = DateTime.Now;
                                    vt.user_id = frmDangNhap.userID;
                                    DanhMuc dm = dbContext.DanhMucs.SingleOrDefault(m => m.tendanhmuc == txtDanhMuc.Text);
                                    vt.madanhmuc = dm.madanhmuc;
                                    dbContext.VatTus.Add(vt);
                                    dbContext.SaveChanges();

                                    vt_save = vt;

                                    LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                                    lichSuHoatDong.thoigian = DateTime.Now;
                                    lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} thêm vật tư mới {vt.mavattu} - {vt.tenvattu}";
                                    lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                                    lichSuHoatDong.id = frmDangNhap.userID;
                                    dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                                    dbContext.SaveChanges();

                                    //DialogResult result = MessageBox.Show("Thêm vật tư mới thành công. Bạn có muốn thêm ảnh vật tư không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                    //if (result == DialogResult.OK)
                                    //{
                                    //    frmChiTietAnhVatTu frm = new frmChiTietAnhVatTu(vt.mavattu, vt.tenvattu);
                                    //    frm.ShowDialog();
                                    //}

                                    SaveImage();

                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Giá trị mã vật tư đã tồn tại hoặc trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Giá trị mã vật tư không hợp lệ?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Hãy nhập đầy đủ các thông tin bắt buộc đánh dấu '*'", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Sửa thông tin vật tư
                if (CheckDistance())
                {
                    using (var dbContext = new QuanLyVatTuDbContext())
                    {
                        if (IsValidVarchar255(txtMaVatTu))
                        {
                            bool check1 = false;
                            bool check2 = false;
                            var vt_find = dbContext.VatTus.FirstOrDefault(m => m.mavattu_hethong == txtMaVatTu.Text);
                            if (string.IsNullOrEmpty(txtMaVatTu.Text))
                            {
                                check1 = true;
                            }
                            if (txtMaVatTu.Text.ToLower().Trim() == vt_save.mavattu_hethong.ToLower().Trim())
                            {
                                check2 = true;
                            }
                            if (vt_find == null || check1 == true || check2 == true)
                            {
                                int mavattu = vt_save.mavattu;
                                VatTu vt = dbContext.VatTus.SingleOrDefault(m => m.mavattu == mavattu);
                                if (check1 != true)
                                {
                                    vt.mavattu_hethong = txtMaVatTu.Text;
                                }
                                vt.tenvattu = txtTenVatTu.Text;
                                vt.donvitinh = txtDonViTinh.Text;
                                try
                                {
                                    vt.dongia = function.ConvertStringToDecimal(txtDonGia.Text);
                                }
                                catch
                                {
                                    vt.dongia = 0;
                                }

                                vt.nguongoc = txtNguonGoc.Text;
                                vt.thongsokythuat = txtThongSoKyThuat.Text;
                                if (txtTinhTrang.Text == "Đang sử dụng")
                                {
                                    vt.trangthai = 1;
                                }
                                else if (txtTinhTrang.Text == "Dừng sử dụng")
                                {
                                    vt.trangthai = 0;
                                }
                                else if (txtTinhTrang.Text == "Bị trùng")
                                {
                                    vt.trangthai = 2;
                                }
                                vt.ghichu = txtGhiChu.Text;
                                vt.nguoisuacuoi = frmDangNhap.tennguoidung;
                                vt.thoigiansua = DateTime.Now;
                                vt.user_id = frmDangNhap.userID;
                                DanhMuc dm = dbContext.DanhMucs.SingleOrDefault(m => m.tendanhmuc == txtDanhMuc.Text);
                                vt.madanhmuc = dm.madanhmuc;
                                dbContext.SaveChanges();

                                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                                lichSuHoatDong.thoigian = DateTime.Now;
                                lichSuHoatDong.hoatdong = $"Tài khoản {frmDangNhap.userID} - {frmDangNhap.tennguoidung} sửa thông tin vật tư {vt.mavattu} - {vt.tenvattu}";
                                lichSuHoatDong.tennguoidung = frmDangNhap.tennguoidung;
                                lichSuHoatDong.id = frmDangNhap.userID;
                                dbContext.LichSuHoatDongs.Add(lichSuHoatDong);
                                dbContext.SaveChanges();

                                SaveImage();

                                MessageBox.Show("Sửa thông tin vật tư thành công");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Giá trị mã vật tư đã tồn tại hoặc trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Giá trị mã vật tư không hợp lệ?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                }
            }
        }

        private bool IsValidVarchar255(TextBox textBox)
        {
            string text = textBox.Text;

            // Kiểm tra độ dài có vượt quá 255 ký tự hay không
            if (text.Length > 255)
            {
                MessageBox.Show("Giá trị quá dài! Không được vượt quá 255 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra ký tự đặc biệt không hợp lệ (nếu cần)
            // Ví dụ, nếu không muốn các ký tự như ';', '"', '\'', '--' để tránh SQL Injection
            string invalidChars = ";\"'--";
            foreach (char c in invalidChars)
            {
                if (text.Contains(c))
                {
                    MessageBox.Show("Giá trị chứa ký tự không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Giá trị hợp lệ cho VARCHAR(255)
            return true;
        }

        private bool CheckDistance()
        {
            if (tenvattu_bandau == txtTenVatTu.Text)
            {
                return true;
            }
            else
            {
                double dogiongkhac = 70;
                string tenvattu_giong = "";
                using (var dbContext = new QuanLyVatTuDbContext())
                {
                    var list_Vattu = dbContext.VatTus.ToList();
                    for (int i = 0; i < list_Vattu.Count; i++)
                    {
                        double dogiongkhac_tamthoi = function.CalculateCosineSimilarity(txtTenVatTu.Text, list_Vattu[i].tenvattu);
                        dogiongkhac_tamthoi = dogiongkhac_tamthoi * 100;
                        if (dogiongkhac_tamthoi > dogiongkhac)
                        {
                            dogiongkhac = dogiongkhac_tamthoi;
                            tenvattu_giong = list_Vattu[i].tenvattu;
                        }
                    }
                }


                if (tenvattu_giong != "")
                {
                    if (dogiongkhac >= 99)
                    {
                        DialogResult result = MessageBox.Show($"Vật tư bạn muốn thêm giống {Math.Round(dogiongkhac, 2)}% vật tư đã có: '{tenvattu_giong}'. Vui lòng kiểm tra lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show($"Vật tư bạn muốn thêm giống {Math.Round(dogiongkhac, 2)}% vật tư đã có: '{tenvattu_giong}'. Nếu vẫn tiếp tục muốn tiếp tục, hãy xác nhận!", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {
            typingTimer.Stop();
            typingTimer.Start();

        }

        private void OnTypingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(ExecuteFunctionAfterTyping));
        }

        private void ExecuteFunctionAfterTyping()
        {
            try
            {
                txtDonGia.Text = function.FormatDecimal(Decimal.Parse(txtDonGia.Text));
            }
            catch { }
        }

        private void OnTypingTimerElapsed1(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(ExecuteFunctionAfterTyping1));
        }

        private void ExecuteFunctionAfterTyping1()
        {
            try
            {
                txtTenVatTu.Text = function.NormalizeString(txtTenVatTu.Text);
            }
            catch { }
        }

        private void txtTenVatTu_TextChanged(object sender, EventArgs e)
        {
            //typingTimer1.Stop();
            //typingTimer1.Start();
        }

        private void btnAnhVatTu_Click(object sender, EventArgs e)
        {
            frmChiTietAnhVatTu frm = new frmChiTietAnhVatTu(vt_save.mavattu, txtTenVatTu.Text);
            frm.ShowDialog();
        }

        public static T MinOfThree<T>(T a, T b, T c) where T : IComparable<T>
        {
            T min = a;

            if (b.CompareTo(min) < 0)
            {
                min = b;
            }

            if (c.CompareTo(min) < 0)
            {
                min = c;
            }

            return min;
        }

        private void frmThemVatTu_Resize(object sender, EventArgs e)
        {
            panel64.Height = (int)(262 * panel63.Height / 330);
            panel_picture2_3.Height = (int)(262 * panel63.Height / 330);
            panel_picture1.Width = 300;
            panel6.Width = (int)((panel1.Width - 300) / 2);

            panel63.Width = (int)(panel2.Width - 420);

            groupPanel2.Height = (int)(panel_picture2_3.Height / 2);
            groupPanel3.Height = (int)(panel_picture2_3.Height / 2);

            int minInt = MinOfThree(groupPanel1.Height, groupPanel2.Height, groupPanel3.Height);

            groupPanel1.Height = minInt;
            groupPanel2.Height = minInt;
            groupPanel3.Height = minInt;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh mới";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                    }
                    using (var image = Image.FromFile(openFileDialog.FileName))
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Image = new Bitmap(image);
                    }
                    path_anh1 = openFileDialog.FileName;
                    checkModifyImage1 = true;
                }
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh mới";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (pictureBox2.Image != null)
                    {
                        pictureBox2.Image.Dispose();
                        pictureBox2.Image = null;
                    }
                    using (var image = Image.FromFile(openFileDialog.FileName))
                    {
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox2.Image = new Bitmap(image);
                    }
                    path_anh2 = openFileDialog.FileName;
                    checkModifyImage2 = true;
                }
            }
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh mới";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (pictureBox3.Image != null)
                    {
                        pictureBox3.Image.Dispose();
                        pictureBox3.Image = null;
                    }
                    using (var image = Image.FromFile(openFileDialog.FileName))
                    {
                        pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox3.Image = new Bitmap(image);
                    }
                    path_anh3 = openFileDialog.FileName;
                    checkModifyImage3 = true;
                }
            }
        }

        public async Task UploadImageAsync(string pathImage, string folderName, string imageName)
        {
            // Kiểm tra file ảnh có tồn tại không
            if (!File.Exists(pathImage))
            {
                Console.WriteLine("File ảnh không tồn tại.");
                return;
            }

            // Đọc file ảnh và chuyển đổi thành Base64
            byte[] imageBytes = File.ReadAllBytes(pathImage);
            string imageBase64 = Convert.ToBase64String(imageBytes);

            // Tạo payload JSON để gửi lên server
            var payload = new
            {
                image_base64 = imageBase64,
                folder_name = folderName,
                file_name = imageName
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                // Gửi POST request tới API
                string apiUrl = $"http://{BienDungChung.serverIP}:9000/upload_image"; //
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Kiểm tra kết quả trả về từ server
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Tải lên ảnh thành công: " + responseBody);
                }
                else
                {
                    Console.WriteLine("Lỗi khi tải lên ảnh: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi gửi yêu cầu: " + ex.Message);
            }
        }

        private async Task LoadImageAsync()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                if (vt_save != null)
                {
                    var list_image = dbContext.AnhVatTus.Where(m => m.mavattu == vt_save.mavattu).ToList();
                    if (list_image != null)
                    {
                        string url = $"http://{BienDungChung.serverIP}:9000";
                        foreach (var image in list_image)
                        {
                            if (image.tt_anh >= 1 && image.tt_anh <= 3) // Đảm bảo tt_anh nằm trong phạm vi các pictureBox
                            {
                                try
                                {
                                    // Tải ảnh trong một luồng khác
                                    byte[] imageBytes = await Task.Run(async () =>
                                    {
                                        using (HttpClient client = new HttpClient())
                                        {
                                            return await client.GetByteArrayAsync($"{url}{image.duongdananh}");
                                        }
                                    });

                                    // Cập nhật UI trong luồng chính
                                    UpdatePictureBox(imageBytes, (int)image.tt_anh);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Lỗi khi tải ảnh {image.tt_anh}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePictureBox(byte[] imageBytes, int pictureBoxNumber)
        {
            if (imageBytes == null) return;

            // Thực hiện cập nhật UI trong luồng chính
            this.Invoke(new Action(() =>
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    switch (pictureBoxNumber)
                    {
                        case 1:
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox1.Image = image;
                            break;
                        case 2:
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Image = image;
                            break;
                        case 3:
                            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox3.Image = image;
                            break;
                    }
                }
            }));
        }

        private async void SaveImage()
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                if (checkModifyImage1)
                {
                    await UploadImageAsync(path_anh1, vt_save.mavattu.ToString(), $"1{Path.GetExtension(path_anh1)}");
                    if (this.Text == "Thêm vật tư")
                    {
                        AnhVatTu anhVatTu = new AnhVatTu();
                        anhVatTu.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/1{Path.GetExtension(path_anh1)}";
                        anhVatTu.mavattu = vt_save.mavattu;
                        anhVatTu.tt_anh = 1;
                        dbContext.AnhVatTus.Add(anhVatTu);
                        dbContext.SaveChanges();
                    }
                    else if (!string.IsNullOrEmpty(path_anh1))
                    {
                        var anhvattu1 = dbContext.AnhVatTus.FirstOrDefault(m => m.mavattu == vt_save.mavattu && m.tt_anh == 1);
                        if (anhvattu1 == null)
                        {
                            AnhVatTu anhVatTu = new AnhVatTu();
                            anhVatTu.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/1{Path.GetExtension(path_anh1)}";
                            anhVatTu.mavattu = vt_save.mavattu;
                            anhVatTu.tt_anh = 1;
                            dbContext.AnhVatTus.Add(anhVatTu);
                        }
                        else
                        {
                            anhvattu1.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/1{Path.GetExtension(path_anh1)}";
                        }
                        dbContext.SaveChanges();
                    }
                }

                if (checkModifyImage2)
                {
                    await UploadImageAsync(path_anh2, vt_save.mavattu.ToString(), $"2{Path.GetExtension(path_anh2)}");
                    if (this.Text == "Thêm vật tư")
                    {
                        AnhVatTu anhVatTu = new AnhVatTu();
                        anhVatTu.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/2{Path.GetExtension(path_anh2)}";
                        anhVatTu.mavattu = vt_save.mavattu;
                        anhVatTu.tt_anh = 2;
                        dbContext.AnhVatTus.Add(anhVatTu);
                        dbContext.SaveChanges();
                    }
                    else if (!string.IsNullOrEmpty(path_anh2))
                    {
                        var anhvattu2 = dbContext.AnhVatTus.FirstOrDefault(m => m.mavattu == vt_save.mavattu && m.tt_anh == 2);
                        if (anhvattu2 == null)
                        {
                            AnhVatTu anhVatTu = new AnhVatTu();
                            anhVatTu.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/2{Path.GetExtension(path_anh2)}";
                            anhVatTu.mavattu = vt_save.mavattu;
                            anhVatTu.tt_anh = 2;
                            dbContext.AnhVatTus.Add(anhVatTu);
                        }
                        else
                        {
                            anhvattu2.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/2{Path.GetExtension(path_anh2)}";
                        }
                        dbContext.SaveChanges();
                    }
                }

                if (checkModifyImage3 || !string.IsNullOrEmpty(path_anh3))
                {
                    await UploadImageAsync(path_anh3, vt_save.mavattu.ToString(), $"3{Path.GetExtension(path_anh3)}");
                    if (this.Text == "Thêm vật tư")
                    {
                        AnhVatTu anhVatTu = new AnhVatTu();
                        anhVatTu.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/3{Path.GetExtension(path_anh3)}";
                        anhVatTu.mavattu = vt_save.mavattu;
                        anhVatTu.tt_anh = 3;
                        dbContext.AnhVatTus.Add(anhVatTu);
                        dbContext.SaveChanges();
                    }
                    else if (!string.IsNullOrEmpty(path_anh3))
                    {
                        var anhvattu3 = dbContext.AnhVatTus.FirstOrDefault(m => m.mavattu == vt_save.mavattu && m.tt_anh == 3);
                        if (anhvattu3 == null)
                        {
                            AnhVatTu anhVatTu = new AnhVatTu();
                            anhVatTu.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/3{Path.GetExtension(path_anh3)}";
                            anhVatTu.mavattu = vt_save.mavattu;
                            anhVatTu.tt_anh = 3;
                            dbContext.AnhVatTus.Add(anhVatTu);
                        }
                        else
                        {
                            anhvattu3.duongdananh = $"/upload/{vt_save.mavattu.ToString()}/3{Path.GetExtension(path_anh3)}";
                        }
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        private void btnPhongToAnh1_Click(object sender, EventArgs e)
        {
            frm_XemAnh frm_XemAnh = new frm_XemAnh($"Ảnh 1 của vật tư: {vt_save.tenvattu}", pictureBox1.Image);
            frm_XemAnh.Show();
        }

        private void btnThayDoiAnh1_Click(object sender, EventArgs e)
        {
            pictureBox1_DoubleClick(sender, e);
        }

        private void btnPhongToAnh2_Click(object sender, EventArgs e)
        {
            frm_XemAnh frm_XemAnh = new frm_XemAnh($"Ảnh 2 của vật tư: {vt_save.tenvattu}", pictureBox1.Image);
            frm_XemAnh.Show();
        }

        private void btnThayDoiAnh2_Click(object sender, EventArgs e)
        {
            pictureBox2_DoubleClick(sender, e);
        }

        private void btnPhongToAnh3_Click(object sender, EventArgs e)
        {
            frm_XemAnh frm_XemAnh = new frm_XemAnh($"Ảnh 3 của vật tư: {vt_save.tenvattu}", pictureBox1.Image);
            frm_XemAnh.Show();
        }

        private void btnThayDoiAnh3_Click(object sender, EventArgs e)
        {
            pictureBox3_DoubleClick(sender, e);
        }
    }
}