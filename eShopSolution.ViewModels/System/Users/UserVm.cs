using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserVm
    {
        public Guid Id { set; get; }
        public string FirstName { set; get; }

        public string LastName { set; get; }
        public string PhoneNumber { set; get; }

        public string UserName { set; get; }

        public string Email { set; get; }
    }
}
