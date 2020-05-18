using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserVm
    {
        public Guid Id { set; get; }
        [Display(Name ="Tên")]
        public string FirstName { set; get; }
        [Display(Name = "Họ")]
        public string LastName { set; get; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }
        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }
        [Display(Name = "Hòm thư")]
        public string Email { set; get; }
        [Display(Name = "Ngày sinh")]
        public DateTime Dob { set; get; }

        public IList<string> Roles { set; get; } 
    }
}
