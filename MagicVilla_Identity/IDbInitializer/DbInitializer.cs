using IdentityModel;
using MagicVilla_Identity.Data;
using MagicVilla_Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MagicVilla_Identity.IDbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer))
                    .GetAwaiter().GetResult();
            }

            else { return; }

            ApplicationUser adminUser = new()
            {
                UserName = SD.Admin,
                Email = SD.Admin,
                EmailConfirmed = true,
                PhoneNumber = "11111111",
                Name = SD.Admin,
            };

            _userManager.CreateAsync(adminUser, "Admin123*")
                .GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, SD.Admin)
                .GetAwaiter().GetResult();
            var claimsAdmin = _userManager.AddClaimsAsync(adminUser, 
                new Claim[]
            {
                new Claim (JwtClaimTypes.Name, adminUser.Name),
                new Claim (JwtClaimTypes.Role, SD.Admin)
            }).Result;

            ApplicationUser customerUser = new()
            {
                UserName = SD.Customer,
                Email = SD.Customer,
                EmailConfirmed = true,
                PhoneNumber = "2222222",
                Name = SD.Customer,
            };

            _userManager.CreateAsync(customerUser, "Customer123*")
                .GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, SD.Customer)
                .GetAwaiter().GetResult();
            var claimsCustomer = _userManager.AddClaimsAsync(customerUser,
                new Claim[]
            {
                new Claim (JwtClaimTypes.Name, customerUser.Name),
                new Claim (JwtClaimTypes.Role, SD.Customer)
            }).Result;
        }
    }
}
