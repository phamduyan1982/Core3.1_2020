using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public Guid Id { set; get; }
        public List<SelectItem> Roles { set; get; } = new List<SelectItem>();
    }
}
