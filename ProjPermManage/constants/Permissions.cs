using System;
using System.Collections.Generic;

namespace ProjPermManage.constants
{
    public class Permissions
    {
        public static List<string> GeneratePermissionForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class Products
        {
            public static string View = "Permissions.Product.View";
            public static string Create = "Permissions.Product.Create";
            public static string Edit = "Permissions.Product.Edit";
            public static string Delete = "Permissions.Product.Delete";
        }
    }
}
