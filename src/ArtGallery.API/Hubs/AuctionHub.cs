using Microsoft.AspNetCore.SignalR;
using ArtGallery.Application.DTOs;
using System.Threading.Tasks;

namespace ArtGallery.API.Hubs
{
	public class AuctionHub : Hub
	{
		public async Task BroadcastBid(BidDto bid)
		{
			await Clients.All.SendAsync("BidPlaced", bid);
		}
	}
}
