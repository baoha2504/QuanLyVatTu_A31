using QuanLyVatTu.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVatTu.GUI.Share
{
    public partial class frmLoad : Form
    {
        private Timer timer;
        public frmLoad()
        {
            InitializeComponent();
            // Thiết lập Timer để kiểm tra trạng thái của form1
            timer = new Timer();
            timer.Interval = 500; // Kiểm tra mỗi 500ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái cập nhật của Form1
            if (BienDungChung.isUpdateCompleted)
            {
                timer.Stop(); // Dừng Timer khi cập nhật xong
                this.Close(); // Đóng formLoad
            }
        }

        private void FormLoad_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Dispose(); // Giải phóng Timer khi formLoad đóng
        }
    }
}
