using GoogleCalenderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace GoogleCalenderApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult OauthRedirect()
        {
            var credentialFiles = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\credentionals.json";
      
            JObject credential= JObject.Parse(System.IO.File.ReadAllText(credentialFiles));
            var client_Id = credential["client_id"];

            var redirectUrl = " https://accounts.google.com/o/oauth2/v2/auth?"+
                "scope= https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                "access_type=offline&"+
                "include_granted_scopes=true&"+
                "response_type=code&"+
                "state=hellohere&"+
                "redirect_uri=https://localhost:44346/OAuth/Callback&" +
                "client_id="+client_Id;
            return Redirect(redirectUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
