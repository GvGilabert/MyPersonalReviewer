using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyPersonalReviewer.Controllers;
using MyPersonalReviewer.Data;
using MyPersonalReviewer.Models;
using Microsoft.EntityFrameworkCore;

namespace MyPersonalReviewer.Services
{
    public class PlacesService:IPlacesService
    {
        private readonly ApplicationDbContext _context;
        public PlacesService(ApplicationDbContext context)
        {_context = context;}

        public async Task<bool> AddPlaceAsync(Places newPlace, ApplicationUser user)
        {
            newPlace.Id = Guid.NewGuid();
            newPlace.CreatedByUserId = user.Id;
            
            if(string.IsNullOrEmpty(newPlace.Address)
            || string.IsNullOrEmpty(newPlace.Name)
            || (int)newPlace.Category == 0)
                throw new PlaceIncompleteDataException();

            _context.Places.Add(newPlace);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }
    }
}