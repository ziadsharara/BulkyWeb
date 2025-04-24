using ArtGallery.Application.DTOs;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public interface IArtworkService
	{
		Task<PagedResult<ArtworkDto>> GetAllAsync(
				string? category = null,
				string? tag = null,
				string sortBy = "date",
				string sortDirection = "desc",
				int pageNumber = 1,
				int pageSize = 10
		);

		Task<ArtworkDto?> GetByIdAsync(int id);
		Task<ArtworkDto> CreateAsync(CreateArtworkDto dto);
		Task<bool> UpdateAsync(int id, ArtworkDto dto);
		Task<bool> DeleteAsync(int id);
	}
}
