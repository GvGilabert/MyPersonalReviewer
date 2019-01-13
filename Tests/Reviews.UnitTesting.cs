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
            return new DbContextOptionsBuilder<MyPersonalReviewer.Data.ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem"+Guid.NewGuid()).Options;
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
                context.Database.EnsureDeleted();
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
                    context.Database.EnsureDeleted();
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
                    context.Database.EnsureDeleted();
                }
            }
        }

        [Fact]
        public async void CalculateReviewsPointsAverageFromSameUserForPlaceShouldReturn3()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(5);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userA.Id); 
            Review reviewA = CreateFakeReview(place,userA,"test","Test",2);
            Review reviewB = CreateFakeReview(place,userA,"test","Test",3);
            Review reviewC = CreateFakeReview(place,userA,"test","Test",4);
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                await service.AddReviewAsync(reviewA,place,userA);
                await service.AddReviewAsync(reviewB,place,userA);
                await service.AddReviewAsync(reviewC,place,userA);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                Assert.True(await service.CalculateAverageAsync(place)==3);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void CalculateReviewsPointsAverageFromDifferentUsersForPlaceShouldReturn3()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(1);
            var userB = CreateFakeUsers(2);
            var userC = CreateFakeUsers(3);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userC.Id); 
            Review reviewA = CreateFakeReview(place,userA,"test","Test",2);
            Review reviewB = CreateFakeReview(place,userB,"test","Test",3);
            Review reviewC = CreateFakeReview(place,userC,"test","Test",4);
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                await service.AddReviewAsync(reviewA,place,userA);
                await service.AddReviewAsync(reviewB,place,userB);
                await service.AddReviewAsync(reviewC,place,userC);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                Assert.True(await service.CalculateAverageAsync(place)==3);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void CalculateReviewsPointsAverageWithNoReviewsShouldReturn0()
        {
            
            var options = CreateDbContext();
            var userA = CreateFakeUsers(1);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userA.Id); 
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                Assert.True(await service.CalculateAverageAsync(place)==0);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void GetAllReviewsFromAPlaceShouldReturnCorrectly()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(1);
            var userB = CreateFakeUsers(2);
            var userC = CreateFakeUsers(3);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userC.Id); 
            Review reviewA = CreateFakeReview(place,userA,"test","Test",2);
            Review reviewB = CreateFakeReview(place,userB,"test","Test",3);
            Review reviewC = CreateFakeReview(place,userC,"test","Test",4);
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                await service.AddReviewAsync(reviewA,place,userA);
                await service.AddReviewAsync(reviewB,place,userB);
                await service.AddReviewAsync(reviewC,place,userC);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                Review [] revArr = await service.GetAllReviewsFromAsync(place);
                Assert.True(revArr.Length == 3);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void GetAllReviewsFromAPlaceWithNoReviewsShouldReturnEmpty()
        {
            var options = CreateDbContext();
            var userA = CreateFakeUsers(1);
            Places place =  CreateFakePlace("FakeName","FakeAddress",Categories.Bar,userA.Id);

            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewsService(context);
                Review [] revArr = await service.GetAllReviewsFromAsync(place);
                Assert.Empty(revArr);
                context.Database.EnsureDeleted();
            }
        }
    }
}