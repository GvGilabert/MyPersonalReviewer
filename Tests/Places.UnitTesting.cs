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

        [Fact]
        public async void AddNewPlaceWithAllFieldsShouldAddItCorrectly()
        {
            var options = CreateDbContext();
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var user = CreateFakeUsers(0);
                await service.AddPlaceAsync(CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id ),user);
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
                    await service.AddPlaceAsync(CreateFakePlace("","FakeAddress",Categories.Bar,user.Id ),user);
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
                    await service.AddPlaceAsync(CreateFakePlace("FakeUserA","",Categories.Bar,user.Id ),user);
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
                    await service.AddPlaceAsync(CreateFakePlace("FakeUserA","",Categories.Undefined,user.Id ),user);
                }
                throw new Exception();
            }
            catch (PlaceIncompleteDataException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    Assert.Empty(await context.Places.ToArrayAsync());
                    context.Database.EnsureDeleted();
                }
            }
        }

        [Fact]
        public async void DeletePlaceWithCorrectUserShouldDeleteCorrectly()
        {
            var options = CreateDbContext();
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var user = CreateFakeUsers(0);
                await service.AddPlaceAsync(CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id),user);
                var addedPlace = await context.Places.FirstOrDefaultAsync();
                await service.DeletePlaceAsync(addedPlace,user);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                Assert.Empty(await context.Places.ToArrayAsync());
                context.Database.EnsureDeleted();
            }
            
        }

        [Fact]
        public async void DeletePlaceWithIncorrectUserShouldFail()
        {
            var options = CreateDbContext();
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    var user = CreateFakeUsers(5);
                    var iUser = CreateFakeUsers(6);
                    await service.AddPlaceAsync(CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id),user);
                    var addedPlace = await context.Places.FirstAsync(); 
                    await service.DeletePlaceAsync(addedPlace,iUser);
                }
                throw new Exception();
            }
            catch (InvalidUserException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    Assert.NotEmpty(await context.Places.ToArrayAsync());
                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
