using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.DTOs;

public class UserDto
{
	public int Id { get; set; }
	public string Username { get; set; } = default!;
	public string Email { get; set; } = default!;
	public string Role { get; set; } = default!;
	// optionally Token
	public string? Token { get; set; }
}
