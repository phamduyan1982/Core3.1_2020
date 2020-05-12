using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserUpdateRequest
    {
        public Guid Id { set; get; }
        [Display(Name = "Tên")]
        public string FirstName { set; get; }
        [Display(Name = "Họ")]
        public string LastName { set; get; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { set; get; }
        [Display(Name = "Hòm thư")]
        public string Email { set; get; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }
       

    }
}
