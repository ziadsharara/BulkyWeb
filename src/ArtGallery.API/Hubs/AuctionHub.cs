using Microsoft.AspNetCore.SignalR;
using ArtGallery.Application.DTOs;
using System.Threading.Tasks;

namespace ArtGallery.API.Hubs
{
	// SignalR Hub: This Hub is responsible for broadcasting bid updates to all connected clients.
	public class AuctionHub : Hub
	{
		// --------------------------------------------------------------------------------
		// BroadcastBid: Method to send the bid details to all connected clients in real-time.
		// This method is invoked from the server-side whenever a new bid is placed.
		// --------------------------------------------------------------------------------
		public async Task BroadcastBid(BidDto bid)
		{
			// Broadcast the bid data to all connected clients
			await Clients.All.SendAsync("BidPlaced", bid);
		}
	}
}
