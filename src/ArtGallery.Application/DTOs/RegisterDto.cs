﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.DTOs;

public class RegisterDto
{
	public string Username { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string Role { get; set; }
}
