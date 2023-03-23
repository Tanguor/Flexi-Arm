using Flexi_Arm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
namespace Flexi_Arm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Configuration()
        {
            return View();
        }

        public ActionResult HandleButtonOn()
        {
            int a = 0;
        connection:
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 1302);
                string messageToSend = "Commande_de_demarrage";
                int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine("sending data to server...");

                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                TempData["MessageReu"] = response;
            }
            catch (Exception )
            {
                TempData["Messagefail"] = "échec de la connexion";
                Console.WriteLine("failed to connect...");
                while (a != 1)
                {
                    a++;
                    goto connection;
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult HandleButtonAcq()
        {
            int a = 0;
        connection:
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 1302);
                string messageToSend = "Acquitement";
                int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine("sending data to server...");

                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                TempData["AcquitementReu"] = response;
            }
            catch (Exception)
            {
                TempData["Messagefail"] = "échec de la connexion";
                Console.WriteLine("failed to connect...");
                while (a != 2)
                {
                    a++;
                    goto connection;
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult HandleButtonOff()
        {
            int a = 0;
        connection:
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 1302);
                string messageToSend = "Commande_Off";
                int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine("sending data to server...");

                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                TempData["ExctinctionReu"] = response;
            }
            catch (Exception)
            {
                TempData["Messagefail"] = "Echec";
                Console.WriteLine("failed to connect...");                
                while (a != 2)
                {
                    a++;
                    goto connection;
                }                
            }

            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}