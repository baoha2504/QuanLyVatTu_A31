using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVatTu.Class
{
    internal class VatTuTrung
    {
        public int mavattu { get; set; }
        public string tenvattu { get; set; }
        public string nguongoc { get; set; }
        public string madanhmuc { get; set; }
        public double giongnhau { get; set; }
        public int tinhtrangxacnhan;
    }
}
