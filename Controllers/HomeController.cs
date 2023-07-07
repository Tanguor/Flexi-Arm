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
        private readonly IHostApplicationLifetime _hostApplicationLifetime;


        public HomeController(ILogger<HomeController> logger, Areas.Identity.Data.ApplicationDbContext context, IOptions<DefaultRecetteOptions> options, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _context = context;
            _defaultRecetteId = options.Value.Id;
            _hostApplicationLifetime = hostApplicationLifetime;
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


            var recette = _context.Recette.FirstOrDefault(r => r.Id == _defaultRecetteId); // Supposons que la propriété Id existe dans le modèle Recette
            var robotdata = _context.Bras_Robot.Find(recette.id_Robot);
            var flexidata = _context.Flexibowl.Find(recette.id_Flexi);
            var Jobdata = _context.Recette.Find(recette.Id_Camera);



            //envoies des instructions Robot
            string StrCommand = "1;" + robotdata.speedapproach + ";" + robotdata.speedfree + ";" + flexidata.sh_count + ";" + flexidata.sh_speed + ";" + flexidata.cw_angle + ";" + flexidata.cww_angle + ";" + recette.Jobs + Convert.ToChar(13);
            SendCommandTCP(StrCommand);

            int a = 0;
            //Envoi de la commande au flexibowl
            // Boucle de connexion
            return RedirectToAction("Index");
        }

        // Action qui gère l'appui sur un bouton pour envoyer un acquittement à un serveur TCP
        public ActionResult HandleButtonAcq()
        {

            return RedirectToAction("Index");
        }

        // Action qui gère l'appui sur un bouton pour envoyer un acquittement à un serveur TCP
        public ActionResult HandleButtonOff()
        {
            _hostApplicationLifetime.StopApplication();

            return RedirectToAction("Index");
        }


        // Cette méthode est chargée de renvoyer une vue Error en cas d'erreur de l'application. Elle est décorée avec l'attribut ResponseCache pour indiquer au navigateur de ne pas mettre en cache la page d'erreur.
        // Elle renvoie une instance de ErrorViewModel contenant un ID de requête ou un identifiant de trace HttpContext.TraceIdentifier.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

   
        private string SendCommandTCP(string StrCommand)
        {
            try
            {
                // Adresse IP et numéro de port du destinataire
                var recetteid = _context.Recette.Find(_defaultRecetteId);
                var robotdata = _context.Bras_Robot.Find(recetteid.id_Robot) ;

                IPAddress ip = IPAddress.Parse(robotdata.Ip);
                int port = robotdata.Port;

                // Création du socket TCP
                TcpClient client = new TcpClient();
                client.Connect(ip, port);
                Thread.Sleep(200);
                // Conversion de la chaîne de caractères en tableau de bytes
                byte[] buffer = Encoding.UTF8.GetBytes(StrCommand);
                // Envoi du message
                NetworkStream stream = client.GetStream();
                Thread.Sleep(1000);
                stream.Write(buffer, 0, buffer.Length);
                Thread.Sleep(1000);
                Console.WriteLine("Message envoyé: " + StrCommand);
                // Fermeture de la connexion
                client.Close();
                return ("message sended");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return ("Erreur : " + ex.Messag);
            }
        }
    }
}