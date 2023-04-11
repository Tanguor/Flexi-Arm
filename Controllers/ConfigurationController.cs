using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Text;
using Serilog;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Models;

namespace Flexi_Arm.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly Areas.Identity.Data.ApplicationDbContext _context;

        public ConfigurationController(ILogger<ConfigurationController> logger, Areas.Identity.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var elt = _context.CommunicationModel;
            return View(elt);
        }

        // Cette action nécessite une autorisation pour les utilisateurs ayant la politique "RequireAdmin"
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Flexibowl()
        {
            var elt = _context.CommunicationModel;
            // Retourne la vue Flexibowl
            return View(elt);
        }

        public IActionResult Selected(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communicationModel = _context.CommunicationModel
                .FirstOrDefault(m => m.Id == id);

            if (communicationModel == null)
            {
                return NotFound();
            }
            else
            {

                TempData["_IdSlct"] = communicationModel.Id;

                return RedirectToAction("Flexibowl");
            }

        }


        public IActionResult Systeme_Vision()
        {
            // Retourne la vue Flexibowl
            return View();
        }

        public IActionResult Bras_Robot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logs()
        {
            // Remplacez "chemin_du_fichier" par le chemin d'accès réel de votre fichier de journalisation
            string logFilePath = "Logs\\FlexiArmLog-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string logContent;

            using (var fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream))
            {
                logContent = streamReader.ReadToEnd();
            }

            return View("Logs", logContent);
        }


        // Cette action nécessite une autorisation pour les utilisateurs ayant la politique "RequireAdmin"
        [Authorize(Policy = "RequireAdmin")]
        public ActionResult COM_Flexibowl(string ip_flexi, int port, string message)
        {
            //Logging (journal)
            var username = HttpContext.User.Identity.Name;
            _logger.LogInformation((EventId)200, "Commande de l'utilisateur:{user} à l'adresse ip:{ip} sur le port {port} le {date}", username, ip_flexi, port, DateTime.Now);
            Log.Warning("L'utilisateur envoie une requete UDP au flexibowl L'echos n'est pas encore implémenté"); //activer si message custom.

            // Redirige vers l'action Flexibowl
            return RedirectToAction("Flexibowl");
        }

        [HttpPost]
        public IActionResult AllumerLumiere(bool toggle)
        {
            if (toggle == true)
            {
                string strCommand = "light=1" + Convert.ToChar(13);
                SendCommand(strCommand);
                return Ok("La lumière est allumée");
            }
            else
            {
                string strCommand = "light=0" + Convert.ToChar(13);
                SendCommand(strCommand);
                return Ok("La lumière est éteinte");
            }

        }
        [HttpPost]
        public IActionResult AllumerServo(bool toggle)
        {
            if (toggle == true)
            {
                string strCommand = "servo=1" + Convert.ToChar(13);
                SendCommand(strCommand);
                return Ok("Le moteur est Allumée");
            }
            else
            {
                string strCommand = "servo=0" + Convert.ToChar(13);
                SendCommand(strCommand);
                return Ok("Le moteur est Eteint");
            }

        }



        // Compteur pour le message envoyé à l'adresse de diffusion
        static int H = 0;

        [HttpPost]
        public IActionResult ForwardAction(int input_1, int input_2, int input_3, int input_4)
        {
            int a = 0;

            // Boucle de connexion
            while (a < 3)
            {
                try
                {
                    // Commandes à envoyer au Flexibowl
                    string StrCommand = "angle=" + input_3 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "speed=" + input_4 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "acc=" + input_1 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "dec=" + input_2 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "forward=1" + Convert.ToChar(13);
                    SendCommand(StrCommand);
                    // Si tout se passe bien, on sort de la boucle
                    break;
                }
                catch (Exception)
                {
                    // Si une erreur se produit, on continue la boucle
                    a++;
                }
            }
            // Do something with the inputs
            return RedirectToAction("Flexibowl");
        }

        [HttpPost]
        public IActionResult BackwardAction(int input_1, int input_2, int input_3, int input_4)
        {
            int a = 0;

            // Boucle de connexion
            while (a < 3)
            {
                try
                {
                    // Commandes à envoyer au Flexibowl

                    string StrCommand = "angle=" + input_3 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "speed=" + input_4 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "acc=" + input_1 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "dec=" + input_2 + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "backward=1" + Convert.ToChar(13);
                    SendCommand(StrCommand);
                    // Si tout se passe bien, on sort de la boucle
                    break;
                }
                catch (Exception)
                {
                    // Si une erreur se produit, on continue la boucle
                    a++;
                }
            }
            // Do something with the inputs
            return RedirectToAction("Flexibowl");
        }



        // Méthode pour envoyer des commandes au Flexibowl
        private void SendCommand(string StrCommand)
        {
            var Id = TempData["_IdSlct"];
            if (Id == null) { Id = 1; };
            var communication = _context.CommunicationModel.Find(Id);
            var ip = communication.Ip;
            var port = communication.Port;
            // Crée un client UDP
            UdpClient udpClient = new UdpClient();

            // Convertit l'encodage ASCII en byte[]
            ASCIIEncoding encoder = new ASCIIEncoding();

            // Établit une connexion
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.3"), 5001);
            udpClient.Connect(RemoteIpEndPoint);

            // Convertit la commande en byte[] et l'envoie
            byte[] b = Encoding.ASCII.GetBytes(StrCommand);
            udpClient.Send(b, b.Length, ip, port);

            // Ferme le client UDP
            udpClient.Close();

            // Attend une demi-seconde avant d'afficher un message dans la console
            Thread.Sleep(50);

            // Affiche un message dans la console
            if (H == 1)
            {
                Console.WriteLine(H + " er message sent to the broadcast address");
            }
            else
            {
                Console.WriteLine(H + "ème message sent to the broadcast address");
            }

            // Incrémente le compteur pour le message envoyé
            H++; 
        }
    }
}


//Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

//IPAddress broadcast = IPAddress.Parse("192.168.0.3");

//byte[] sendbuf = Encoding.ASCII.GetBytes(StrCommand);
//IPEndPoint ep = new IPEndPoint(broadcast, 5001);

//s.SendTo(sendbuf, ep);


