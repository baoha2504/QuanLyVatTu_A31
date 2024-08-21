namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TuKhoaVatTu")]
    public partial class TuKhoaVatTu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TuKhoaVatTu()
        {
            TuKhoaTrungs = new HashSet<TuKhoaTrung>();
        }

        [Key]
        public int tukhoa_id { get; set; }

        [StringLength(255)]
        public string tukhoachinh { get; set; }

        [StringLength(255)]
        public string nguoisuacuoi { get; set; }

        public DateTime? thoigiansua { get; set; }

        public int? user_id { get; set; }

        [StringLength(100)]
        public string madanhmuc { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TuKhoaTrung> TuKhoaTrungs { get; set; }
    }
}
