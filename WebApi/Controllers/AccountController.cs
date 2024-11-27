﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok("You are not logged in.");
            }

            return Ok("Welcome to the API!");
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest();
            }

            var userProfile = new UserProfile
            {
                Id = currentUser.Id,
                Name = currentUser.UserName ?? "",
                Email = currentUser.Email ?? "",
                PhoneNumber = currentUser.PhoneNumber ?? "",
            };

            return Ok(userProfile);
        }
    }
}
