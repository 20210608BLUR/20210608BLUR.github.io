using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HolyShong.BackStage.Entity
{
    public partial class HolyShongContext : DbContext
    {
        public HolyShongContext()
        {
        }

        public HolyShongContext(DbContextOptions<HolyShongContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Businesshour> Businesshours { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Deliver> Delivers { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<DiscountMember> DiscountMembers { get; set; }
        public virtual DbSet<DiscountStore> DiscountStores { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemDetail> ItemDetails { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderDetailOption> OrderDetailOptions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }
        public virtual DbSet<ProductOptionDetail> ProductOptionDetails { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreCategory> StoreCategories { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressDetail).IsRequired();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Member");
            });

            modelBuilder.Entity<Businesshour>(entity =>
            {
                entity.HasKey(e => e.BusinesshoursId);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Businesshours)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Businesshours_Store");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");
            });

            modelBuilder.Entity<Deliver>(entity =>
            {
                entity.ToTable("Deliver");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.HeadshotImg).IsRequired();

                entity.Property(e => e.IdentityAndLicenseImg).IsRequired();

                entity.Property(e => e.InsuranceImg).IsRequired();

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.IsDelivering).HasColumnName("isDelivering");

                entity.Property(e => e.IsOnline).HasColumnName("isOnline");

                entity.Property(e => e.PoliceCriminalRecordImg).IsRequired();

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deliver_Member");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.DiscountCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.IsAllStore).HasColumnName("isAllStore");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DiscountMember>(entity =>
            {
                entity.ToTable("DiscountMember");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.DiscountMembers)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountMember_Discount");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.DiscountMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountMember_Member");
            });

            modelBuilder.Entity<DiscountStore>(entity =>
            {
                entity.ToTable("DiscountStore");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.DiscountStores)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountStroe_Discount");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.DiscountStores)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountStroe_Store");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorite");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Favorite_Member");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Favorite_Store");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Cart");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Product");
            });

            modelBuilder.Entity<ItemDetail>(entity =>
            {
                entity.ToTable("ItemDetail");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemDetail_Item");

                entity.HasOne(d => d.ProductOptionDetail)
                    .WithMany(p => p.ItemDetails)
                    .HasForeignKey(d => d.ProductOptionDetailId)
                    .HasConstraintName("FK_ItemDetail_ProductOptionDetail");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.AuditStatus).HasComment("-99=非外送員、-1=拒絕、0=待審核、1=待補件、2=審核完成");

                entity.Property(e => e.Cellphone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTime).HasColumnType("datetime");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.StoreAuditStatus).HasComment("-99=非店家、-1=拒絕、0=待審核、1=審核完成");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryAddress).IsRequired();

                entity.Property(e => e.DeliveryFee).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DiscountMoney).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Notes).IsRequired();

                entity.Property(e => e.OrderStatus).HasComment("1已付2準備中3餐點完成4等待配送5配送中6已配送");

                entity.Property(e => e.OrderStatusUpdateTime).HasColumnType("datetime");

                entity.Property(e => e.OrginalMoney).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.Tips).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Deliver)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliverId)
                    .HasConstraintName("FK_Order_Deliver");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Member");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<OrderDetailOption>(entity =>
            {
                entity.ToTable("OrderDetailOption");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.OrderDetailOptions)
                    .HasForeignKey(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetailOption_OrderDetail");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 1)");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductCategory");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCategory_Store");
            });

            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.ToTable("ProductOption");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOptions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOption_Product");
            });

            modelBuilder.Entity<ProductOptionDetail>(entity =>
            {
                entity.ToTable("ProductOptionDetail");

                entity.Property(e => e.AddPrice).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ProductOption)
                    .WithMany(p => p.ProductOptionDetails)
                    .HasForeignKey(d => d.ProductOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOptionDetail_ProductOption");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.ToTable("Rank");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Ranks)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rank_Member");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Cellphone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Img).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<StoreCategory>(entity =>
            {
                entity.ToTable("StoreCategory");

                entity.Property(e => e.Img).IsRequired();

                entity.Property(e => e.KeyWord).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
