using System;
using Microsoft.AspNetCore.Authorization;

namespace ProjPermManage.Models.Permission
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {

        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

    }
}
