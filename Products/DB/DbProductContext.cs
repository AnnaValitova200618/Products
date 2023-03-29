using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Products.Model;

namespace Products.DB;

public partial class DbProductContext : DbContext
{
    public DbProductContext()
    {
    }

    public DbProductContext(DbContextOptions<DbProductContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=test.sqlite");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateOfsale)
                .HasColumnType("date")
                .HasColumnName("DateOFSale");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.PriceEnd).IsUnicode(false);
            entity.Property(e => e.PriceStart).IsUnicode(false); 

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_Product_Status");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
