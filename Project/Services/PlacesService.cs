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
        public async Task<bool> AddMenuItemAsync(Menu newMenu, Guid place, ApplicationUser user)
        {
            newMenu.Id = Guid.NewGuid();
            newMenu.CreatorsId = user.Id;
            newMenu.PlaceId = place;

            if(string.IsNullOrEmpty(newMenu.Name))
                throw new MenuItemIncompleteDataException();

            _context.Menus.Add(newMenu);
            var result = await _context.SaveChangesAsync();
            return result==1;
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

        public async Task<bool> DeletePlaceAsync(Guid placeId, ApplicationUser user)
        {
            var placeToDelete = _context.Places.Where(p=>p.Id==placeId).SingleOrDefault(); 
            if(placeToDelete.CreatedByUserId != user.Id)
                 throw new InvalidUserException();
                
            if(placeToDelete==null) return false;

            _context.Places.Remove(placeToDelete);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }
        public async Task<Menu[]> GetMenuItemsList(Guid placeId)
        {
            return await _context.Menus.Where(m => m.PlaceId == placeId).ToArrayAsync();
        }

        public async Task<Places[]> GetPlacesList()
        {
            return await _context.Places.ToArrayAsync();
        }
        public async Task<Places[]> GetPlacesList(ApplicationUser user)
        {
            return await _context.Places.Where(p => p.CreatedByUserId == user.Id).ToArrayAsync();
        }


    }
}