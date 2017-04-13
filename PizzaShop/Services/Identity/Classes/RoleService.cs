using PizzaShop.Services.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace PizzaShop.Services.Identity.Classes
{
    public class RoleService : IRoleService
    {
        readonly RoleStore<IdentityRole> _roleStore;

        public RoleService(RoleStore<IdentityRole> roleStore)
        {
            _roleStore = roleStore;
        }

        public async Task CreateRoleAsync(string name)
        {
            var identityRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name
            };
            await _roleStore.CreateAsync(identityRole);
        }

        public async Task DeleteRoleAcync(IdentityRole role)
        {
            await _roleStore.DeleteAsync(role);
        }

        public async Task<IdentityRole> FindByIdAsync(string id)
        {
            var result = await _roleStore.FindByIdAsync(id);
            return result;
        }

        public List<IdentityRole> RoleList()
        {
            var result = _roleStore.Roles.ToList();
            return result;
        }
    }
}