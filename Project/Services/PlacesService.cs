using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyPersonalReviewer.Controllers;
using MyPersonalReviewer.Data;
using MyPersonalReviewer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyPersonalReviewer.Services
{
    public class PlacesService:IPlacesService
    {
        private readonly ApplicationDbContext _context;
        public PlacesService(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<bool> DeletePlaceAsync(Places PlaceToDelete, ApplicationUser user)
        {
            if(PlaceToDelete.CreatedByUserId != user.Id)
                throw new InvalidUserException();
                
            var place = await _context.Places.Where
            (p=> p.Id==PlaceToDelete.Id
            && p.CreatedByUserId == user.Id)
            .SingleOrDefaultAsync();

            if(place==null) return false;

            _context.Places.Remove(place);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }
    }
}