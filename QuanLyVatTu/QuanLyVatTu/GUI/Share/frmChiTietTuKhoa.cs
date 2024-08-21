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
    public partial class frmChiTietTuKhoa : Form
    {
        public TuKhoaVatTu tkvt;

        public frmChiTietTuKhoa(TuKhoaVatTu tkvt)
        {
            InitializeComponent();
            this.tkvt = tkvt;
        }
    }
}
