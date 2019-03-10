using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyPersonalReviewer.Models;
using MyPersonalReviewer.Services;
using Microsoft.Extensions.Configuration;

namespace MyPersonalReviewer.Controllers
{
    public class MyPlacesController:Controller
    {
        private readonly IPlacesService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

    public MyPlacesController(IPlacesService placesService, UserManager<ApplicationUser> userManager,IConfiguration config)
    {
        _service = placesService;
        _userManager = userManager;
        _config = config;
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

        public IActionResult AddNewPlaces()
        {
            return View();
        }

        public async Task<IActionResult> AddPlaceAction(Places place)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            place.CreatedByUserId = currentUser.Id;
            var success = await _service.AddPlaceAsync(place,currentUser);
            
            if(!success)
                return BadRequest("Could not add item");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddressPartial(string address)
        {
            if(address==null)
                return BadRequest("Address field is empty");
            
            GeolocatorApiReplyModel model = new GeolocatorApiReplyModel();
            MapApiService map = new MapApiService(_config);
            model.Info = await map.GeolocationServiceAsync(address);
            return PartialView (model);
        } 
    }
}