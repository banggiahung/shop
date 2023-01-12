using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Models.EntityFramework
{
    public partial class ShopQuanAoDBContext : DbContext
    {
        public ShopQuanAoDBContext()
            : base("name=ShopQuanAoDBContext")
        {
        }

        public virtual DbSet<ACOUNT> ACOUNTs { get; set; }
        public virtual DbSet<BAI_VIET> BAI_VIET { get; set; }
        public virtual DbSet<CHI_TIET_HOA_DON> CHI_TIET_HOA_DON { get; set; }
        public virtual DbSet<COLOR> COLORs { get; set; }
        public virtual DbSet<HOA_DON> HOA_DON { get; set; }
        public virtual DbSet<LOAI_SAN_PHAM> LOAI_SAN_PHAM { get; set; }
        public virtual DbSet<SAN_PHAM> SAN_PHAM { get; set; }
        public virtual DbSet<SAN_PHAM_CHI_TIET> SAN_PHAM_CHI_TIET { get; set; }
        public virtual DbSet<SIZE> SIZEs { get; set; }
        public virtual DbSet<SLIDE> SLIDEs { get; set; }
        public virtual DbSet<STATUS_HOA_DON> STATUS_HOA_DON { get; set; }
        public virtual DbSet<NHAN_VIEN> NHAN_VIEN { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACOUNT>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<ACOUNT>()
                .Property(e => e.MAT_KHAU)
                .IsUnicode(false);

            modelBuilder.Entity<ACOUNT>()
                .Property(e => e.LINK_ANH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ACOUNT>()
                .Property(e => e.PHONE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BAI_VIET>()
                .Property(e => e.SLUG)
                .IsUnicode(false);

            modelBuilder.Entity<BAI_VIET>()
                .Property(e => e.IMAGES)
                .IsUnicode(false);

            modelBuilder.Entity<CHI_TIET_HOA_DON>()
                .Property(e => e.GIA_BAN)
                .HasPrecision(15, 4);

            modelBuilder.Entity<COLOR>()
                .Property(e => e.SLUG)
                .IsUnicode(false);

            modelBuilder.Entity<COLOR>()
                .Property(e => e.MA_MAU)
                .IsUnicode(false);

            modelBuilder.Entity<COLOR>()
                .Property(e => e.MA_SP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<COLOR>()
                .Property(e => e.IMAGES)
                .IsUnicode(false);

            modelBuilder.Entity<HOA_DON>()
                .Property(e => e.SDT_NHAN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOA_DON>()
                .Property(e => e.TONG_TIEN)
                .HasPrecision(15, 4);

            modelBuilder.Entity<LOAI_SAN_PHAM>()
                .Property(e => e.SLUG)
                .IsUnicode(false);

           
            modelBuilder.Entity<SAN_PHAM>()
                .Property(e => e.MA_SP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SAN_PHAM>()
                .Property(e => e.LINK_ANH_CHINH)
                .IsUnicode(false);

            modelBuilder.Entity<SAN_PHAM>()
                .Property(e => e.LIST_ANH_KEM)
                .IsUnicode(false);

            modelBuilder.Entity<SAN_PHAM>()
                .Property(e => e.GIA_NHAP)
                .HasPrecision(15, 4);

            modelBuilder.Entity<SAN_PHAM>()
                .Property(e => e.GIA_BAN)
                .HasPrecision(15, 4);

            modelBuilder.Entity<SAN_PHAM_CHI_TIET>()
                .Property(e => e.MA_SP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SIZE>()
                .Property(e => e.TEN_SIZE)
                .IsUnicode(false);

            modelBuilder.Entity<SIZE>()
                .Property(e => e.SLUG)
                .IsUnicode(false);

            modelBuilder.Entity<SLIDE>()
                .Property(e => e.LINK)
                .IsUnicode(false);

            modelBuilder.Entity<SLIDE>()
                .Property(e => e.IMAGES)
                .IsUnicode(false);
            modelBuilder.Entity<NHAN_VIEN>()
                .Property(e => e.TEN_NHAN_VIEN)
                .IsUnicode(false);
        }
    }
}
