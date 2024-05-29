using CatalogOfFootballPlayers_test1.Models;
using Microsoft.AspNetCore.SignalR;

namespace CatalogOfFootballPlayers_test1.Hubs
{
    public class FootballersHub : Hub
    {
        public async Task SendFootballersUpdate(Footballer footballer)
        {
            await Clients.All.SendAsync("ReceivefootballersUpdate", footballer);
        }
    }
}
