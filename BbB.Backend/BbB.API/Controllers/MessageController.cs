using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BbB.API.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly DataRepository data;
        
        public MessageController(DataRepository repository)
        {
            data = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Message>>> GetTo(int id)
        {
            return Ok(await data.GetMsgTo(id));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<Message>>> GetFrom(int id)
        {
            return Ok(await data.GetMsgFrom(id));
        }

        //[HttpGet("{id:home}")]
        [HttpPost("{from}:{to}:{content}")]
        public async Task<IActionResult> Post(int from, int to, string content)
        {
            return null;
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
