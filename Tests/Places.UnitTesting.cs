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
using Microsoft.Extensions.Configuration;

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

        public static Menu CreateFakeMenuItem(string fakeName, decimal fakePrice, ApplicationUser user)
        {
            return new Menu
            {
                Name = fakeName,
                Price = fakePrice,
                CreatorsId = user.Id
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
        
        [Fact]
        public async void AddNewMenuItemWithAllFieldsShouldAddItCorrectly()
        {
            var options = CreateDbContext();
            var user = CreateFakeUsers(0);
            var place = CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id);

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var menu = CreateFakeMenuItem("FakeMenuName",20.50m,user);
                await service.AddPlaceAsync(place,user);
                await service.AddMenuItemAsync(menu,place,user);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var menuItem = await context.Menus.FirstAsync();
                Assert.NotNull(menuItem);
                Assert.True(menuItem.CreatorsId == user.Id);
                Assert.True(menuItem.PlaceId == place.Id);
                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void AddNewMenuWithAnEmptyNameShouldFail()
        {
            var options = CreateDbContext();
            var user = CreateFakeUsers(0);
            var place = CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id);
            try
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    await service.AddPlaceAsync(place,user);
                    var menu = CreateFakeMenuItem("",20.50m,user);
                    await service.AddMenuItemAsync(menu,place,user);
                }
                throw new Exception();
            }
            catch (MenuItemIncompleteDataException)
            {
                using (var context = new ApplicationDbContext(options))
                {
                    var service = new PlacesService(context);
                    Assert.Empty(await context.Menus.ToArrayAsync());
                }
            }
        }
        
        [Fact]
        public async void GetAllPlacesFromSameUserShouldReturnAllCorrectly()
        {
            var options = CreateDbContext();
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var user = CreateFakeUsers(0);
                await service.AddPlaceAsync(CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id ),user);
                await service.AddPlaceAsync(CreateFakePlace("FakeNameB","FakeAddressB",Categories.Cafe,user.Id ),user);
                await service.AddPlaceAsync(CreateFakePlace("FakeNameC","FakeAddressC",Categories.Carnicería,user.Id ),user);            
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var placesArr = await service.GetPlacesList(); 
                Assert.True(placesArr.Length == 3);
                Assert.True(placesArr[0].Name == "FakeName");
                context.Database.EnsureDeleted();
            }   
        }

        [Fact]
        public async void GetAllPlacesFromDifferentUsersShouldReturnAllCorrectly()
        {
            var options = CreateDbContext();
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var user = CreateFakeUsers(0);
                var userA = CreateFakeUsers(1);
                var userB = CreateFakeUsers(2);
                await service.AddPlaceAsync(CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id ),user);
                await service.AddPlaceAsync(CreateFakePlace("FakeNameB","FakeAddressB",Categories.Cafe,userA.Id ),userA);
                await service.AddPlaceAsync(CreateFakePlace("FakeNameC","FakeAddressC",Categories.Carnicería,userB.Id ),userB);            
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var placesArr = await service.GetPlacesList(); 
                Assert.True(placesArr.Length == 3);
                Assert.True(placesArr[0].Name == "FakeName");
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetAllPlacesFromAUserShouldReturnAllCorrectly()
        {
            var options = CreateDbContext();
            var user = CreateFakeUsers(0);
            var userA = CreateFakeUsers(1);
            var userB = CreateFakeUsers(2);
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                await service.AddPlaceAsync(CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id ),user);
                await service.AddPlaceAsync(CreateFakePlace("FakeNameB","FakeAddressB",Categories.Cafe,userA.Id ),userB);
                await service.AddPlaceAsync(CreateFakePlace("FakeNameC","FakeAddressC",Categories.Carnicería,userB.Id ),userA);            
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var placesArr = await service.GetPlacesList(user); 
                Assert.True(placesArr.Length == 1);
                Assert.True(placesArr[0].Name == "FakeName");
                context.Database.EnsureDeleted();
            }   
        }

        [Fact]
        public async void GetAllMenuItemsFromAPlaceShouldReturnAllCorrectly()
        {
            var options = CreateDbContext();
            var user = CreateFakeUsers(0);
            var place = CreateFakePlace("FakeName","FakeAddress",Categories.Bar,user.Id);
            var menu = CreateFakeMenuItem("FakeMenuItem",15.55m,user);
            var menuB = CreateFakeMenuItem("FakeMenuItemB",19.55m,user);
            var menuC = CreateFakeMenuItem("FakeMenuItemC",44.55m,user);
            
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                await service.AddPlaceAsync(place,user);
                await service.AddMenuItemAsync(menu,place,user);
                await service.AddMenuItemAsync(menuB,place,user);
                await service.AddMenuItemAsync(menuC,place,user);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new PlacesService(context);
                var placesArr = await service.GetMenuItemsList(place); 
                Assert.True(placesArr.Length == 3);
                Assert.True(placesArr[0].Name == "FakeMenuItem");
                context.Database.EnsureDeleted();
            }   
        }
    }
}
