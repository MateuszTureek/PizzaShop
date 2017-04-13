using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models;
using PizzaShop.Services.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PizzaShop.Services.Identity.Classes
{
    public class UserService : IUserService
    {
        readonly ApplicationUserManager _userManager;
        readonly IMapper _mapper;

        public UserService(IMapper mappper)
        {
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _mapper = mappper;
        }

        public List<ApplicationUser> UserList()
        {
            var users = _userManager.Users.ToList();
            return users;
        }

        public List<string> UserRoleList(string id)
        {
            var roles = _userManager.GetRolesAsync(id).Result.ToList();
            return roles;
        }

        public List<UserViewModel> UsersToViewModels(List<ApplicationUser> users)
        {
            IList<string> roles;
            var viewModelList = _mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);
            for (var i = 0; i < users.Count; ++i)
            {
                roles = UserRoleList(users[i].Id);
                viewModelList[i].Roles = roles as List<string>;
            }
            return viewModelList;
        }

        public Task<ApplicationUser> FindUserAsync(string id)
        {
            var result = _userManager.FindByIdAsync(id);
            return result;
        }

        public Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            var result = _userManager.DeleteAsync(user);
            return result;
        }
    }
}