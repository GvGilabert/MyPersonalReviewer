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
    [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
                if(currentUser == null) return Challenge();
            
            var modelData = await _service.GetPlacesList(currentUser);
            
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

        [ValidateAntiForgeryToken]
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
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlaceAction(Guid placeId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            
            var success = await _service.DeletePlaceAsync(placeId,currentUser);
            
            if(!success)
                return BadRequest("Could not delete item");
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

        public async Task<IActionResult> MenuPartial(Guid placeId)
        {
            //var currentUser = await _userManager.GetUserAsync(User);
            //if(currentUser == null) return Challenge();
            var modelData = await _service.GetMenuItemsList(placeId);
            
            MenuItemsViewModel model = new MenuItemsViewModel()
            {
                menuItems = modelData
            }; 
           
            return PartialView (model);
        }

        public async Task<IActionResult> AddMenuItemAction(Guid placeId, Menu menu)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            var success = await _service.AddMenuItemAsync(menu,placeId,currentUser);    
            if(!success)
                return BadRequest("Could not add item");
            return RedirectToAction("Index");
        } 
    }
}