using System;
using System.Linq;
using System.Threading.Tasks;
using MyPersonalReviewer.Controllers;
using MyPersonalReviewer.Services;
using MyPersonalReviewer.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyPersonalReviewer.Data;
using static MyPersonalReviewer.Models.Enums;

namespace Tests
{
    public class ReviewsUnitTesting
    {
        public static DbContextOptions<ApplicationDbContext> CreateDbContext ()
        {
            return new DbContextOptionsBuilder<MyPersonalReviewer.Data.ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
        }
        public static ApplicationUser CreateFakeUsers(int id)
        {
            return new ApplicationUser
            {
                Id = "fake-"+id,
                UserName = "fake"+id+"@example.com"
            };
        }

        public static Places CreateFakePlace(string fakeName, string fakeAddress,Categories category,string userId)
        {
            return new Places
            {
                Name = fakeName,
                Address = fakeAddress,
                Category = category,
                CreatedByUserId = userId
            };
            
        }

        public static Review CreateFakeReview(Places place, ApplicationUser user)
        {
            return new Review
            {
                Title = "Las mejores papas fritas de bs as",
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
                Points = 3
            };
        }
          public static Review CreateFakeReview(Places place, ApplicationUser user, string title, string review, int stars)
        {
            return new Review
            {
                Title = title,
                ReviewText = review,
                Points = stars
            };
        }
        
        [Fact]
        public async void AddNewReviewWithAllFieldsShouldAddCorrectly()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(0);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userA.Id); 
            Review review = CreateFakeReview(place,userA);
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                await service.AddReviewAsync(review,place,userA);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                Assert.NotNull(await context.Reviews.FirstAsync());
            }
        }

        [Fact]
        public async void AddNewReviewWithEmptyTitleShouldFail()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(0);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userA.Id); 
            Review review = CreateFakeReview(place,userA,"","lorem ipsum sarandanga",1);
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new ReviewsService(context);
                    await service.AddReviewAsync(review,place,userA);
                }
                throw new Exception();
            }
            catch(ReviewIncompleteException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new ReviewsService(context);
                    Assert.Empty(await context.Reviews.ToArrayAsync());
                }
            }
        }
        
        [Fact]
        public async void AddNewReviewWithEmptyReviewTextShouldFail()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(0);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userA.Id); 
            Review review = CreateFakeReview(place,userA,"A Fake Title","",1);
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new ReviewsService(context);
                    await service.AddReviewAsync(review,place,userA);
                }
                throw new Exception();
            }
            catch(ReviewIncompleteException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new ReviewsService(context);
                    Assert.Empty(await context.Reviews.ToArrayAsync());
                }
            }
        }
    }
}