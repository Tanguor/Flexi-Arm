using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace Flexi_Arm.Controllers
{
    public class ConfigurationController : Controller
    {
        [Authorize(Policy = "RequireAdmin")]//192.168.143.1
        public IActionResult Flexibowl()
        {

            return View();
        }

        [Authorize(Policy = "RequireAdmin")]
        public ActionResult COM_Flexibowl(string ip, int port, string message)
        {
            int a = 0;
        connection:
            try
            {
                string StrCommand = "servo=1" + Convert.ToChar(13);
                SendCommand(StrCommand);

                StrCommand = "angle=200" + Convert.ToChar(13);
                SendCommand(StrCommand);

                StrCommand = "speed=20"  + Convert.ToChar(13);
                SendCommand(StrCommand);

                StrCommand = "acc=20" + Convert.ToChar(13);
                SendCommand(StrCommand);

                StrCommand = "dec=20" + Convert.ToChar(13);
                SendCommand(StrCommand);

                StrCommand = "forward=1" + Convert.ToChar(13);
                SendCommand(StrCommand);
                //UdpClient udpClient = new UdpClient();
                //ASCIIEncoding encoder = new ASCIIEncoding();
                //string messageToSend = message + Convert.ToChar(13);
                //IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                //udpClient.Connect(RemoteIpEndPoint);
                //byte[] b = encoder.GetBytes(messageToSend);
                //udpClient.Send(b, b.Length);
                //udpClient.Close();
                //System.Threading.Thread.Sleep(50);
                //Console.WriteLine("sending data to server...");

            }
            catch (Exception)
            {
                while (a != 1)
                {
                    a++;
                    goto connection;
                }
            }
            return RedirectToAction("Flexibowl");
        }

        static int H  = 0;
        private void SendCommand(string StrCommand)
        {
            UdpClient udpClient = new UdpClient();
            ASCIIEncoding encoder = new ASCIIEncoding();
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5001);
            udpClient.Connect(RemoteIpEndPoint);
            byte[] b = encoder.GetBytes(StrCommand);
            udpClient.Send(b, b.Length);
            udpClient.Close();
            Thread.Sleep(50);

            //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //IPAddress broadcast = IPAddress.Parse("192.168.0.3");

            //byte[] sendbuf = Encoding.ASCII.GetBytes(StrCommand);
            //IPEndPoint ep = new IPEndPoint(broadcast, 5001);

            //s.SendTo(sendbuf, ep);
            Thread.Sleep(500);

            Console.WriteLine(H + "Message sent to the broadcast address");
            H++; 
        }
    }
}
