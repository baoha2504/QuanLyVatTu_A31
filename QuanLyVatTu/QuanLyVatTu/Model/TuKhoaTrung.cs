namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TuKhoaTrung")]
    public partial class TuKhoaTrung
    {
        public int id { get; set; }

        [Column("tukhoatrung")]
        [StringLength(255)]
        public string tukhoatrung { get; set; }

        [StringLength(255)]
        public string nguoisuacuoi { get; set; }

        public DateTime? thoigiansua { get; set; }

        public int? tukhoa_id { get; set; }

        public virtual TuKhoaVatTu TuKhoaVatTu { get; set; }
    }
}
