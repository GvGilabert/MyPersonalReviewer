using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPersonalReviewer.Models;
using MyPersonalReviewer.Services;
using static MyPersonalReviewer.Models.GeolocatorApiReplyModel;

namespace MyPersonalReviewer.Controllers
{
    public class HomeController : Controller
    {
        readonly IConfiguration _config;
        readonly IPlacesService _service;
        public HomeController(IConfiguration config, IPlacesService service)
        {
            _config = config;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var modelData = await _service.GetPlacesList();
            
            MyPlacesViewModel model = new MyPlacesViewModel()
            {
                Places = modelData
            };
            ViewData["ApiKey"] = _config.GetSection("GeoLocationApi").GetSection("ApiKey").Value;
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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

        //temporal
        public async Task<IActionResult> SearchAddress(string address)
        {
        MapApiService mapSer = new MapApiService(_config);
        var apiReply = await mapSer.GeolocationServiceAsync(address);
        return View(apiReply);
         
        }
    }
}
