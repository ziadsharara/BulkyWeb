using ArtGallery.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	/// <summary>
	///     Business logic for admin operations: approving/rejecting artists and artworks.
	/// </summary>
	public interface IAdminService
	{
		/// <summary>
		///     Gets all artists whose registrations are pending approval.
		/// </summary>
		Task<IEnumerable<UserDto>> GetPendingArtistsAsync();

		/// <summary>
		///     Approves the specified artist, enabling them to post artworks.
		/// </summary>
		/// <param name="artistId">ID of the artist to approve.</param>
		Task<bool> ApproveArtistAsync(int artistId);

		/// <summary>
		///     Rejects the specified artist registration and removes their account.
		/// </summary>
		/// <param name="artistId">ID of the artist to reject.</param>
		Task<bool> RejectArtistAsync(int artistId);

		/// <summary>
		///     Gets all artworks whose submissions are pending approval.
		/// </summary>
		Task<IEnumerable<ArtworkDto>> GetPendingArtworksAsync();

		/// <summary>
		///     Approves the specified artwork, making it visible to buyers.
		/// </summary>
		/// <param name="artworkId">ID of the artwork to approve.</param>
		Task<bool> ApproveArtworkAsync(int artworkId);

		/// <summary>
		///     Rejects the specified artwork submission and removes it from the system.
		/// </summary>
		/// <param name="artworkId">ID of the artwork to reject.</param>
		Task<bool> RejectArtworkAsync(int artworkId);
	}
}
