namespace QuanLyVatTu.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnhVatTu")]
    public partial class AnhVatTu
    {
        public int id { get; set; }

        public string duongdananh { get; set; }

        public int? mavattu { get; set; }

        public int? tt_anh { get; set; }

        public virtual VatTu VatTu { get; set; }
    }
}
