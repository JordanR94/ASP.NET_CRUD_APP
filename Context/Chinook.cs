using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JR.Shared
{
    public class Chinook : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string CurrentDir = System.Environment.CurrentDirectory;
            string ParentDir = System.IO.Directory.GetParent(CurrentDir).FullName;
            string path = System.IO.Path.Combine(ParentDir, "Chinook.db; Foreign Keys = False");

            optionsBuilder.UseSqlite($"Filename={path}").EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
              .HasOne(alb => alb.Artist)
              .WithMany(art => art.Album)
              .HasForeignKey(a => a.ArtistID)
              .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<Artist>()
            //   .HasMany(art => art.Album)
            //   .WithOne(alb => alb.Artist)
            //   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
