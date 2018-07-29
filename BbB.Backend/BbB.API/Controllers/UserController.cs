﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BbB.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataRepository data;
        private readonly BbBContext context;
        private SignInManager<IdentityUser> _signInManager { get; }

        public UserController(DataRepository repository, BbBContext bbB, SignInManager<IdentityUser> signInManager)
        {
            data = repository;
            context = bbB;
            _signInManager = signInManager;
        }

        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            return data.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User lookup = await data.GetUser(id);
            if (lookup == null)
            {
                return NoContent();
            }
            return lookup;
        }

        [HttpPost]
        public async void Post(string name, string email, string pass, string company)
        {
            await data.AddUser(name, email, pass, company);
        }

        [HttpPut("{id}")]
        public void Put(int id, string pass, string company)
        {
            Task<User> result = data.GetUser(id);
            if (pass != null)
            {
                result.Result.Pass = pass;
            }

            if (company != null)
            {
                result.Result.Company = company;
            }

            context.Update(result);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Task<User> result = data.GetUser(id);
            context.Remove(result);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> Login(User input)//, [FromServices] UserManager<IdentityUser> userManager)
        {
            bool verify = await data.VerifyLogin(input.Name, input.Pass);
            var user = new IdentityUser(input.Name);

            //var result = await userManager.CreateAsync(user, input.Pass);

            //if (!result.Succeeded)
            //{
            //    return BadRequest(result);
            //}

            if (verify)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            else
            {
                return StatusCode(403);
            }

            //if (!signIn.Succeeded)
            //{
            //    return StatusCode(403); // Forbidden
            //}

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<NoContentResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Register(User input, [FromServices] UserManager<IdentityUser> userManager)
            //[FromServices] RoleManager<IdentityRole> roleManager, bool admin = false) keep around if we want to implement user roles
        {
            // with an [ApiController], model state is always automatically checked
            // and return 400 if any errors.

            var user = new IdentityUser(input.Name);

            var result = await userManager.CreateAsync(user, input.Pass);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            //if (admin)
            //{
            //    if (!(await roleManager.RoleExistsAsync("admin")))
            //    {
            //        var adminRole = new IdentityRole("admin");
            //        result = await roleManager.CreateAsync(adminRole);
            //        if (!result.Succeeded)
            //        {
            //            return StatusCode(500, result);
            //        }
            //    }
            //    result = await userManager.AddToRoleAsync(user, "admin");
            //    if (!result.Succeeded)
            //    {
            //        return StatusCode(500, result);
            //    }
            //}

            await data.AddUser(input.Name, input.Email, input.Pass, input.Company);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return NoContent();
        }
    }
}
