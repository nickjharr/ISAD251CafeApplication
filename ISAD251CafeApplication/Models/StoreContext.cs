using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISAD251CafeApplication.Models
{
    public class StoreContext : DbContext
    {

        public DbSet<OrderLines> OrderLines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLines>()
                .HasKey(o => new { o.OrderId, o.ItemId});

        }

        public DbSet<Orders> OpenOrders { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Items> Menu { get; set; }
        public DbSet<Orders> Orders { get; set; }

        public DbSet<OrderLines> GetOrderLines { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            
        }

    }
}
