using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1
//DataAnnotations error messages are localized with IStringLocalizer<T>. 
//Using the option ResourcesPath = "Resources", the error messages in 
//RegisterViewModel can be stored in either of the following paths:
//Resources/ViewModels.Account.RegisterViewModel.fr.resx
//Resources/ViewModels/Account/RegisterViewModel.fr.resx

namespace ASPNETCoreWebApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The First Name field is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        [Display(Name = "Email")]
        [Remote(action: "VerifyEmail", controller: "Account")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
