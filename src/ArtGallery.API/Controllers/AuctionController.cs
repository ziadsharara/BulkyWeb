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

		// Constructor: Inject the auction service and SignalR hub
		public AuctionController(IAuctionService auctionService, IHubContext<AuctionHub> hub)
		{
			_auctionService = auctionService;
			_hub = hub;
		}

		// --------------------------------------------------------------------------------
		// POST api/auction/place-bid
		// Places a bid on an artwork during the auction, validates the bid data, 
		// and sends the bid update to all connected clients via SignalR.
		// --------------------------------------------------------------------------------
		[HttpPost("place-bid")]
		public async Task<IActionResult> PlaceBid([FromBody] BidDto bidDto)
		{
			// Check if bid data is provided
			if (bidDto == null)
				return BadRequest(new { message = "Bid data is null." });

			// Ensure the bid amount is valid
			if (bidDto.Amount <= 0)
				return BadRequest(new { message = "Bid amount must be greater than zero." });

			try
			{
				// Place the bid through the auction service
				var result = await _auctionService.PlaceBidAsync(bidDto);

				// Send bid update to all clients connected to the auction hub
				await _hub.Clients.All.SendAsync("BidPlaced", new BidDto
				{
					ArtworkId = result.ArtworkId,
					BuyerId = result.BuyerId,
					Amount = result.Amount,
					BidTime = result.BidTime
				});

				// Return the created bid details
				return CreatedAtAction(
						nameof(GetBidsForArtwork),
						new { artworkId = result.ArtworkId },
						result
				);
			}
			catch (KeyNotFoundException ex)
			{
				// Handle case where the artwork or user does not exist
				return NotFound(new { message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				// Handle case where the operation is invalid (e.g., bid amount too low)
				return BadRequest(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				// General exception handling
				return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
			}
		}

		// --------------------------------------------------------------------------------
		// GET api/auction/bids/{artworkId}
		// Retrieves all bids for a specific artwork.
		// --------------------------------------------------------------------------------
		[HttpGet("bids/{artworkId}")]
		public async Task<IActionResult> GetBidsForArtwork(int artworkId)
		{
			try
			{
				// Retrieve all bids for the artwork from the auction service
				var bids = await _auctionService.GetBidsForArtworkAsync(artworkId);
				return Ok(bids);
			}
			catch (KeyNotFoundException ex)
			{
				// Handle case where the artwork does not exist
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				// General exception handling
				return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
			}
		}
	}
}
