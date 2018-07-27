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
    public class DestinationController : ControllerBase
    {
        private readonly DataRepository data;

        public DestinationController(DataRepository repository)
        {
            data = repository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Library.Destination>>> Get()
        {
            return Ok(await data.GetDestinations());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Library.Destination>> Get(int id)
        {
            return Ok(await data.GetDestinationById(id));
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Library.Destination>> Get(string name)
        {
            return Ok(await data.GetDestinationByName(name));
        }
    }
}