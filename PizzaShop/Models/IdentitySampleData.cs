using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace PizzaShop.Models
{
    public class IdentitySampleData : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        ApplicationDbContext _db = null;

        public IdentitySampleData()
        {
            _db = ApplicationDbContext.Create();
        }

        public string Hash(string password)
        {
            int saltSize = 16;
            int bytesRequired = 32;
            byte[] array = new byte[1 + saltSize + bytesRequired];
            int iterations = 1000; // 1000, afaik, which is the min recommended for Rfc2898DeriveBytes
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltSize, iterations))
            {
                byte[] salt = pbkdf2.Salt;
                Buffer.BlockCopy(salt, 0, array, 1, saltSize);
                byte[] bytes = pbkdf2.GetBytes(bytesRequired);
                Buffer.BlockCopy(bytes, 0, array, saltSize + 1, bytesRequired);
            }
            return Convert.ToBase64String(array);
        }

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            IdentityRole superRole = new IdentityRole("superAdmin");
            IdentityRole role = new IdentityRole("admin");
            _db.Roles.Add(superRole);
            _db.Roles.Add(role);
            ApplicationUser user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                PasswordHash = Hash("admin"),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            user.Roles.Add(new IdentityUserRole()
            {
                RoleId = superRole.Id,
                UserId = user.Id
            });
            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}