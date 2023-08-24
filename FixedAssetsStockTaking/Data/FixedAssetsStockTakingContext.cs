using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FixedAssetsStockTaking.Models;
using Microsoft.CodeAnalysis;

namespace FixedAssetsStockTaking.Data
{
    public class FixedAssetsStockTakingContext : DbContext
    {
        public FixedAssetsStockTakingContext (DbContextOptions<FixedAssetsStockTakingContext> options)
            : base(options)
        {
        }

        public DbSet<FixedAssetsStockTaking.Models.SuperAdmin> SuperAdmin { get; set; } = default!;

        public DbSet<FixedAssetsStockTaking.Models.Inventar> Inventar { get; set; } = default!;

        public DbSet<FixedAssetsStockTaking.Models.User> Users { get; set; } = default!;

        public DbSet<FixedAssetsStockTaking.Models.Role> Roles { get; set; } = default!;

        public DbSet<FixedAssetsStockTaking.Models.MijlocFix> MijlocFix { get; set; } = default!;

        public DbSet<FixedAssetsStockTaking.Models.CostCenter> CostCenters { get; set; } = default!;
        public DbSet<FixedAssetsStockTaking.Models.ExtraLine> ExtraLines { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => new { u.Username, u.InventarID });

            modelBuilder.Entity<ExtraLine>()
                .HasKey(e => new { e.InventarID, e.MFLine });

            // Restul configurărilor

            base.OnModelCreating(modelBuilder);


            //modelBuilder.Entity<MijlocFix>()
            // .HasOne(m => m.InventarID)
            // .HasForeignKey(m => m.MijlocFixs)
            // .HasForeignKey(u => u.subzoneId);
            modelBuilder.Entity<MijlocFix>()
             .HasOne(m => m.Inventar)      
             .WithMany(i => i.MijlocFixes)    
             .HasForeignKey(m => m.InventarID)  
             .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ExtraLine>()
            //    .HasOne(e => e.MijlocFix)
            //    .WithMany()
            //    .HasForeignKey(e => e.MFLine)
            //    .OnDelete(DeleteBehavior.NoAction);

            
        }
    }
}
