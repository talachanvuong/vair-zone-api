using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAirZoneWebAPI.DTOs.Organizer;
using VAirZoneWebAPI.Interfaces;

namespace VAirZoneWebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrganizerController(IOrganizerRepository organizerRepo, IMapper mapper) : ControllerBase
	{
		[HttpGet]
		[Authorize]
		[Route("get")]
		public async Task<IActionResult> GetOrganziers()
		{
			var organizers = await organizerRepo.GetOrganizers();
			var result = mapper.Map<List<OrganizerDto>>(organizers);
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		[Route("activity/latest")]
		public async Task<IActionResult> GetLatestActivities()
		{
			int size = 6;
			var activities = await organizerRepo.GetLatestActivities(size);
			var result = mapper.Map<List<ActivityDto>>(activities);
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		[Route("{organizerId:int}/pagecount")]
		public async Task<IActionResult> GetPageCount(int organizerId)
		{
			var result = await organizerRepo.GetPageCountById(organizerId);
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		[Route("{organizerId:int}/activity/{page:int}")]
		public async Task<IActionResult> GetActivitiesByPage(int organizerId, int page)
		{
			var activities = await organizerRepo.GetActivitiesByIdOnPage(organizerId, page);
			var result = mapper.Map<List<ActivityDto>>(activities);
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		[Route("{organizerId:int}/activity")]
		public async Task<IActionResult> GetActivities(int organizerId)
		{
			var activities = await organizerRepo.GetActivitiesById(organizerId);
			var result = mapper.Map<List<ActivityDto>>(activities);
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		[Route("errorcatcher")]
		public async Task<IActionResult> GetErrorCatchers()
		{
			var errorCatchers = await organizerRepo.GetErrorCatchers();
			return Ok(errorCatchers);
		}
	}
}