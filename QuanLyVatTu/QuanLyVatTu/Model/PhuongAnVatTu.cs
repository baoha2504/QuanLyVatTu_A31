namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhuongAnVatTu")]
    public partial class PhuongAnVatTu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhuongAnVatTu()
        {
            ChiTietPhuongAns = new HashSet<ChiTietPhuongAn>();
        }

        [Key]
        public int maphuongan { get; set; }

        [StringLength(255)]
        public string tenphuongan { get; set; }

        public decimal? tongtien { get; set; }

        [StringLength(255)]
        public string nguoilap { get; set; }

        public DateTime? thoigianlap { get; set; }

        [StringLength(255)]
        public string nguoiduyet { get; set; }

        public DateTime? thoigianduyet { get; set; }

        public string noidungduyet { get; set; }

        public int? user_id { get; set; }

        public int? hoanthanh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhuongAn> ChiTietPhuongAns { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
