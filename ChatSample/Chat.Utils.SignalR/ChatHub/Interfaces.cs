using System.Threading.Tasks;

namespace Chat.Utils.SignalR.ChatHub
{
    public interface IChatHubClient
    {
        void ReceiveServerMessage(string user, string message);
    }

    public interface IChatHub
    {
        Task UploadMessage(string name, string message);
    }
}
