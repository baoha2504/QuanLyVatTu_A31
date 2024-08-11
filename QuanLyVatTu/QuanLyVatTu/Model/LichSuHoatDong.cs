namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichSuHoatDong")]
    public partial class LichSuHoatDong
    {
        public int id { get; set; }

        public DateTime? thoigian { get; set; }

        public string hoatdong { get; set; }

        [StringLength(255)]
        public string tennguoidung { get; set; }

        public int? user_id { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
