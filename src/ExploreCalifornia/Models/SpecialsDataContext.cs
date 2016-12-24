using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExploreCalifornia.Models
{
    public class SpecialsDataContext : DbContext
    {
        public DbSet<Special> Specials { get; set; }

        public SpecialsDataContext(DbContextOptions<SpecialsDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Special>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Special>()
                .HasIndex(s => s.Key)
                .IsUnique();
        }

        public IEnumerable<Special> GetMonthlySpecials()
        {
            return Specials.OrderByDescending(x => x.Created).Where(x => x.Created.Month == DateTime.Now.Month).ToArray();
        }

        public IEnumerable<Special> GetSpecials()
        {
            return Specials.OrderByDescending(x => x.Created).ToArray();
        }
    }
}
