using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyPersonalReviewer.Models;

namespace MyPersonalReviewer.Controllers
{
    public interface IPlacesService
    {
    Task<bool> AddPlaceAsync(Places newPlace,ApplicationUser user);
    Task<bool> DeletePlaceAsync(Guid PlaceToDelete, ApplicationUser user);
    Task<Places[]> GetPlacesList();
    Task<Places[]> GetPlacesList(ApplicationUser user);
    Task<Menu[]> GetMenuItemsList(Guid placeID);
    Task<bool> AddMenuItemAsync(Menu menu, Guid place, ApplicationUser user);

    }
}