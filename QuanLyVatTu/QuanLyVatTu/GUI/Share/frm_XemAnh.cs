using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frm_XemAnh : Form
    {
        public frm_XemAnh()
        {
            InitializeComponent();
        }

        private float zoomFactor = 1.0f;
        private Image originalImage;

        public frm_XemAnh(string text, Image image)
        {
            InitializeComponent();
            LoadImage(image);
            this.Text = text;
            pictureBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
        }

        private void LoadImage(Image image)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = image;
        }

        private void UpdatePictureBox(byte[] imageBytes)
        {
            if (imageBytes == null) return;

            // Thực hiện cập nhật UI trong luồng chính
            this.Invoke(new Action(() =>
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Image = image;
                }
            }));
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoomFactor += 0.1f; // Tăng tỷ lệ
            }
            else if (e.Delta < 0)
            {
                zoomFactor -= 0.1f; // Giảm tỷ lệ
            }

            if (zoomFactor < 0.1f) // Đảm bảo tỷ lệ không quá nhỏ
            {
                zoomFactor = 0.1f;
            }

            // Cập nhật kích thước PictureBox dựa trên tỷ lệ phóng to/thu nhỏ
            pictureBox.Width = (int)(pictureBox.Image.Width * zoomFactor);
            pictureBox.Height = (int)(pictureBox.Image.Height * zoomFactor);

            // Cập nhật vị trí PictureBox để luôn hiển thị ở giữa Form
            pictureBox.Left = (this.ClientSize.Width - pictureBox.Width) / 2;
            pictureBox.Top = (this.ClientSize.Height - pictureBox.Height) / 2;
        }

        private void btnXoayTrai_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pictureBox.Refresh();
            }
        }

        private void img_XoayTrai_Click(object sender, EventArgs e)
        {
            btnXoayTrai_Click(sender, e);
        }

        private void btnDoiXung_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox.Refresh();
            }
        }

        private void img_DoiXung_Click(object sender, EventArgs e)
        {
            btnDoiXung_Click(sender, e);
        }

        private void btnXoayPhai_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox.Refresh();
            }
        }

        private void imgXoayPhai_Click(object sender, EventArgs e)
        {
            btnXoayPhai_Click(sender, e);
        }
    }
}
