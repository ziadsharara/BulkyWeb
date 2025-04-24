using ArtGallery.Application.DTOs;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public interface IAuthService
	{
		Task<UserDto> RegisterAsync(RegisterDto dto);
		Task<string> LoginAsync(LoginDto dto);
	}
}
