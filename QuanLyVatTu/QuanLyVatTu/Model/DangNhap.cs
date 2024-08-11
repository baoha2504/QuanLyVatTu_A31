namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangNhap")]
    public partial class DangNhap
    {
        [Key]
        [StringLength(100)]
        public string tentaikhoan { get; set; }

        public string matkhau { get; set; }

        public int? phanquyen { get; set; }

        public int? trangthai { get; set; }

        [StringLength(255)]
        public string nguoisuacuoi { get; set; }

        public int? user_id { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
