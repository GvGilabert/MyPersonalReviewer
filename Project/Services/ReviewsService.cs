using System.Threading.Tasks;
using MyPersonalReviewer.Data;
using MyPersonalReviewer.Models;
using MyPersonalReviewer.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class ReviewsService:IReviewsService
    {
        private ApplicationDbContext _context;

        public ReviewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddReviewAsync(Review review, Places places, ApplicationUser user)
        {
            if(string.IsNullOrEmpty(review.Title)
            || string.IsNullOrEmpty(review.ReviewText))
                throw new ReviewIncompleteException();
                
            review.PlaceId = places.Id;
            review.CreatedByUserId = user.Id;

            _context.Reviews.Add(review);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<int> CalculateAverageAsync(Places places)
        {
            var result = await  _context.Reviews
                        .Where(x => x.PlaceId == places.Id)
                        .Select(p => p.Points).ToArrayAsync();
            
            return (result.Length==0) ? 0 : (int)result.Average();
        }

        public async Task<Review[]> GetAllReviewsFromAsync(Places place)
        {
            return await _context.Reviews.Where(r => r.PlaceId == place.Id).ToArrayAsync();
        }
    }
}