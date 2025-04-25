using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Domain.Entities
{
	public class ArtworkLike
	{
		public int ArtworkId { get; set; }
		public Artwork Artwork { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }

		public DateTime LikedAt { get; set; } = DateTime.UtcNow;
	}
}
