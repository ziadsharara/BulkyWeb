using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services;

using ArtGallery.Application.DTOs;

public interface IUserService
{
	Task<UserDto> RegisterAsync(RegisterDto dto);
	Task<UserDto?> LoginAsync(LoginDto dto);
}