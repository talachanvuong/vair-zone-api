using AutoMapper;
using VAirZoneWebAPI.DTOs.Organizer;
using VAirZoneWebAPI.Models.Organizer;

namespace VAirZoneWebAPI.Mappers
{
	public class OrganizerProfile : Profile
	{
		public OrganizerProfile()
		{
			CreateMap<OrganizerModel, OrganizerDto>();
			CreateMap<ActivityModel, ActivityDto>();
		}
	}
}