using Jay_A4_FunAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jay_A4_FunAPI.Data
{
    public class FunContext : DbContext
    {
        public FunContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<FunModel> FunModel { get; set; }
    }
}
