﻿using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]Credentials credentials)
        {
            var user = new IdentityUser { UserName = credentials.Email, Email = credentials.Email };

            var result = await _userManager.CreateAsync(user, credentials.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }


            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(CreateToken(user));

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]Credentials credentials)
        {
            var result = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(credentials.Email);

            return Ok(CreateToken(user));
        }

        string CreateToken(IdentityUser user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            };


            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));

            var signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);

            return  new JwtSecurityTokenHandler().WriteToken(jwt);

        }
    }
}
