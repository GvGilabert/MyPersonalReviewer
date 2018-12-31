using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPersonalReviewer.Controllers;
using MyPersonalReviewer.Models;

namespace MyPersonalReviewer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Places> Places { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
