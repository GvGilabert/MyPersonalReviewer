using System.Threading.Tasks;
using MyPersonalReviewer.Models;

namespace MyPersonalReviewer.Services
{
    public interface IReviewsService
    {
        Task<bool> AddReviewAsync(Review review, Places place, ApplicationUser user);  
        Task<int> CalculateAverageAsync(Places places);
        Task<Review[]> GetAllReviewsFromAsync (Places place);
    }
}