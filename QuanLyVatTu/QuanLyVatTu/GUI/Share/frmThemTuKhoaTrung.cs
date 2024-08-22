using QuanLyVatTu.Model;
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
    public partial class frmThemTuKhoaTrung : Form
    {
        public TuKhoaTrung tkt = new TuKhoaTrung();
        int tukhoa_id;
        public frmThemTuKhoaTrung(int tukhoa_id)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.tukhoa_id = tukhoa_id;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTuKhoaTrung.Text))
            {
                MessageBox.Show("Tên từ khóa đang trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dbContext = new QuanLyVatTuDbContext())
            {
                string tukhoa = txtTenTuKhoaTrung.Text.Trim().ToLower();

                // Kiểm tra từ khóa chính
                bool isTukhoachinhExists = dbContext.TuKhoaVatTus.Any(tk => tk.tukhoachinh.ToLower() == tukhoa);
                if (isTukhoachinhExists)
                {
                    MessageBox.Show("Tên từ khóa vật tư đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra từ khóa trùng
                bool isTukhoatrungExists = dbContext.TuKhoaTrungs.Any(tk => tk.tukhoatrung.ToLower() == tukhoa);
                if (isTukhoatrungExists)
                {
                    MessageBox.Show("Tên từ khóa vật tư đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                tkt.tukhoatrung = txtTenTuKhoaTrung.Text;
                tkt.nguoisuacuoi = frmDangNhap.tennguoidung;
                tkt.thoigiansua = DateTime.Now;
                tkt.tukhoa_id = tukhoa_id;
                this.Close();
            }
        }
    }
}
