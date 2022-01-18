using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace HolyShong.Models.HolyShongModel
{
    public partial class HolyShongContext : DbContext
    {
        public HolyShongContext()
            : base("name=HolyShongConnection")
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Businesshours> Businesshours { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Deliver> Deliver { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<DiscountMember> DiscountMember { get; set; }
        public virtual DbSet<DiscountStore> DiscountStore { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemDetail> ItemDetail { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderDetailOption> OrderDetailOption { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductOption> ProductOption { get; set; }
        public virtual DbSet<ProductOptionDetail> ProductOptionDetail { get; set; }
        public virtual DbSet<Rank> Rank { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreCategory> StoreCategory { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasMany(e => e.Item)
                .WithRequired(e => e.Cart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.Amount)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Discount>()
                .HasMany(e => e.DiscountMember)
                .WithRequired(e => e.Discount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Discount>()
                .HasMany(e => e.DiscountStore)
                .WithRequired(e => e.Discount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.ItemDetail)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Cellphone)
                .IsFixedLength();

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Address)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Deliver)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.DiscountMember)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Favorite)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Rank)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.DeliveryFee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.Tips)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.DiscountMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.OrginalMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetail)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderDetail>()
                .HasMany(e => e.OrderDetailOption)
                .WithRequired(e => e.OrderDetail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Item)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductOption)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.ProductCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductOption>()
                .HasMany(e => e.ProductOptionDetail)
                .WithRequired(e => e.ProductOption)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductOptionDetail>()
                .Property(e => e.AddPrice)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Businesshours)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.DiscountStore)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Favorite)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.ProductCategory)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);
        }
    }
}
