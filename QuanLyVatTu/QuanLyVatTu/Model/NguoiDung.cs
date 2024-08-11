namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguoiDung")]
    public partial class NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguoiDung()
        {
            DangNhaps = new HashSet<DangNhap>();
            DanhMucs = new HashSet<DanhMuc>();
            LichSuDangNhaps = new HashSet<LichSuDangNhap>();
            LichSuHoatDongs = new HashSet<LichSuHoatDong>();
            PhuongAnVatTus = new HashSet<PhuongAnVatTu>();
            VatTus = new HashSet<VatTu>();
        }

        [Key]
        public int user_id { get; set; }

        [StringLength(255)]
        public string tennguoidung { get; set; }

        [StringLength(100)]
        public string quanham { get; set; }

        [StringLength(100)]
        public string chucvu { get; set; }

        [StringLength(255)]
        public string nguoisuacuoi { get; set; }

        public DateTime? thoigiansua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangNhap> DangNhaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhMuc> DanhMucs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichSuDangNhap> LichSuDangNhaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichSuHoatDong> LichSuHoatDongs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhuongAnVatTu> PhuongAnVatTus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VatTu> VatTus { get; set; }
    }
}
