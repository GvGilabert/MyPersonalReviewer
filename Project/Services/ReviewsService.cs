using System.Threading.Tasks;
using MyPersonalReviewer.Data;
using MyPersonalReviewer.Models;
using MyPersonalReviewer.Services;

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
    }
}