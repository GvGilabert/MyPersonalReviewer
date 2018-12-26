using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    public class MyPlacesController:Controller
    {
        private readonly IPlacesService _IPlaceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IActionResult Index(UserManager<ApplicationUser> user, IPlacesService service )
        {
            
            return View();
        }
    }
}