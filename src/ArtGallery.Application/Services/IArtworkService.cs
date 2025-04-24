using System.Collections.Generic;
using System.Threading.Tasks;
using ArtGallery.Application.DTOs;

namespace ArtGallery.Application.Services
{
	public interface IArtworkService
	{
		Task<ArtworkDto> CreateAsync(ArtworkDto artworkDto);
		Task<IEnumerable<ArtworkDto>> GetAllAsync();
		Task<ArtworkDto> GetByIdAsync(int id);
		Task<bool> UpdateAsync(int id, ArtworkDto artworkDto);
		Task<bool> DeleteAsync(int id);
	}
}
