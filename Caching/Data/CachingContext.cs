using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Caching.Models;

namespace Caching.Data
{
    public partial class CachingContext : DbContext
    {
        public CachingContext()
        {
        }

        public CachingContext(DbContextOptions<CachingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Toy> Toys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Toy>(entity =>
            {
                entity.ToTable("toys");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
