using ASPNETCoreWebApp.Models;
using ASPNETCoreWebApp.Services;
using ASPNETCoreWebApp.ViewModels;
//Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// Usefull links:
/// 
/// https://code-maze.com/authentication-aspnet-core-identity/
/// </summary>
namespace ASPNETCoreWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AccountController(IEmailSender emailSender, 
            ISmsSender smsSender,
            ILogger<AccountController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _smsSender = smsSender;
            _emailSender = emailSender;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
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
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            Trace.WriteLine("Register(RegisterViewModel model, string returnUrl = null)");
            Trace.WriteLine($"model.Email: {registerViewModel.Email}, model.Password: {registerViewModel.Password}");

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = _mapper.Map<ApplicationUser>(registerViewModel);

                IdentityResult result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }

                    return View(registerViewModel);
                }

                //Simple add Roles and claims based on username
                if (registerViewModel.FirstName == "Mohamed")
                {
                    await _userManager.AddToRoleAsync(user, "Director");
                    await _userManager.AddToRoleAsync(user, "Manager");
                    await _userManager.AddToRoleAsync(user, "Employee");
                    await _userManager.AddToRoleAsync(user, "Guest");
                }
                else if (registerViewModel.FirstName == "Jan")
                {
                    await _userManager.AddToRoleAsync(user, "Director");
                }
                else if (registerViewModel.FirstName == "Anouk")
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Guest");
                    await _userManager.AddClaimAsync(user, new Claim("IsAdult", "Yes"));
                }

                return RedirectToAction(nameof(HomeController.Index), "Home");


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
                //await Task.Delay(100);
                //return RedirectToAction(nameof(Index),"Home");
            }

            // !ModelState.IsValid, If we got this far, something failed, redisplay form
            return View(registerViewModel);
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

            if(email.Contains("Moo", StringComparison.OrdinalIgnoreCase))
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Illustrates several ways to retrieve Get and Post parameters
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, 
            string AuthenticationMethod,
            string MyRoute,
            IFormCollection values)
        {
            if (ModelState.IsValid)
            {
                string str1 = HttpContext.Request.Query["AuthenticationMethod"];
                string str2 = HttpContext.Request.Query["MyRoute"];
                //Retrieve Post parameters, Request.Query cant be used to retrieve Post parameters 
                //string str3 = HttpContext.Request.Query["Username"];
                string str4 = HttpContext.Request.Form["Username"];

                foreach (string key in values.Keys)
                {
                    Debug.WriteLine($"Key={key}, Value={values[key]}");
                }

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username,
                    loginViewModel.Password,
                    loginViewModel.RememberMe,
                    false);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Login request failed.");

            return View();

            //if (!ModelState.IsValid)
            //{
            //    return View(loginViewModel);
            //}

            //var user = await _userManager.FindByEmailAsync(loginViewModel.Username);

            //if (user != null &&
            //    await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
            //{
            //    var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            //    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            //    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
            //        new ClaimsPrincipal(identity));

            //    return RedirectToAction(nameof(HomeController.Index), "Home");
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Invalid UserName or Password");
            //    return View();
            //}
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");  
        }


    }
}
