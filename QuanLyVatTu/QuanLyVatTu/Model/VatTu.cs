namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VatTu")]
    public partial class VatTu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VatTu()
        {
            ChiTietPhuongAns = new HashSet<ChiTietPhuongAn>();
        }

        [Key]
        public int mavattu { get; set; }

        [StringLength(255)]
        public string tenvattu { get; set; }

        [StringLength(50)]
        public string donvitinh { get; set; }

        public decimal? dongia { get; set; }

        [StringLength(255)]
        public string nguongoc { get; set; }

        public string thongsokythuat { get; set; }

        public int? trangthai { get; set; }

        public string ghichu { get; set; }

        [StringLength(255)]
        public string nguoisuacuoi { get; set; }

        public DateTime? thoigiansua { get; set; }

        public int? user_id { get; set; }

        [StringLength(100)]
        public string madanhmuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhuongAn> ChiTietPhuongAns { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
