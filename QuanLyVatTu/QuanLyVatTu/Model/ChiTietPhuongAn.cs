namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhuongAn")]
    public partial class ChiTietPhuongAn
    {
        public int id { get; set; }

        [StringLength(255)]
        public string tenvattu { get; set; }

        [StringLength(50)]
        public string donvitinh { get; set; }

        public decimal? soluong { get; set; }

        public decimal? doicu { get; set; }

        public decimal? capmoi { get; set; }

        public decimal? dongia { get; set; }

        public decimal? thanhtien { get; set; }

        public string ghichu { get; set; }

        public string binhluan { get; set; }

        public int? maphuongan { get; set; }

        public int? mavattu { get; set; }

        public virtual PhuongAnVatTu PhuongAnVatTu { get; set; }

        public virtual VatTu VatTu { get; set; }
    }
}
