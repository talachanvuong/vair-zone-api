using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VAirZoneWebAPI.DTOs.User;
using VAirZoneWebAPI.Helpers;
using VAirZoneWebAPI.Interfaces;
using VAirZoneWebAPI.Models.User;

namespace VAirZoneWebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController(
		IUserRepository userRepo,
		IPasswordService passwordService,
		IMailService mailService,
		ITokenService tokenService,
		IMapper mapper
		) : ControllerBase
	{
		[HttpGet]
		[Authorize]
		[Route("data")]
		public async Task<IActionResult> GetUser()
		{
			var emailClaim = User.FindFirst(ClaimTypes.Email);

			if (emailClaim == null)
			{
				return Problem(title: ProblemTitles.NOT_LOGIN,
					statusCode: StatusCodes.Status404NotFound);
			}

			var user = await userRepo.GetUserByEmailAsync(emailClaim.Value);

			if (user == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_USER,
					statusCode: StatusCodes.Status404NotFound);
			}

			var result = mapper.Map<UserDto>(user);
			return Ok(result);
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			if (!ModelState.IsValid)
			{
				return Problem(title: ProblemTitles.WRONG_INPUT,
					statusCode: StatusCodes.Status400BadRequest);
			}

			if (await userRepo.GetUserByEmailAsync(registerDto.Email) != null)
			{
				return Problem(title: ProblemTitles.EXIST_USER,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var user = new UserModel()
			{
				DisplayName = registerDto.DisplayName,
				Password = passwordService.CreatePassword(registerDto.Password),
				Email = registerDto.Email,
				Gender = registerDto.Gender,
				Bio = string.Empty,
				AdditionalUrl = string.Empty,
				CreatedOn = DateTime.Now
			};

			await userRepo.CreateAsync(user);
			return Ok();
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			if (!ModelState.IsValid)
			{
				return Problem(title: ProblemTitles.WRONG_INPUT,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var user = await userRepo.GetUserByEmailAsync(loginDto.Email);

			if (user == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_USER,
					statusCode: StatusCodes.Status404NotFound);
			}

			if (!passwordService.VerifyPassword(loginDto.Password, user.Password))
			{
				return Problem(title: ProblemTitles.WRONG_PASSWORD,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var result = new
			{
				token = tokenService.CreateToken(loginDto.Email, loginDto.Password)
			};
			return Ok(result);
		}

		[HttpPatch]
		[Authorize]
		[Route("update")]
		public async Task<IActionResult> Update([FromBody] UpdateDto updateDto)
		{
			if (!ModelState.IsValid)
			{
				return Problem(title: ProblemTitles.WRONG_INPUT,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var emailClaim = User.FindFirst(ClaimTypes.Email);

			if (emailClaim == null)
			{
				return Problem(title: ProblemTitles.NOT_LOGIN,
					statusCode: StatusCodes.Status404NotFound);
			}

			var user = await userRepo.GetUserByEmailAsync(emailClaim.Value);

			if (user == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_USER,
					statusCode: StatusCodes.Status404NotFound);
			}

			if (updateDto.DisplayName != null)
				user.DisplayName = updateDto.DisplayName;

			if (updateDto.Email != null)
				user.Email = updateDto.Email;

			if (updateDto.Bio != null)
				user.Bio = updateDto.Bio;

			if (updateDto.AdditionalUrl != null)
				user.AdditionalUrl = updateDto.AdditionalUrl;

			await userRepo.UpdateAsync(user);
			return Ok();
		}

		[HttpPatch]
		[Authorize]
		[Route("changepassword")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
		{
			if (!ModelState.IsValid)
			{
				return Problem(title: ProblemTitles.WRONG_INPUT,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var emailClaim = User.FindFirst(ClaimTypes.Email);

			if (emailClaim == null)
			{
				return Problem(title: ProblemTitles.NOT_LOGIN,
					statusCode: StatusCodes.Status404NotFound);
			}

			var user = await userRepo.GetUserByEmailAsync(emailClaim.Value);

			if (user == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_USER,
					statusCode: StatusCodes.Status404NotFound);
			}

			if (!passwordService.VerifyPassword(changePasswordDto.OldPassword, user.Password))
			{
				return Problem(title: ProblemTitles.WRONG_PASSWORD,
					statusCode: StatusCodes.Status400BadRequest);
			}

			if (changePasswordDto.NewPassword == changePasswordDto.OldPassword)
			{
				return Problem(title: ProblemTitles.SAME_LAST_PASSWORD,
					statusCode: StatusCodes.Status400BadRequest);
			}

			user.Password = passwordService.CreatePassword(changePasswordDto.NewPassword);

			await userRepo.UpdateAsync(user);
			return Ok();
		}

		[HttpPost]
		[Route("sendcode")]
		public async Task<IActionResult> SendCode([FromBody] RequestCodeDto requestCodeDto)
		{
			if (!ModelState.IsValid)
			{
				return Problem(title: ProblemTitles.WRONG_INPUT,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var user = await userRepo.GetUserByEmailAsync(requestCodeDto.Email);

			if (user == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_USER,
					statusCode: StatusCodes.Status404NotFound);
			}

			var oldUserCode = await userRepo.GetUserCodeByIdAsync(user.UserId);

			if (oldUserCode != null)
			{
				await userRepo.DeleteAsync(oldUserCode);
			}

			var userCode = new UserCodeModel()
			{
				UserId = user.UserId,
				Code = userRepo.GenerateCode(),
				TimeExpired = DateTime.Now.AddMinutes(10)
			};

			await userRepo.CreateAsync(userCode);
			await mailService.SendForgetPasswordCode(user.Email, userCode.Code);
			return Ok();
		}

		[HttpPost]
		[Route("renewpassword")]
		public async Task<IActionResult> RenewPassword([FromBody] RenewPasswordDto forgetPasswordDto)
		{
			if (!ModelState.IsValid)
			{
				return Problem(title: ProblemTitles.WRONG_INPUT,
					statusCode: StatusCodes.Status400BadRequest);
			}

			var user = await userRepo.GetUserByEmailAsync(forgetPasswordDto.Email);

			if (user == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_USER,
					statusCode: StatusCodes.Status404NotFound);
			}

			var userCode = await userRepo.GetUserCodeByIdAsync(user.UserId);

			if (userCode == null)
			{
				return Problem(title: ProblemTitles.NOT_FOUND_CODE,
					statusCode: StatusCodes.Status404NotFound);
			}

			if (userCode.TimeExpired < DateTime.Now)
			{
				return Problem(title: ProblemTitles.EXPIRE_CODE,
					statusCode: StatusCodes.Status400BadRequest);
			}

			if (userCode.Code != forgetPasswordDto.Code)
			{
				return Problem(title: ProblemTitles.WRONG_CODE,
					statusCode: StatusCodes.Status400BadRequest);
			}

			user.Password = passwordService.CreatePassword(forgetPasswordDto.NewPassword);

			await userRepo.UpdateAsync(user);
			await userRepo.DeleteAsync(userCode);
			return Ok();
		}
	}
}