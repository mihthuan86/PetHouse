using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetHouse.Models;
using System;

namespace PetHouse.Data
{
	public class PetHouseDbContext : IdentityDbContext<User>
	{
		public PetHouseDbContext(DbContextOptions<PetHouseDbContext> options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderDetail>().HasKey(am => new
			{
				am.OrderId,
				am.ProductId
			});
			modelBuilder.Entity<OrderDetail>().HasOne(m => m.Order).WithMany(am => am.OrderDetails).HasForeignKey(m => m.OrderId);
			modelBuilder.Entity<OrderDetail>().HasOne(m => m.Product).WithMany(am => am.OrderDetails).HasForeignKey(m => m.ProductId);


			modelBuilder.Entity<ImportDetail>().HasKey(am => new
			{
				am.ImportId,
				am.ProductId
			});
			modelBuilder.Entity<ImportDetail>().HasOne(m => m.Import).WithMany(am => am.ImportDetails).HasForeignKey(m => m.ImportId);
			modelBuilder.Entity<ImportDetail>().HasOne(m => m.Product).WithMany(am => am.ImportDetails).HasForeignKey(m => m.ProductId);
			
			base.OnModelCreating(modelBuilder);
		}
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Menu> Menus { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Payment> Payments { get;set; } 
		public DbSet<Post> Posts { get; set; }
		public DbSet<Product> Products { get; set; }		
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<Import> Imports { get; set; }
		public DbSet<ImportDetail> ImportDetails { get; set; }

	}
}
