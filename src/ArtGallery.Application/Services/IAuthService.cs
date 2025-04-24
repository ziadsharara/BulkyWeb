using ArtGallery.Application.DTOs;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public interface IAuthService
	{
		Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
		Task<AuthResponseDto> LoginAsync(LoginDto dto);
	}
}
