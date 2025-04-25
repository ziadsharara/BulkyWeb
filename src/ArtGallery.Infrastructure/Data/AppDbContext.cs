using ArtGallery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
						: base(options) { }

		public DbSet<Artwork> Artworks { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Bid> Bids { get; set; }
		public DbSet<ArtworkLike> ArtworkLikes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
											.ToTable("Users")
											.HasIndex(u => u.Email)
											.IsUnique();

			modelBuilder.Entity<Artwork>().ToTable("Artworks");
			modelBuilder.Entity<Bid>().ToTable("Bids");

			modelBuilder.Entity<Artwork>()
											.HasOne(a => a.Artist)
											.WithMany(u => u.Artworks)
											.HasForeignKey(a => a.ArtistId)
											.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Bid>()
											.HasOne(b => b.Artwork)
											.WithMany(a => a.Bids)
											.HasForeignKey(b => b.ArtworkId);

			modelBuilder.Entity<Bid>()
											.HasOne(b => b.Buyer)
											.WithMany(u => u.Bids)
											.HasForeignKey(b => b.BuyerId)
											.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<ArtworkLike>()
											.HasKey(al => new { al.ArtworkId, al.UserId });

			modelBuilder.Entity<ArtworkLike>()
											.HasOne(al => al.Artwork)
											.WithMany(a => a.Likes)
											.HasForeignKey(al => al.ArtworkId);

			modelBuilder.Entity<ArtworkLike>()
											.HasOne(al => al.User)
											.WithMany(u => u.LikedArtworks)
											.HasForeignKey(al => al.UserId);
		}
	}

}
