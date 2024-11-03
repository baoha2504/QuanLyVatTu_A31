using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmChiTietAnhVatTu : Form
    {
        List<AnhVatTu> anhVatTus = new List<AnhVatTu>();
        int mavattu = 0;
        string tenvattu = "";
        ToolTip toolTip = new ToolTip();
        string path_anh1 = "";
        string path_anh2 = "";
        string path_anh3 = "";
        string path_anh4 = "";

        public frmChiTietAnhVatTu(int mavattu, string tenvattu)
        {
            InitializeComponent();
            toolTip.SetToolTip(pictureBox1, "Kích đúp chuột vào để thay đổi ảnh mới");
            toolTip.SetToolTip(pictureBox2, "Kích đúp chuột vào để thay đổi ảnh mới");
            toolTip.SetToolTip(pictureBox3, "Kích đúp chuột vào để thay đổi ảnh mới");
            toolTip.SetToolTip(pictureBox4, "Kích đúp chuột vào để thay đổi ảnh mới");
            this.Text += $": {tenvattu}";
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                var ds_anh = dbContext.AnhVatTus.Where(m => m.mavattu == mavattu).ToList();
                anhVatTus = ds_anh;
                this.mavattu = mavattu;
                this.tenvattu = tenvattu;
            }
            LoadImage();
        }

        private void LoadImage()
        {
            foreach (var item in anhVatTus)
            {
                if (item != null && item.tt_anh >= 1 && item.tt_anh <= 4)
                {
                    if (item.tt_anh == 1)
                    {
                        if (System.IO.File.Exists(item.duongdananh))
                        {
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox1.Image = Image.FromFile(item.duongdananh);
                        }
                        else
                        {
                            //MessageBox.Show("File ảnh 1 không tồn tại.");
                        }
                    }
                    else if (item.tt_anh == 2)
                    {
                        if (System.IO.File.Exists(item.duongdananh))
                        {
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Image = Image.FromFile(item.duongdananh);
                        }
                        else
                        {
                            //MessageBox.Show("File ảnh 2 không tồn tại.");
                        }
                    }
                    else if (item.tt_anh == 3)
                    {
                        if (System.IO.File.Exists(item.duongdananh))
                        {
                            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox3.Image = Image.FromFile(item.duongdananh);
                        }
                        else
                        {
                            //MessageBox.Show("File ảnh 3 không tồn tại.");
                        }
                    }
                    else if (item.tt_anh == 4)
                    {
                        if (System.IO.File.Exists(item.duongdananh))
                        {
                            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox4.Image = Image.FromFile(item.duongdananh);
                        }
                        else
                        {
                            //MessageBox.Show("File ảnh 4 không tồn tại.");
                        }
                    }
                }
            }
        }

        private void pictureBox1_DoubleClick(object sender, System.EventArgs e)
        {
            ChonAnh(1);
        }

        private void pictureBox2_DoubleClick(object sender, System.EventArgs e)
        {
            ChonAnh(2);
        }

        private void pictureBox3_DoubleClick(object sender, System.EventArgs e)
        {
            ChonAnh(3);
        }

        private void pictureBox4_DoubleClick(object sender, System.EventArgs e)
        {
            ChonAnh(4);
        }

        private void ChonAnh(int tt_anh)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh mới";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (tt_anh == 1)
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
                    }
                    else if (tt_anh == 2)
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
                    }
                    else if (tt_anh == 3)
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
                    }
                    else if (tt_anh == 4)
                    {
                        if (pictureBox4.Image != null)
                        {
                            pictureBox4.Image.Dispose();
                            pictureBox4.Image = null;
                        }
                        using (var image = Image.FromFile(openFileDialog.FileName))
                        {
                            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox4.Image = new Bitmap(image);
                        }
                        path_anh4 = openFileDialog.FileName;
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                SaveOrUpdateImage(dbContext, path_anh1, mavattu, 1);
                SaveOrUpdateImage(dbContext, path_anh2, mavattu, 2);
                SaveOrUpdateImage(dbContext, path_anh3, mavattu, 3);
                SaveOrUpdateImage(dbContext, path_anh4, mavattu, 4);
                dbContext.SaveChanges();
                MessageBox.Show("Lưu thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        public string CheckAndCopyImage(string sourceFilePath, string destinationFolderName, string destinationFileName)
        {
            // Đường dẫn thư mục gốc
            string destinationDirectory = @"C:\QuanLyVatTu\Data\AnhVatTu";

            // Tạo thư mục con với tên từ file destination (không có phần mở rộng)
            destinationDirectory = Path.Combine(destinationDirectory, destinationFolderName);

            // Kiểm tra nếu thư mục đích chưa tồn tại, thì tạo thư mục
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            // Tạo đường dẫn file đích với tên file từ destinationFilePath trong thư mục đích
            string destinationFullPath = Path.Combine(destinationDirectory, Path.GetFileName(destinationFileName));

            // Sao chép file từ sourceFilePath đến destinationFullPath, ghi đè nếu đã tồn tại
            File.Copy(sourceFilePath, destinationFullPath, true);

            return destinationFullPath;
        }

        private void SaveOrUpdateImage(QuanLyVatTuDbContext dbContext, string imagePath, int mavattu, int imageType)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                var anh = dbContext.AnhVatTus.FirstOrDefault(m => m.mavattu == mavattu && m.tt_anh == imageType);
                if (anh != null)
                {
                    string destinationFullPath = CheckAndCopyImage(imagePath, tenvattu, $"{imageType}{Path.GetExtension(imagePath)}");
                    anh.duongdananh = destinationFullPath;
                }
                else
                {
                    string destinationFullPath = CheckAndCopyImage(imagePath, tenvattu, $"{imageType}{Path.GetExtension(imagePath)}");
                    var newAnh = new AnhVatTu
                    {
                        duongdananh = destinationFullPath,
                        mavattu = mavattu,
                        tt_anh = imageType
                    };
                    dbContext.AnhVatTus.Add(newAnh);
                }
            }
        }
    }
}
