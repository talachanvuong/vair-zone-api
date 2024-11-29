using Microsoft.EntityFrameworkCore;
using VAirZoneWebAPI.Databases;
using VAirZoneWebAPI.DTOs.Organizer;
using VAirZoneWebAPI.Interfaces;
using VAirZoneWebAPI.Models.Organizer;

namespace VAirZoneWebAPI.Repositories
{
	public class OrganizerRepository(ApplicationDbContext applicationDbContext) : IOrganizerRepository
	{
		public async Task CreateAsync<T>(T record) where T : class
		{
			await applicationDbContext.Set<T>().AddAsync(record);
			await applicationDbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync<T>(T record) where T : class
		{
			applicationDbContext.Set<T>().Remove(record);
			await applicationDbContext.SaveChangesAsync();
		}

		public async Task<List<OrganizerModel>> GetOrganizers()
		{
			return await applicationDbContext.Organizers.ToListAsync();
		}

		public async Task<List<ActivityModel>> GetActivitiesByIdOnPage(int organzierId, int page)
		{
			int size = 6;
			int skip = (page - 1) * size;
			return await applicationDbContext.Activities
				.Where(record => record.OrganizerId == organzierId)
				.OrderByDescending(record => record.CreatedOn)
				.Skip(skip)
				.Take(size)
				.ToListAsync();
		}

		public async Task<List<ActivityModel>> GetActivitiesById(int organzierId)
		{
			return await applicationDbContext.Activities
				.Where(record => record.OrganizerId == organzierId)
				.OrderByDescending(record => record.CreatedOn)
				.ToListAsync();
		}

		public async Task<PageCountDto> GetPageCountById(int organzierId)
		{
			int size = 6;
			var activities = await applicationDbContext.Activities
				.Where(record => record.OrganizerId == organzierId)
				.ToListAsync();

			return new PageCountDto
			{
				Count = (int)Math.Ceiling((double)activities.Count / size)
			};
		}

		public async Task<List<ActivityModel>> GetLatestActivities(int size)
		{
			return await applicationDbContext.Activities
				.OrderByDescending(record => record.CreatedOn)
				.Take(size)
				.ToListAsync();
		}

		public async Task<List<ErrorCatcherDto>> GetErrorCatchers()
		{
			return await applicationDbContext.ErrorCatchers
				.Join(applicationDbContext.Organizers,
					errorCatcher => errorCatcher.OrganizerId,
					organizer => organizer.OrganizerId,
					(errorCatcher, organizer) => new ErrorCatcherDto
					{
						OrganizerName = organizer.OrganizerName,
						Problem = errorCatcher.Problem,
						CreatedOn = errorCatcher.CreatedOn
					})
				.ToListAsync();
		}
	}
}