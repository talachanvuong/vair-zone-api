using VAirZoneWebAPI.Models.User;

namespace VAirZoneWebAPI.Interfaces
{
	public interface IUserRepository
	{
		Task CreateAsync<T>(T record) where T : class;
		Task DeleteAsync<T>(T record) where T : class;
		Task UpdateAsync<T>(T record) where T : class;
		Task<UserModel?> GetUserByIdAsync(int userId);
		Task<UserModel?> GetUserByEmailAsync(string email);
		Task<UserCodeModel?> GetUserCodeByIdAsync(int userId);
		string GenerateCode();
	}
}