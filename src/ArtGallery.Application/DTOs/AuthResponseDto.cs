namespace ArtGallery.Application.DTOs
{
	// --------------------------------------------------------------
	// DTO representing the response after a user authenticates (login)
	// --------------------------------------------------------------
	public class AuthResponseDto
	{
		// --------------------------------------------------------------
		// The unique identifier of the user
		// --------------------------------------------------------------
		public int Id { get; set; }

		// --------------------------------------------------------------
		// The username of the authenticated user
		// --------------------------------------------------------------
		public string Username { get; set; } = string.Empty;

		// --------------------------------------------------------------
		// The email of the authenticated user
		// --------------------------------------------------------------
		public string Email { get; set; } = string.Empty;

		// --------------------------------------------------------------
		// The role of the user (Admin, Artist, Buyer)
		// --------------------------------------------------------------
		public string Role { get; set; } = string.Empty;

		// --------------------------------------------------------------
		// The authentication token returned after successful login
		// --------------------------------------------------------------
		public string Token { get; set; } = string.Empty;
	}
}
