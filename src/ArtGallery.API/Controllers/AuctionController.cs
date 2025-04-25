using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using ArtGallery.API.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ArtGallery.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuctionController : ControllerBase
	{
		private readonly IAuctionService _auctionService;
		private readonly IHubContext<AuctionHub> _hub;

		public AuctionController(IAuctionService auctionService, IHubContext<AuctionHub> hub)
		{
			_auctionService = auctionService;
			_hub = hub;
		}

		// POST api/auction/place-bid
		[HttpPost("place-bid")]
		public async Task<IActionResult> PlaceBid([FromBody] BidDto bidDto)
		{
			if (bidDto == null)
				return BadRequest(new { message = "Bid data is null." });

			try
			{
				var result = await _auctionService.PlaceBidAsync(bidDto);

				await _hub.Clients.All.SendAsync("BidPlaced", new BidDto
				{
					ArtworkId = result.ArtworkId,
					BuyerId = result.BuyerId,
					Amount = result.Amount,
					Timestamp = result.Timestamp
				});

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
