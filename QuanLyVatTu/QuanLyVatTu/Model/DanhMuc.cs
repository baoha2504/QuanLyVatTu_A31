namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMuc")]
    public partial class DanhMuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMuc()
        {
            VatTus = new HashSet<VatTu>();
            TuKhoaVatTus = new HashSet<TuKhoaVatTu>();
        }

        [Key]
        [StringLength(100)]
        public string madanhmuc { get; set; }

        [StringLength(255)]
        public string tendanhmuc { get; set; }

        [StringLength(255)]
        public string nguoisuacuoi { get; set; }

        public DateTime? thoigiansua { get; set; }

        public int? user_id { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VatTu> VatTus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TuKhoaVatTu> TuKhoaVatTus { get; set; }
    }
}
