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
    [ApiController]
    public class MessageController : Controller
    {
        private readonly DataRepository data;
        
        public MessageController(DataRepository repository)
        {
            data = repository;
        }

        /// <summary>
        /// Finds the message with given id.
        /// <para> Returns OK if found, NOTFOUND if not</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> Get(int id)
        {
            var msg = await data.GetMsg(id);
            if (msg == null)
                return NotFound();
            else
                return Ok(msg);
        }

        /// <summary>
        /// Finds all messages to the user with given id.
        /// <para>Returns OK if user exists, NOTFOUND if not</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/sent")] // *messages that the user sent to someone?
        public async Task<ActionResult<List<Message>>> GetTo(int id)
        {
           var msgs = await data.GetMsgTo(id);
            if (msgs == null)
                return NotFound();
            else
                return Ok(msgs);
        }

        /// <summary>
        /// Finds all messages from the user with given id. 
        /// <para>Returns OK if user exists, NOTFOUND if not</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/received")] // *messages received from someone? 
        public async Task<ActionResult<List<Message>>> GetFrom(int id)
        {
            var msgs = await data.GetMsgFrom(id);
            if (msgs == null)
                return NotFound();
            else
                return Ok(msgs);
        }

        /// <summary>
        /// Posts a message with given users from/to, and message content, at current Time.
        /// <para>Returns NoContent if found, NotFound if not</para> 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("{from}:{to}:{content}")]
        public async Task<IActionResult> Post(int from, int to, string content)
        {
            if(await data.GetUser(from)!= null && await data.GetUser(to) != null)
            {
                await data.AddMessage(from, to, content);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
