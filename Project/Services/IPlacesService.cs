using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyPersonalReviewer.Models;

namespace MyPersonalReviewer.Controllers
{
    public interface IPlacesService
    {
    Task<bool> AddPlaceAsync(Places newPlace,ApplicationUser user);
    Task<bool> DeletePlaceAsync(Places PlaceToDelete, ApplicationUser user);
    }
}