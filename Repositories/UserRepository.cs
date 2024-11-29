using Microsoft.EntityFrameworkCore;
using VAirZoneWebAPI.Databases;
using VAirZoneWebAPI.Interfaces;
using VAirZoneWebAPI.Models.User;

namespace VAirZoneWebAPI.Repositories
{
	public class UserRepository(ApplicationDbContext applicationDbContext) : IUserRepository
	{
		public async Task CreateAsync<T>(T record) where T : class
		{
			await applicationDbContext.Set<T>().AddAsync(record);
			await applicationDbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync<T>(T record) where T : class
		{
			applicationDbContext.Set<T>().Update(record);
			await applicationDbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync<T>(T record) where T : class
		{
			applicationDbContext.Set<T>().Remove(record);
			await applicationDbContext.SaveChangesAsync();
		}

		public async Task<UserModel?> GetUserByIdAsync(int userId)
		{
			return await applicationDbContext.Users.FirstOrDefaultAsync(record => record.UserId == userId);
		}

		public async Task<UserModel?> GetUserByEmailAsync(string email)
		{
			return await applicationDbContext.Users.FirstOrDefaultAsync(record => record.Email == email);
		}

		public async Task<UserCodeModel?> GetUserCodeByIdAsync(int userId)
		{
			return await applicationDbContext.UserCodes.FirstOrDefaultAsync(record => record.UserId == userId);
		}

		public string GenerateCode()
		{
			return Random.Shared.NextDouble().ToString().Substring(2, 4);
		}
	}
}