namespace ArtGallery.Application.DTOs
{
	/// <summary>
	/// Data Transfer Object (DTO) used for user registration.
	/// Contains the necessary information to create a new user account.
	/// </summary>
	public class RegisterDto
	{
		/// <summary>
		/// The user's full name.
		/// </summary>
		public string Username { get; set; } = string.Empty;

		/// <summary>
		/// The user's email address.
		/// Must be unique and in a valid email format.
		/// </summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// The user's password.
		/// Should be stored securely after hashing.
		/// </summary>
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// The role assigned to the user (e.g., Admin, Artist, Buyer).
		/// </summary>
		public string Role { get; set; } = string.Empty;
	}
}
