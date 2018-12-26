using System;
using System.Threading.Tasks;
using Project.Controllers;
using Project.Services;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
         public static ApplicationUser CreateFakeUsers(int id)
        {
            return new ApplicationUser
            {
                Id = "fake-"+id,
                UserName = "fake"+id+"@example.com"
            };
        }

        public ApplicationUser user;

        [Fact]
        public void Test1()
        {

        }
    }
}
