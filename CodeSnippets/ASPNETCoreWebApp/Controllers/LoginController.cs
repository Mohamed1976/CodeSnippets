using ASPNETCoreWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

//https://www.c-sharpcorner.com/article/policy-based-role-based-authorization-in-asp-net-core/
//https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-3.1
//https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/
/// <summary>
/// /
/// </summary>

//JWT is very famous in web development. It is an open standard that allows transmitting data 
//between parties as a JSON object in a secure and compact way.
namespace ASPNETCoreWebApp.Controllers
{    
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _config;

        public LoginController(ILogger<LoginController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// You can call this method using Postman:
        /// POST    https://localhost:5001/api/Login    raw, JSON
        /// Content message sent:
        /// 
        /// {
        ///     "Username": "Mohamed",
        ///     "Password":"MyPassword",
        ///     "RememberMe":true
        /// }
        /// 
        /// Content message received:
        /// 
        /// {
        ///     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb2hhbWVkLmthbG1vdWFAZ21haWwuY29tIiwianRpIjoiNmRhNjlmZjYtNTRmNy00YWIzLTk0ZTMtNTliMmRkYzBlZWU4IiwidW5pcXVlX25hbWUiOiJtb2hhbWVkLmthbG1vdWEiLCJleHAiOjE1OTEzNjY4NDYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6InVzZXJzIn0.2fPo9mqyiwu9HzJHqdXZL9isdHrcYqwsq9eDe9BpCVI",
        ///     "expiration": "2020-06-05T14:20:46Z"
        /// }
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            Debug.WriteLine("Entering Login([FromBody] LoginViewModel model)");
            Debug.WriteLine($"Username: {model.Username}, Password: {model.Password}, RememberMe: {model.RememberMe}");

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "mohamed_kalmoua"),
                new Claim(JwtRegisteredClaimNames.Email, "mohamed.kalmoua@gmail.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, "mohamed.kalmoua2020"),
                new Claim("DateOfJoing", DateTime.Now.AddYears(-5).ToString()),
                new Claim("Remark", "One of our best customers")
            };

            if(model.Username == "Mohamed")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Director"));
                claims.Add(new Claim(ClaimTypes.Role, "Manager"));
                claims.Add(new Claim(ClaimTypes.Role, "Employee"));
                claims.Add(new Claim(ClaimTypes.Role, "Guest"));
            }
            else if(model.Username == "Jan")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Director"));
            }
            else if(model.Username == "Anouk")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Manager"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Guest"));
                claims.Add(new Claim("IsAdult", "Yes"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };

            Debug.WriteLine($"Exiting Login, token: {results.token}, expiration: {results.expiration}");

            return Created("", results);

            //IActionResult response = Unauthorized();
            //var user = AuthenticateUser(login);
            //if (user != null)
            //{
            //    var tokenString = GenerateJSONWebToken(user);
            //    response = Ok(new { token = tokenString });
            //}
            //return response;
            //return Created("", null);
            //return BadRequest();
        }

        //You can use PostMan to check this method
        //GET https://localhost:5001/api/Login?name=Mohamed
        //In the Authorization tab select Type = Bearer Token
        //https://learning.postman.com/docs/postman/sending-api-requests/authorization/#bearer-token

        //Token = "KEY which you got from the Post login method from above"
        //If we call this method without a token, we will get 401 (UnAuthorizedAccess) HTTP status 
        //code as a response. If we want to bypass the authentication for any method, we can mark 
        //that method with the AllowAnonymous attribute.
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<string>> Get(string name)
        {
            Debug.WriteLine("Entering ActionResult<IEnumerable<string>> GetInfo()"); 
            List<string> info = new List<string>();

            try
            {
                var currentUser = HttpContext.User;

                //Check if current user HasClaim
                if (currentUser.HasClaim(c => c.Type == "DateOfJoing"))
                {
                    info.Add(currentUser.Claims.FirstOrDefault(c => c.Type == "DateOfJoing").Value);
                }

                if (currentUser.HasClaim(c => c.Type == "Remark"))
                {
                    info.Add(currentUser.Claims.FirstOrDefault(c => c.Type == "Remark").Value);
                }

                info.Add("Welcome: " + name);

                //Add other claims
                //info.Add(currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
                //info.Add(currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value);
                //info.Add(currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
                //info.Add(currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);
                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in ActionResult<IEnumerable<string>> Get(): {ex}");
                return BadRequest("Failed in ActionResult<IEnumerable<string>> Get()");
            }
        }

        [HttpGet]
        //Must be Employee and Manager to access this method   
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Employee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Products/{name}")]
        public ActionResult<IEnumerable<string>> Products(string name)
        {
            List<string> info = new List<string>()
            {
                name,
                "Product1",
                "Product2"
            };

            return Ok(info);
        }

        [HttpGet]
        //Must be Director or Manager  
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Director,Manager")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Orders/{name}")]
        public ActionResult<IEnumerable<string>> Orders(string name)
        {
            List<string> info = new List<string>()
            {
                name,
                "Order1",
                "Order2"
            };

            return Ok(info);
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        //UserPolicy is defined in Startup.cs
        [Authorize(Policy = "UserPolicy")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Payments/{name}/{year}")]
        public ActionResult<IEnumerable<string>> Payments(string name,int year)
        {
            List<string> info = new List<string>()
            {
                name,
                year.ToString(),
                "Payment1",
                "Payment2"
            };

            return Ok(info);
        }
    }
}
