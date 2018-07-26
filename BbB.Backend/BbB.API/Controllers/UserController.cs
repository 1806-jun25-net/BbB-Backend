using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BbB.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly DataRepository data;
        private readonly BbBContext context;
        
        public UserController(DataRepository repository, BbBContext bbB)
        {
            data = repository;
            context = bbB;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {

            return null;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
