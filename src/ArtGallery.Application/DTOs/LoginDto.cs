namespace ArtGallery.Application.DTOs
{
	/// <summary>
	/// Data Transfer Object (DTO) used for user login.
	/// Contains the necessary information to authenticate a user.
	/// </summary>
	public class LoginDto
	{
		/// <summary>
		/// The user's email address used for login.
		/// Must be a valid email format.
		/// </summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// The user's password used for authentication.
		/// This should be securely hashed when stored or compared.
		/// </summary>
		public string Password { get; set; } = string.Empty;
	}
}
