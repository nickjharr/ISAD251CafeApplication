using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISAD251CafeApplication.Models
{
    public class StoreContext : DbContext
    {
        public DbSet<Items> Items { get; set; }
        public DbSet<Menu> Menu { get; set; }

        

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

    }
}
