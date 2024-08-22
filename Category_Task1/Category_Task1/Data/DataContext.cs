using Category_Task1.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Category_Task1.Data
{
    // Install package Microsoft.EntityFrameworkCore
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee entity
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            // Account entity
            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Accounts)
                .HasForeignKey(a => a.EmployeeId)
                .IsRequired();

            // Category entity
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            // Product entity
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderProduct entity
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(op => op.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail entity
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.OrderDetailPrice)
                .HasColumnType("decimal(10, 2)");

            //modelBuilder.Entity<OrderDetail>()
            //    .HasOne(od => od.OrderProduct)
            //    .WithMany(op => op.OrderDetails)
            //    .HasForeignKey(od => od.OrderId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal properties
            modelBuilder.Entity<OrderProduct>()
                .Property(op => op.OrderTotalPrice)
                .HasColumnType("decimal(10, 2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductPrice)
                .HasColumnType("decimal(10, 2)");

            base.OnModelCreating(modelBuilder);
        }

    }
}