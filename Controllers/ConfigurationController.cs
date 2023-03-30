using Flexi_Arm.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using Serilog;
using Microsoft.Extensions.Logging;


namespace Flexi_Arm.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController( ILogger<ConfigurationController> logger)
        {
            _logger = logger;
        }


        // Cette action nécessite une autorisation pour les utilisateurs ayant la politique "RequireAdmin"
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Flexibowl()
        {
            // Retourne la vue Flexibowl
            return View();
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
            // Tentative de connexion
            int a = 0;

            // Boucle de connexion
            while (a < 3)
            {
                try
                {
                    // Commandes à envoyer au Flexibowl
                    string StrCommand = "servo=1" + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "angle=200" + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "speed=20" + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "acc=20" + Convert.ToChar(13);
                    SendCommand(StrCommand);

                    StrCommand = "dec=20" + Convert.ToChar(13);
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

            //Logging (journal)
            var username = HttpContext.User.Identity.Name;
            _logger.LogInformation((EventId)200, "Commande de l'utilisateur:{user} à l'adresse ip:{ip} sur le port {port} le {date}", username, ip_flexi, port, DateTime.Now);
            Log.Warning("L'utilisateur envoie une requete UDP au flexibowl"); //activer si message custom.

            // Redirige vers l'action Flexibowl
            return RedirectToAction("Flexibowl");
        }

        // Compteur pour le message envoyé à l'adresse de diffusion
        static int H  = 0;

        // Méthode pour envoyer des commandes au Flexibowl
        private void SendCommand(string StrCommand)
        {
            // Crée un client UDP
            UdpClient udpClient = new UdpClient();

            // Convertit l'encodage ASCII en byte[]
            ASCIIEncoding encoder = new ASCIIEncoding();

            // Établit une connexion
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.3"), 5001);
            udpClient.Connect(RemoteIpEndPoint);

            // Convertit la commande en byte[] et l'envoie
            byte[] b = encoder.GetBytes(StrCommand);
            udpClient.Send(b, b.Length);

            // Ferme le client UDP
            udpClient.Close();

            // Attend une demi-seconde avant d'afficher un message dans la console
            Thread.Sleep(500);

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


