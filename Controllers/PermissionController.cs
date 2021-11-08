using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjPermManage.constants;
using ProjPermManage.Helpers;
using ProjPermManage.Models;
using ProjPermManage.Seeds;

namespace ProjPermManage.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PermissionController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;

        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);
            var role = await roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var Claim = await roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = Claim.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }


        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            var claims = await roleManager.GetClaimsAsync(role);

            foreach (var claim in claims)
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();

            foreach (var claim in selectedClaims)
            {
                await roleManager.AddAPermissionClaim(role, claim.Value);
            }

            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}