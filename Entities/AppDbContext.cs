using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;

namespace WebMonAn.Entities
{
    public class AppDbContext:DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Cart>Cart { get; set; }
        public virtual DbSet<Cart_item> Cart_item { get; set; }
        public virtual DbSet<Decentralization> Decentralization { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Order_detail> Order_Detail { get; set; }
        public virtual DbSet<Order_status> Order_Statuse { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Product_review> Product_review { get; set; }
        public virtual DbSet<Product_type> Product_type { get; set; }
        public virtual DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* modelBuilder.Entity<User>()
                .HasOne(e => e.Account)
                .WithOne(e => e.User)
                .HasForeignKey<Account>(e => e.User_id)
                .IsRequired();
            modelBuilder.Entity<Cart_item>()
                .HasOne(e => e.Cart).WithMany(e => e.Cart_items).HasForeignKey(e=>e.Cart_id);
            modelBuilder.Entity<Cart_item>()
                .HasOne(e => e.Product).WithMany(e => e.Cart_items).HasForeignKey(e => e.Product_id);
            modelBuilder.Entity<Order_detail>()
                .HasOne(e => e.Product).WithMany(e => e.Order_details).HasForeignKey(e => e.Product_id);
            *//*            modelBuilder.Entity<Product_type>()*//*
                            .HasMany(e => e.Products).WithOne(e => e.Product_type).HasPrincipalKey(e=>e.Products);*/
           /* modelBuilder.Entity<Product>()
               .HasOne(e => e.Product_type).WithMany(et => et.Products).HasForeignKey(e => e.Product_type_id);
               */
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-TDU7EDU\SQLEXPRESS; Database = MonAn; 
                                       MultipleActiveResultSets=True; Trusted_Connection=True;TrustServerCertificate=true");
        }
    }
}
