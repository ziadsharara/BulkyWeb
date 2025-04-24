using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ArtGallery.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuctionController : ControllerBase
	{
		private readonly IAuctionService _auctionService;
		public AuctionController(IAuctionService auctionService)
				=> _auctionService = auctionService;

		// POST api/auction/place-bid
		[HttpPost("place-bid")]
		public async Task<IActionResult> PlaceBid([FromBody] BidDto bidDto)
		{
			if (bidDto == null)
				return BadRequest(new { message = "Bid data is null." });

			try
			{
				var result = await _auctionService.PlaceBidAsync(bidDto);
				return CreatedAtAction(
						nameof(GetBidsForArtwork),
						new { artworkId = result.ArtworkId },
						result
				);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		// GET api/auction/bids/{artworkId}
		[HttpGet("bids/{artworkId}")]
		public async Task<IActionResult> GetBidsForArtwork(int artworkId)
		{
			try
			{
				var bids = await _auctionService.GetBidsForArtworkAsync(artworkId);
				return Ok(bids);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}
	}
}
