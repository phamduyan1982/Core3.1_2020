using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public  class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
