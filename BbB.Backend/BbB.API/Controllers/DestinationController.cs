using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BbB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly DataRepository data;

        public DestinationController(DataRepository repository)
        {
            data = repository;
        }

        /// <summary>
        /// Returns the list of destinations available
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Library.Destination>>> Get()
        {
            return Ok(await data.GetDestinations());
        }

        /// <summary>
        /// Returns a destination based in the id provided
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Library.Destination>> Get(int id)
        {
            return Ok(await data.GetDestinationById(id));
        }

        /// <summary>
        /// Returns a destination based on the title provided.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("{title}/name")]
        public async Task<ActionResult<Library.Destination>> GetByName(string title)
        {
            return Ok(await data.GetDestinationByTitle(title));
        }

        /// <summary>
        /// Get the menu items of a destination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/menu")]
        public async Task<ActionResult<List<Library.MenuItem>>> GetMenu(int id)
        {
            var menu = await data.GetMenuItems(id);
            return Ok(menu.ToList());
        }


        [HttpPost]
        public async Task<ActionResult<Library.Destination>> NewDest(Library.Destination input)
        {
            var dest = await data.NewDestination(input);
            if (dest == null)
                return BadRequest();
            return Ok(dest);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Library.MenuItem>> AddItem(Library.MenuItem input, int id)
        {
            var item = await data.NewMenuItem(input, id);
            if (item == null)
                return BadRequest();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Library.Destination>> DeleteItem(Library.MenuItem input, int id)
        {
            var dest = await data.RemoveMenuItem(input, id);
            if (dest == null)
                return BadRequest();
            return Ok(dest);
        }
    }
}