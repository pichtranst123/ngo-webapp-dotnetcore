﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ngo_webapp.Models.Entities;

public partial class NgoManagementContext : DbContext
{
    public NgoManagementContext()
    {
    }

    public NgoManagementContext(DbContextOptions<NgoManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appeal> Appeals { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=NGO_Management;Persist Security Info=True;User ID=sa;Password=Hieu1309;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appeal>(entity =>
        {
            entity.HasKey(e => e.AppealsId).HasName("PK__Appeals__5E813B30B5B226A6");

            entity.HasIndex(e => e.AppealsName, "UC_Appeals_AppealsName").IsUnique();

            entity.Property(e => e.AppealsId).HasColumnName("AppealsID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AppealsImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Appeals_Image");
            entity.Property(e => e.AppealsName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Organization)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E505569532E");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.BlogImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Blog_Image");
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Blogs_Users");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Donation__C5082EDB64D8D63A");

            entity.Property(e => e.DonationId).HasColumnName("DonationID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AppealsId).HasColumnName("AppealsID");
            entity.Property(e => e.DonationDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Appeals).WithMany(p => p.Donations)
                .HasForeignKey(d => d.AppealsId)
                .HasConstraintName("FK_Donations_Appeals");

            entity.HasOne(d => d.User).WithMany(p => p.Donations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Donations_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC8AB4EA64");

            entity.HasIndex(e => e.Email, "UC_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UC_Users_Username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GoogleHash)
                .HasMaxLength(255)
                .HasColumnName("Google_Hash");
            entity.Property(e => e.IsAdmin).HasColumnName("Is_Admin");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.UserImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("User_Image");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
