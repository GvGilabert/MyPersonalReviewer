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
    public class PlacesUnitTesting
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

         public static Places CreateFakeItem(string fakeName, string fakeAddress,Categories category,string userId)
        {
            return new Places
            {
                Name = fakeName,
                Address = fakeAddress,
                Category = category,
                CreatedByUserId = userId
            };
            
        }
        public ApplicationUser user;

        [Fact]
        public async void AddNewPlaceWithAllFieldsShouldAddItCorrectly()
        {
            var options = CreateDbContext();
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var user = CreateFakeUsers(0);
                await service.AddPlaceAsync(CreateFakeItem("FakeName","FakeAddress",Categories.Bar,user.Id ),user);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                Assert.NotNull(await context.Places.FirstAsync());
                context.Database.EnsureDeleted();
            }
            
        }

        [Fact]
        public async void AddNewPlaceWithEmptyNameShouldFail()
        {
            var options = CreateDbContext();
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    var user = CreateFakeUsers(1);
                    await service.AddPlaceAsync(CreateFakeItem("","FakeAddress",Categories.Bar,user.Id ),user);
                }
                throw new Exception();
            }
            catch (PlaceIncompleteDataException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    Assert.Empty(await context.Places.ToArrayAsync());
                }
            }
        }
    
        [Fact]
        public async void AddNewPlaceWithEmptyAddressShouldFail()
        {
            var options = CreateDbContext();
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    var user = CreateFakeUsers(2);
                    await service.AddPlaceAsync(CreateFakeItem("FakeUserA","",Categories.Bar,user.Id ),user);
                }
                throw new Exception();
            }
            catch (PlaceIncompleteDataException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    Assert.Empty(await context.Places.ToArrayAsync());
                }
            }
        }
        [Fact]
        public async void AddNewPlaceWithUndefinedCategoryShouldFail()
        {
            var options = CreateDbContext();
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    var user = CreateFakeUsers(3);
                    await service.AddPlaceAsync(CreateFakeItem("FakeUserA","",Categories.Undefined,user.Id ),user);
                }
                throw new Exception();
            }
            catch (PlaceIncompleteDataException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    Assert.Empty(await context.Places.ToArrayAsync());
                }
            }
        }
    }
}
