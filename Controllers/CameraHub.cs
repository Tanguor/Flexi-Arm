using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Flexi_Arm.Controllers
{


    public class CameraHub : Hub
    {
        public async Task ReceiveImage(byte[] imageData)
        {
            // Broadcast image data to all connected clients
            await Clients.All.SendAsync("ReceiveImage", imageData);
        }
    }
}
