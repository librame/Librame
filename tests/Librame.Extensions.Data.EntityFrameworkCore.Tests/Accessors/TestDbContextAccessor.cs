﻿using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Tests
{
    using Models;

    public class TestDbContextAccessor : DbContextAccessor
    {
        public TestDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable();

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).HasMaxLength(256).IsRequired();

                // 关联
                b.HasMany(x => x.Articles).WithOne(x => x.Category)
                    .IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(b =>
            {
                b.ToTable(descr => descr.ChangeSuffix("19")); // descr.ChangeDateSuffix(now => now.ToString("yy")) // descr.ChangeSuffix("20")

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).HasMaxLength(256);
                b.Property(x => x.Title).HasMaxLength(256).IsRequired();
            });
        }

    }
}
