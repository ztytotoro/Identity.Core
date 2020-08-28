using Database.Models;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityCoreUser> _manager;
        private readonly SignInManager<IdentityCoreUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<IdentityCoreUser> manager,
            SignInManager<IdentityCoreUser> signInManager,
            IConfiguration configuration
            )
        {
            _manager = manager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<IdentityCoreUser> Get()
        {
            return _manager.Users.ToArray();
        }

        [HttpPost("register")]
        public async Task<IdentityResult> Register(RegisterDto data)
        {
            IdentityResult result = await _manager.CreateAsync(new IdentityCoreUser { UserName = data.UserName, EmailConfirmed = true }, data.Password);

            return result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterDto data)
        {
            IdentityCoreUser user = await _manager.FindByNameAsync(data.UserName);

            if (user != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync
                            (user, data.Password, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    return Unauthorized();
                }

                List<Claim> claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, data.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: _configuration["Token:Issuer"],
                    audience: _configuration["Token:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                            SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return NotFound();
        }
    }
}
