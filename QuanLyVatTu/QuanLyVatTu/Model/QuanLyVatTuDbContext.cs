using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyVatTu.Model
{
    public partial class QuanLyVatTuDbContext : DbContext
    {
        public QuanLyVatTuDbContext()
            : base("name=QuanLyVatTuDbContext")
        {
        }

        public virtual DbSet<ChiTietPhuongAn> ChiTietPhuongAns { get; set; }
        public virtual DbSet<DangNhap> DangNhaps { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<LichSuDangNhap> LichSuDangNhaps { get; set; }
        public virtual DbSet<LichSuHoatDong> LichSuHoatDongs { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<PhuongAnVatTu> PhuongAnVatTus { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TuKhoaTrung> TuKhoaTrungs { get; set; }
        public virtual DbSet<TuKhoaVatTu> TuKhoaVatTus { get; set; }
        public virtual DbSet<VatTu> VatTus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DangNhap>()
                .Property(e => e.tentaikhoan)
                .IsUnicode(false);

            modelBuilder.Entity<DangNhap>()
                .Property(e => e.matkhau)
                .IsUnicode(false);

            modelBuilder.Entity<DanhMuc>()
                .Property(e => e.madanhmuc)
                .IsUnicode(false);

            modelBuilder.Entity<VatTu>()
                .Property(e => e.madanhmuc)
                .IsUnicode(false);
        }
    }
}
