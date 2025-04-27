using ArtGallery.Application.DTOs;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public interface IArtworkService
	{
		Task<ArtworkDto> CreateAsync(CreateArtworkDto dto, int userId);
		Task<PagedResult<ArtworkDto>> GetAllAsync(string? category, string? tag, string sortBy, string sortDirection, int pageNumber, int pageSize);
		Task<PagedResult<ArtworkDto>> FilterAndSortAsync(ArtworkFilterDto filter);
		Task<ArtworkDto?> GetByIdAsync(int id);
		Task<bool> UpdateAsync(int id, ArtworkDto dto);
		Task<bool> DeleteAsync(int id);

	}
}
