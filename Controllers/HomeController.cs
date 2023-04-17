using Flexi_Arm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Flexi_Arm.Controllers { 



// Déclaration de la classe HomeController qui hérite de la classe Controller
public class HomeController : Controller
    {
        // Déclaration d'un champ de type ILogger pour gérer les logs
        ILogger<HomeController> _logger;

        // Constructeur de la classe HomeController qui prend un objet ILogger en argument
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("About page visited at {DT}",
                DateTime.UtcNow.ToLongTimeString());
        }

        // Action qui retourne la vue Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogPage()
        {
            return View();
        }

        public IActionResult LogMessage(string logLevel, string msg)
        {
            _logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), logLevel, true), msg);
            return Ok();
        }

        // Action qui requiert une autorisation de la politique "RequireAdmin" pour retourner la vue Configuration
        [Authorize(Policy = "RequireAdmin")]
    public IActionResult Configuration()
    {
        return View();
    }

    // Action qui gère l'appui sur un bouton pour envoyer une commande de démarrage à un serveur TCP
    public ActionResult HandleButtonOn()
    {
        int a = 0;
    // Etiquette de saut pour la boucle de reconnexion
    connection:
        try
        {
            // Création d'un client TCP et connexion au serveur à l'adresse 127.0.0.1 sur le port 1302
            TcpClient client = new TcpClient("127.0.0.1", 1302);
            string messageToSend = "Commande_de_demarrage";
            int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
            byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

            // Obtention du flux de réseau associé au client TCP et envoi des données
            NetworkStream stream = client.GetStream();
            stream.Write(sendData, 0, sendData.Length);
            Console.WriteLine("sending data to server...");

            // Lecture de la réponse du serveur et fermeture de la connexion
            StreamReader sr = new StreamReader(stream);
            string response = sr.ReadLine();
            Console.WriteLine(response);

            stream.Close();
            client.Close();

            // Stockage de la réponse dans un objet TempData pour être utilisé ultérieurement
            TempData["MessageReu"] = response;
        }
        catch (Exception )
        {
            // Si la connexion échoue, stockage d'un message d'erreur dans un objet TempData et tentative de reconnexion
            TempData["Messagefail"] = "échec de la connexion";
            Console.WriteLine("failed to connect...");
                while (a != 1)
            {
                a++;
                goto connection;
            }
        }


        // Redirection vers la vue Index
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
}

}