using Microsoft.EntityFrameworkCore;
using VAirZoneWebAPI.Models.Organizer;
using VAirZoneWebAPI.Models.User;

namespace VAirZoneWebAPI.Databases
{
	public class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
	{
		public DbSet<UserModel> Users { get; set; }
		public DbSet<UserCodeModel> UserCodes { get; set; }

		public DbSet<OrganizerModel> Organizers { get; set; }
		public DbSet<ActivityModel> Activities { get; set; }
		public DbSet<ErrorCatcherModel> ErrorCatchers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<ActivityModel>()
			//	.HasOne(col => col.Organizer)
			//	.WithMany(col => col.Activities)
			//	.HasForeignKey(col => col.OrganizerId)
			//	.OnDelete(DeleteBehavior.NoAction); // If there are any activities cannot delete organizer

			//modelBuilder.Entity<ErrorCatcherModel>()
			//	.HasOne(col => col.Organizer)
			//	.WithMany(col => col.ErrorCatchers)
			//	.HasForeignKey(col => col.OrganizerId)
			//	.OnDelete(DeleteBehavior.NoAction); // If there are any activities cannot delete organizer

			modelBuilder.Entity<UserModel>()
				.HasIndex(col => col.Email)
				.IsUnique();

			modelBuilder.Entity<OrganizerModel>()
				.HasIndex(col => col.OrganizerName)
				.IsUnique();
		}
	}
}