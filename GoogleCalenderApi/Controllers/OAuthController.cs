using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text;

namespace GoogleCalenderApi.Controllers
{
    public class OAuthController : Controller
    {
        public void Callback(string code,string error,string state)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                this.GetTokens(code);
            }
        }
        public  IActionResult GetTokens(string code)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            var credentialFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\credentionals.json";
            JObject credentials = JObject.Parse(System.IO.File.ReadAllText(credentialFile));
          
            RestRequest restRequest = new RestRequest();
            var x = credentials["client_id"].ToString();
            restRequest.AddQueryParameter("client_id", credentials["client_id"].ToString());
            restRequest.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            restRequest.AddQueryParameter("code", code);
            restRequest.AddQueryParameter("grant_type","authorization_code");
            restRequest.AddQueryParameter("redirect_uri", "https://localhost:44346/OAuth/Callback");
            
            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/oauth2/v4/token"));

            var response = restClient.Post(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(tokenFile,response.Content);
                return RedirectToAction("Index", "Home");
            }


            return View("Error");
        }


        public IActionResult RefreshToken()
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            var credentialFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\credentionals.json";
            JObject credentials = JObject.Parse(System.IO.File.ReadAllText(credentialFile));
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();
            var x = tokens["access_token"].ToString();
            restRequest.AddQueryParameter("client_id", credentials["client_id"].ToString());
            restRequest.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            // restRequest.AddQueryParameter("code", code);
            restRequest.AddQueryParameter("grant_type", "refresh_token");
            restRequest.AddQueryParameter("refresh_token", tokens["refresh_token"].ToString());



            RestClient restClient = new RestClient(new System.Uri("https://oauth2.googleapis.com/token"));

            var response = restClient.Post(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject newTokens = JObject.Parse(response.Content);
                newTokens["refresh_token"] = tokens["refresh_token"];
                System.IO.File.WriteAllText(tokenFile, newTokens.ToString());
                return RedirectToAction("Index", "Home",new { status = "success" });
            }


            return View("Error");
        }


        public IActionResult RevokeToken(string code)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();
          
            restRequest.AddQueryParameter("token", tokens["access_token"].ToString());

            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/oauth2/v4/revoke"));

            var response = restClient.Post(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home", new { status = "success" });
            }


            return View("Error");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
