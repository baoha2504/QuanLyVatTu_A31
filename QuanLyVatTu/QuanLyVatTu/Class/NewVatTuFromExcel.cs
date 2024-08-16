using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVatTu.Class
{
    public class NewVatTuFromExcel
    {
        public string tenvattu { get; set; }
        public string donvitinh { get; set; }
        public decimal dongia { get; set; }
        public string nguongoc { get; set; }
        public string thongsokythuat { get; set; }
        public string ghichu { get; set; }
        public string tendanhmuc { get; set; }
        public string tenvattudaco { get; set; }
        public double giongnhau { get; set; }
    }
}