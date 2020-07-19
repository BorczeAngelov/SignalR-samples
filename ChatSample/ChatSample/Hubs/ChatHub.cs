using Chat.Utils.SignalR.ChatHub;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatSample.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        public async Task UploadMessage(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync(nameof(IChatHubClient.ReceiveServerMessage), name, message);
        }
    }
}