using Microsoft.EntityFrameworkCore;

namespace ngo_webapp.Areas.Admin.Models.Entities;

public partial class NGODbContext : DbContext
{
	public NGODbContext()
	{
	}

	public NGODbContext(DbContextOptions<NGODbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Appeal> Appeals { get; set; }

	public virtual DbSet<Blog> Blogs { get; set; }

	public virtual DbSet<Donation> Donations { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source=DEVBLOCK;Initial Catalog=Ngo-management;Persist Security Info=True;User ID=sa;Password=05012004;Trust Server Certificate=True");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Appeal>(entity =>
		{
			entity.HasKey(e => e.AppealsId).HasName("PK__Appeals__5E813B30A8132DFC");

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
			entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E50C6BFD400");

			entity.Property(e => e.BlogId).HasColumnName("BlogID");
			entity.Property(e => e.AppealId).HasColumnName("AppealID");
			entity.Property(e => e.Content).IsUnicode(false);
			entity.Property(e => e.CreationDate).HasColumnType("datetime");
			entity.Property(e => e.Title)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.UserId).HasColumnName("UserID");

			entity.HasOne(d => d.Appeal).WithMany(p => p.Blogs)
				.HasForeignKey(d => d.AppealId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Blogs_Appeals");

			entity.HasOne(d => d.User).WithMany(p => p.Blogs)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Blogs_Users");
		});

		modelBuilder.Entity<Donation>(entity =>
		{
			entity.HasKey(e => e.DonationId).HasName("PK__Donation__C5082EDB745B0277");

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
			entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC6A9A6DB9");

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
