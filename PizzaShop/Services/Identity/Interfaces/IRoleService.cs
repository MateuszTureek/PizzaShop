﻿using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Services.Identity.Interfaces
{
    public interface IRoleService
    {
        List<IdentityRole> RoleList();
        Task CreateRoleAsync(string name);
        Task<IdentityRole> FindByIdAsync(string id);
        Task DeleteRoleAcync(IdentityRole role);
        List<RoleViewModel> MapRoleListToViewModelList(List<IdentityRole> roleList);
    }
}
