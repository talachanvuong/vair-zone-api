using VAirZoneWebAPI.DTOs.Organizer;
using VAirZoneWebAPI.Models.Organizer;

namespace VAirZoneWebAPI.Interfaces
{
	public interface IOrganizerRepository
	{
		Task CreateAsync<T>(T record) where T : class;
		Task DeleteAsync<T>(T record) where T : class;

		//Get all organziers 
		Task<List<OrganizerModel>> GetOrganizers();

		//Get activities of 1 organizer on specific page
		Task<List<ActivityModel>> GetActivitiesByIdOnPage(int organzierId, int page);

		//Get all activities of 1 organizer
		Task<List<ActivityModel>> GetActivitiesById(int organzierId);

		//Get page count activities of 1 organizer
		Task<PageCountDto> GetPageCountById(int organzierId);

		// Get top latest activities
		Task<List<ActivityModel>> GetLatestActivities(int size);

		// Get all errorCatchers
		Task<List<ErrorCatcherDto>> GetErrorCatchers();
	}
}