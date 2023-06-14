using Flexi_Arm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Flexi_Arm.Controllers
{
    // Déclaration de la classe HomeController qui hérite de la classe Controller
    public class HomeController : Controller
    {
        // Déclaration d'un champ de type ILogger pour gérer les logs
        private readonly ILogger<HomeController> _logger;
        private readonly Areas.Identity.Data.ApplicationDbContext _context;
        private readonly int _defaultRecetteId;

        public HomeController(ILogger<HomeController> logger, Areas.Identity.Data.ApplicationDbContext context, IOptions<DefaultRecetteOptions> options)
        {
            _logger = logger;
            _context = context;
            _defaultRecetteId = options.Value.Id;
        }

        public void OnGet()
        {
            _logger.LogInformation("About page visited at {DT}",
                DateTime.UtcNow.ToLongTimeString());
        }

        // Action qui retourne la vue Index
        public IActionResult Index()
        {

            // Chercher la recette avec l'id correspondant
            var recette = _context.Recette.FirstOrDefault(r => r.Id == _defaultRecetteId); // Supposons que la propriété Id existe dans le modèle Recette
            {
                return View("Index", recette.Name);
            }
            

        }

        public IActionResult LogPage()
        {
            return View();
        }


        // Action qui requiert une autorisation de la politique "RequireAdmin" pour retourner la vue Configuration
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Configuration()
        {
            return View();
        }

        // Action qui gère l'appui sur un bouton pour envoyer une commande de démarrage à un serveur TCP
        public IActionResult HandleButtonOn()
        {
            int b = 0;
            var robotdata = _context.Bras_Robot.Find(3);
            var flexidata = _context.Flexibowl.Find(3);
            var Jobdata = _context.Recette.Find(5);

            //envoies des instructions Robot
            string StrCommand = "1," + robotdata.speedapproach + "," + robotdata.speedfree + "," + flexidata.sh_count + "," + flexidata.sh_speed + "," + flexidata.cw_angle + "," + flexidata.cww_angle + "," + Jobdata.Jobs + Convert.ToChar(13);
            SendCommandTCP(StrCommand);

            int a = 0;
            //Envoi de la commande au flexibowl
            // Boucle de connexion
            return RedirectToAction("Index");
        }

        // Action qui gère l'appui sur un bouton pour envoyer un acquittement à un serveur TCP
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

        // Action qui gère l'appui sur un bouton pour envoyer un acquittement à un serveur TCP
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


        // Cette méthode est chargée de renvoyer une vue Error en cas d'erreur de l'application. Elle est décorée avec l'attribut ResponseCache pour indiquer au navigateur de ne pas mettre en cache la page d'erreur.
        // Elle renvoie une instance de ErrorViewModel contenant un ID de requête ou un identifiant de trace HttpContext.TraceIdentifier.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static int H = 0;
        private void SendCommand(string StrCommand)
        {
            var Id = TempData["_IdSlctR"];
            if (Id == null) { Id = 1; };
            var communication = _context.Flexibowl.Find(Id);
            var ip = communication.Ip;
            var port = communication.Port;
            // Crée un client UDP
            UdpClient udpClient = new UdpClient();

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

        private void SendCommandTCP(string StrCommand)
        {
            try
            {
                // Adresse IP et numéro de port du destinataire
                IPAddress ip = IPAddress.Parse("192.168.0.2");
                int port = 5002;

                // Création du socket TCP
                TcpClient client = new TcpClient();
                client.Connect(ip, port);
                Thread.Sleep(200);
                // Conversion de la chaîne de caractères en tableau de bytes
                byte[] buffer = Encoding.UTF8.GetBytes(StrCommand);
                // Envoi du message
                NetworkStream stream = client.GetStream();
                Thread.Sleep(200);
                stream.Write(buffer, 0, buffer.Length);
                Thread.Sleep(1000);
                Console.WriteLine("Message envoyé: " + StrCommand);
                // Fermeture de la connexion
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }

        private int ListenTCP()
        {
            TcpListener server = null;
            string responseData = "0";
            try
            {
                // Définit l'adresse IP et le port à écouter
                IPAddress localAddr = IPAddress.Parse("192.168.0.2");
                int port = 5003;

                // Crée un objet TcpListener pour écouter les connexions entrantes
                server = new TcpListener(IPAddress.Any, port);

                // Démarrer l'écoute
                server.Start();
                Console.WriteLine("En attente de connexion...");

                // Accepter une connexion cliente
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connexion acceptée.");

                // Traitement de la requête du client
                NetworkStream stream = client.GetStream();

                // Lecture des données envoyées par le client
                byte[] data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Données reçues : {0}", responseData);

                // Fermer la connexion
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : {0}", e);
            }
            finally
            {
                // Fermer la connexion
                server.Stop();
            }
            return Int16.Parse(responseData);

        }

    }
}