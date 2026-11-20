using System;
using System.Collections.Generic;
using DataBaseFirstWpfApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseFirstWpfApp.Data;

public partial class WarehouseContext : DbContext
{
    public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Packaging).HasColumnName("packaging");
            entity.Property(e => e.ShelfLife).HasColumnName("shelf_life");
            entity.Property(e => e.StorageConditions).HasColumnName("storage_conditions");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.ContractDate)
                .HasColumnType("DATETIME")
                .HasColumnName("contract_date");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.ToTable("warehouse");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeliveryMethod).HasColumnName("delivery_method");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ReceiptDate)
                .HasColumnType("DATETIME")
                .HasColumnName("receipt_date");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.Product).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
