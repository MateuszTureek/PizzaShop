using PizzaShop.Services.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;

namespace PizzaShop.Services.Identity.Classes
{
    public class RoleService : IRoleService
    {
        readonly RoleStore<IdentityRole> _roleStore;
        readonly IMapper _mapper;

        public RoleService(RoleStore<IdentityRole> roleStore, IMapper mapper)
        {
            _roleStore = roleStore;
            _mapper = mapper;
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

        public List<RoleViewModel> MapRoleListToViewModelList(List<IdentityRole> roleList)
        {
            var model = _mapper.Map<List<IdentityRole>, List<RoleViewModel>>(roleList);
            return model;
        }
    }
}