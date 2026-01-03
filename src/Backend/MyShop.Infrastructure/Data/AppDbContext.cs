using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MyShop.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserConfig> UserConfigs { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasColumnType("character varying")
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("Customer_pkey");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "Customer_phone_key").IsUnique();

            entity.Property(e => e.CustomerId)
                .ValueGeneratedOnAdd()
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
            entity.Property(e => e.CreateDate).HasColumnName("create_date");
            entity.Property(e => e.FullName)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Point).HasColumnName("point");
            entity.Property(e => e.TierId).HasColumnName("tier_id");
            entity.Property(e => e.IsRemoved)
                .HasColumnType("boolean")
                .HasColumnName("is_removed");

            entity.HasOne(d => d.Tier).WithMany(p => p.Customers)
                .HasForeignKey(d => d.TierId)
                .HasConstraintName("Customer_tier_id_fkey");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.TierId).HasName("Membership_pkey");

            entity.ToTable("Membership");

            entity.Property(e => e.TierId)
                .ValueGeneratedOnAdd()
                .HasColumnName("tier_id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Threshold).HasColumnName("threshold");
            entity.Property(e => e.TierName)
                .HasColumnType("character varying")
                .HasColumnName("tier_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DiscountAmount).HasColumnName("discount_amount");
            entity.Property(e => e.FinalPrice).HasColumnName("final_price");
            entity.Property(e => e.OrderCode)
                .HasColumnType("character varying")
                .HasColumnName("order_code");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoucherId).HasColumnName("voucher_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Order_customer_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Order_user_id_fkey");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("Order_voucher_id_fkey");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("OrderDetail_pkey");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CurrentCost).HasColumnName("current_cost");
            entity.Property(e => e.CurrentPrice).HasColumnName("current_price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalLine).HasColumnName("total_line");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderDetail_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderDetail_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.IsRemoved)
                .HasColumnType("boolean")
                .HasColumnName("is_removed");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.Unit)
                .HasColumnType("character varying")
                .HasColumnName("unit");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("Product_category_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("user_id");
            entity.Property(e => e.FullName)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasColumnType("bit(1)")
                .HasColumnName("status");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("User_role_id_fkey");
        });

        modelBuilder.Entity<UserConfig>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("User_Config_pkey");

            entity.ToTable("User_Config");

            entity.Property(e => e.SettingId)
                .ValueGeneratedNever()
                .HasColumnName("setting_id");
            entity.Property(e => e.LastModule)
                .HasColumnType("character varying")
                .HasColumnName("last_module");
            entity.Property(e => e.PerPage).HasColumnName("per_page");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserConfigs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("User_Config_user_id_fkey");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("Voucher_pkey");

            entity.ToTable("Voucher");

            entity.Property(e => e.VoucherId)
                .ValueGeneratedNever()
                .HasColumnName("voucher_id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsRemoved).HasColumnName("is_removed");
            entity.Property(e => e.MinThreshold).HasColumnName("min_threshold");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.VoucherCode)
                .HasColumnType("character varying")
                .HasColumnName("voucher_code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}