using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Services.Identity.Interfaces
{
    public interface IUserService
    {
        List<ApplicationUser> UserList();
        List<string> UserRoleList(string id);
        List<UserViewModel> UsersToViewModels(List<ApplicationUser> users);
        Task<ApplicationUser> FindUserAsync(string id);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
    }
}
