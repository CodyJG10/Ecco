using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using static Ecco.Web.Models.AuthModel;

namespace Ecco.Web.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<EccoUser> _userManager;

        public AuthController(UserManager<EccoUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateToken([FromForm]LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var authClaims = new[]
                {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.SigningKey));

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:44355",
                    audience: "https://localhost:44355",
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                string tokenText = new JwtSecurityTokenHandler().WriteToken(token);
                var expirationText = token.ValidTo;

                return Ok(new
                {
                    token = tokenText,
                    expiration = expirationText
                });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EccoUser user = new EccoUser
            {
                UserName = model.Email,
                ProfileName = model.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            var errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return BadRequest(404);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [HttpGet("UserData")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> GetUserData(string id = null, string profileName = null) 
        {
            if (id != null)
            {
                var user = await _userManager.FindByIdAsync(id);
                return JsonConvert.SerializeObject(user);
            }
            else
            {
                var user = _userManager.Users.Single(x => x.ProfileName.Equals(profileName));
                return JsonConvert.SerializeObject(user);
            }
        }

        [HttpGet("UserExists")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public bool UserExists(string profileName)
        {
            return _userManager.Users.Any(x => x.ProfileName.Equals(profileName));
        }

        [HttpGet("testtoken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult TestToken()
        {
            return Ok("You're authorized!");
        }
    }
}