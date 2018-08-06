using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BbB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("{userName}/user")]
        public async Task<ActionResult<User>> GetUser(string userName)
        {
            User user = await data.GetUserByUsername(userName);
            if (user == null)
            {
                return NoContent();
            }
            return user;
        }

        [HttpGet("{userName}/driver")]
        public async Task<ActionResult<User>> GetDriver(string userName)
        {
            int id = await data.LookupUserId(userName);
            Library.Driver driver = await data.GetDriverByUserId(id);
            if (driver == null)
            {
                return await GetUser(userName);
            }
            return driver;
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
        public async Task Delete(int id)
        {
            User result = await data.GetUser(id);
            context.Remove(result);
        }

        [HttpPost("upgrade")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Upgrade(Library.Driver driver)
        {
            int id = await data.LookupUserId(driver.Name);
            var check = await data.GetDriverByUserId(id);

            if (check != null)
            {
                return StatusCode(403);
            }

            await data.AddDriver(id, driver.Seats, driver.MeetLoc);

            return NoContent();
        }

        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [AllowAnonymous]
        public async Task<ActionResult> Login(User input)
        {
            bool verify = await data.VerifyLogin(input.Name, input.Pass);

            if (!verify)
            {
                return StatusCode(403);
            }

            var result = await _signInManager.PasswordSignInAsync(input.Name, input.Pass,
                    isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return StatusCode(403);
            }

            return NoContent();
        }

        [HttpPost("loginadmin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> LoginAdmin(User input, [FromServices] RoleManager<IdentityRole> roleManager,
             [FromServices] UserManager<IdentityUser> userManager)
        {
            bool verify = await data.VerifyLogin(input.Name, input.Pass);

            if (!verify)
            {
                return StatusCode(403);
            }

            var check = await userManager.GetRolesAsync(await userManager.FindByNameAsync(input.Name));

            if (!check.Contains("admin"))
            {
                return StatusCode(403);
            }

            var result = await _signInManager.PasswordSignInAsync(input.Name, input.Pass,
                    isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return StatusCode(403);
            }

            //if ()
            //{
            //    return StatusCode(403);
            //}

            return NoContent();
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        public async Task<NoContentResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Register(User input, [FromServices] UserManager<IdentityUser> userManager)
            //[FromServices] RoleManager<IdentityRole> roleManager, bool admin = false) //keep around if we want to implement user roles
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
