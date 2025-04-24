﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.DTOs;

public class PagedResult<T>
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public int TotalCount { get; set; }
	public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
}
