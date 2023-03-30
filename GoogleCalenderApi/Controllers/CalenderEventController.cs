using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
//using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util.Store;
using GoogleCalenderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GoogleCalenderApi.Controllers
{
    public class CalenderEventController : Controller
    {

        public IActionResult CreateEvent(Event calenderEvent)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            var KeyFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\Key.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));
            JObject Key = JObject.Parse(System.IO.File.ReadAllText(KeyFile));
            RestRequest restRequest = new RestRequest();

            calenderEvent.Start.DateTime = DateTime.Parse(calenderEvent.Start.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffk");
            calenderEvent.End.DateTime = DateTime.Parse(calenderEvent.End.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffk");
            var model = JsonConvert.SerializeObject(calenderEvent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            restRequest.AddQueryParameter("Key", Key["Key"].ToString());
            restRequest.AddHeader("Authorization", $"Bearer" + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", model, ParameterType.RequestBody);
            RestClient restClient = new RestClient(new Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events"));


            var response = restClient.Post(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("AllEvents");
            }


            return View("Error");

        }

        public IActionResult GetEvent(string id)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("Key", "AIzaSyC32KAsV52qmfWJkGLLM60i4gxTOm2rm9I");
            restRequest.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");

            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events"+id));


            var response = restClient.Get(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calenderEvents = JObject.Parse(response.Content);
                return View(calenderEvents.ToObject<Event>());
            }


            return View("Error");

        }

        public IActionResult AllEvents()
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();
            
            restRequest.AddQueryParameter("Key", "AIzaSyC32KAsV52qmfWJkGLLM60i4gxTOm2rm9I");
            restRequest.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");
            
            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events"));


            var response = restClient.Get(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calenderEvents = JObject.Parse(response.Content);
                var allEvent = calenderEvents["items"].ToObject<IEnumerable<Event>>();


                return View(allEvent);
            }


            return View("Error");

        }

        [HttpGet]
        public IActionResult UpdateEvent(string id)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();
           

            restRequest.AddQueryParameter("Key", "AIzaSyC32KAsV52qmfWJkGLLM60i4gxTOm2rm9I");
            restRequest.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");
         
            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events"+id));


            var response = restClient.Get(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calenderEvent = JObject.Parse(response.Content);
                return View(calenderEvent.ToObject<Event>());
           
            }


            return View("Error");

        }
        [HttpPost]
        public IActionResult UpdateEvent(string id,Event calenderEvent)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();
            calenderEvent.Start.DateTime = DateTime.Parse(calenderEvent.Start.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffk");
            calenderEvent.End.DateTime = DateTime.Parse(calenderEvent.End.DateTime).ToString("yyyy-mm-dd'T'HH:mm:ss.fffk");
            var model = JsonConvert.SerializeObject(calenderEvent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            restRequest.AddQueryParameter("Key", "AIzaSyC32KAsV52qmfWJkGLLM60i4gxTOm2rm9I");
            restRequest.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", model, ParameterType.RequestBody);

            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events" + id));
            var response = restClient.Patch(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("AllEvent", "CalenderEvent", new { status = "updated" });
            }


            return View("Error");

        }
        public IActionResult DeleteEvent(string id)
        {
            var tokenFile = "E:\\Task2\\GoogleCalenderApi\\GoogleCalenderApi\\Files\\tokens.json";
            JObject tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("Key", "AIzaSyC32KAsV52qmfWJkGLLM60i4gxTOm2rm9I");
            restRequest.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");

            RestClient restClient = new RestClient(new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events" + id));


            var response = restClient.Delete(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View("Index");
            }


            return View("Error");

        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
