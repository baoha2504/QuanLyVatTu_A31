namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichSuDangNhap")]
    public partial class LichSuDangNhap
    {
        public int id { get; set; }

        public DateTime? thoigian { get; set; }

        public string trangthai { get; set; }

        [StringLength(255)]
        public string tennguoidung { get; set; }

        public int? user_id { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
