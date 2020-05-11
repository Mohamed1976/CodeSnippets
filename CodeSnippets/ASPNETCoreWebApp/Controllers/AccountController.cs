using ASPNETCoreWebApp.Models;
using ASPNETCoreWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

        public AccountController(IEmailSender emailSender, ISmsSender smsSender)
        {
            _smsSender = smsSender;
            _emailSender = emailSender;
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            Trace.WriteLine("Register(RegisterViewModel model, string returnUrl = null)");
            Trace.WriteLine($"model.Email: {model.Email}, model.Password: {model.Password}");

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                /*var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);*/
                await Task.Delay(100);
                return RedirectToAction(nameof(Index),"Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //The examples provided previously in this section involve a static set of rules 
        //that can be attributed on the model.There will be situations in which you will 
        //need to perform a more interactive validation. An example is the Register 
        //User section of an application.You want new users to know immediately if 
        //the user name they enter is available.The only way to do this is through 
        //a remote validator that posts the user name back to the server, which 
        //tells you whether that value is available. Remote validation has two parts.
        //One is the server action that evaluates validity.Typically, you create 
        //a validation-specific controller to handle all your validation.
        //The following code sample demonstrates a remote validation action.
        //The IsUserAvailable action method accepts a user name and checks to see 
        //whether it is already used. If it does not exist, the user name has passed 
        //validation. If it does exist, the method creates an alternate name and 
        //responds with that name.Note that it responds with a value and a 
        //JsonRequestBehavior.AllowGet enum, which ensures that if the user accepts 
        //the returned value, the validation will not run
        //https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1
        [AcceptVerbs("GET", "POST")]
        public JsonResult VerifyEmail(string email)
        {
            Trace.WriteLine($"Account VerifyEmail(string email), email: {email}");

            if(email.Contains("Mohamed", StringComparison.OrdinalIgnoreCase))
            {
                return Json($"Email {email} is already in use.");
            }

            //return Json($"Email {email} is already in use, try .", JsonRequestBehavior.AllowGet);
            //for (int i = 1; i < 100; i++)
            //{
            //    string altCandidate = username + i.ToString();
            //    if (!WebSecurity.UserExists(altCandidate))
            //    {
            //        suggestedUID = String.Format(CultureInfo.InvariantCulture,
            //        "{0} is not available. Try {1}.", username, altCandidate);
            //        break;
            //    }
            //}

            //if (!_userService.VerifyEmail(email))
            //{
            //return Json($"Email {email} is already in use.");
            //}

            return Json(true);
        }
    }
}
