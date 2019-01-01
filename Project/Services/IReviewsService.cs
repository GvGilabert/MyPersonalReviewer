using System.Threading.Tasks;
using MyPersonalReviewer.Models;

namespace MyPersonalReviewer.Services
{
    public interface IReviewsService
    {
        Task<bool> AddReviewAsync(Review review, Places place, ApplicationUser user);  
    }
}