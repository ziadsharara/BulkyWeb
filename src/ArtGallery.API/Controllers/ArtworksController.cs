using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtGallery.API.Hubs;

namespace ArtGallery.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtworkController : ControllerBase
	{
		private readonly IArtworkService _artworkService;
		private readonly IHubContext<AuctionHub> _auctionHubContext;  // WebSocket (SignalR) Hub

		public ArtworkController(IArtworkService artworkService, IHubContext<AuctionHub> bidHubContext)
		{
			_artworkService = artworkService;
			_auctionHubContext = bidHubContext;
		}

		// --------------------------------------------------------------------------------
		// Create a new artwork
		// --------------------------------------------------------------------------------
		[HttpPost]
		public async Task<ActionResult<ArtworkDto>> Create([FromBody] CreateArtworkDto dto)
		{
			try
			{
				var userId = 1; // This should be fetched from the authenticated user context
				var artwork = await _artworkService.CreateAsync(dto, userId);
				return CreatedAtAction(nameof(GetById), new { id = artwork.Id }, artwork);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Retrieve all artworks with filters, sorting, and pagination
		// --------------------------------------------------------------------------------
		[HttpGet]
		public async Task<ActionResult<PagedResult<ArtworkDto>>> GetAll(
						[FromQuery] string? category,
						[FromQuery] string? tag,
						[FromQuery] string sortBy = "createdat",
						[FromQuery] string sortDirection = "desc",
						[FromQuery] int pageNumber = 1,
						[FromQuery] int pageSize = 10,
						[FromQuery] string? artistName = null,
						[FromQuery] string? tags = null)
		{
			try
			{
				var result = await _artworkService.GetAllAsync(category, tag, sortBy, sortDirection, pageNumber, pageSize, artistName, tags);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Get a single artwork by ID
		// --------------------------------------------------------------------------------
		[HttpGet("{id}")]
		public async Task<ActionResult<ArtworkDto>> GetById(int id)
		{
			var artwork = await _artworkService.GetByIdAsync(id);
			if (artwork == null) return NotFound();
			return Ok(artwork);
		}

		// --------------------------------------------------------------------------------
		// Update an existing artwork
		// --------------------------------------------------------------------------------
		[HttpPut("{id}")]
		public async Task<ActionResult> Update(int id, [FromBody] ArtworkDto dto)
		{
			try
			{
				var success = await _artworkService.UpdateAsync(id, dto);
				if (!success) return NotFound();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Delete an artwork by ID
		// --------------------------------------------------------------------------------
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			try
			{
				var success = await _artworkService.DeleteAsync(id);
				if (!success) return NotFound();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Place a bid on an artwork
		// --------------------------------------------------------------------------------
		[HttpPost("{artworkId}/bid")]
		public async Task<ActionResult> PlaceBid(int artworkId, [FromBody] BidDto bidDto)
		{
			try
			{
				var success = await _artworkService.PlaceBidAsync(artworkId, bidDto.BuyerId, bidDto.Amount);
				if (!success) return BadRequest("Bid failed.");

				// Notify clients in real-time via WebSocket
				await _auctionHubContext.Clients.All.SendAsync("ReceiveBidUpdate", artworkId, bidDto.Amount, DateTime.Now);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Get the bid history for an artwork
		// --------------------------------------------------------------------------------
		[HttpGet("{artworkId}/bids")]
		public async Task<ActionResult<IEnumerable<BidDto>>> GetBidHistory(int artworkId)
		{
			try
			{
				var bids = await _artworkService.GetBidHistoryAsync(artworkId);
				return Ok(bids);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Extend auction time for an artwork
		// --------------------------------------------------------------------------------
		[HttpPut("{artworkId}/extend-auction")]
		public async Task<ActionResult> ExtendAuctionTime(int artworkId, [FromBody] DateTime newEndTime)
		{
			try
			{
				var artistId = 1; // This should be fetched from the authenticated user context
				var success = await _artworkService.ExtendAuctionTimeAsync(artworkId, newEndTime, artistId);
				if (!success) return BadRequest("Failed to extend auction time.");

				// Notify clients in real-time via WebSocket about auction extension
				await _auctionHubContext.Clients.All.SendAsync("ReceiveAuctionExtension", artworkId, newEndTime);

				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// --------------------------------------------------------------------------------
		// Get the winner of an artwork's auction
		// --------------------------------------------------------------------------------
		[HttpGet("{artworkId}/winner")]
		public async Task<ActionResult<BidDto>> GetWinner(int artworkId)
		{
			try
			{
				var artistId = 1; // This should be fetched from the authenticated user context
				var winner = await _artworkService.GetWinnerAsync(artworkId, artistId);
				if (winner == null) return NotFound();
				return Ok(winner);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
