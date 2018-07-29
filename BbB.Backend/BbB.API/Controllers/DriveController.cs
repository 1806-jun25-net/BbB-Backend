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
    public class DriveController : Controller
    {
        private readonly DataRepository data;

        public DriveController(DataRepository repository)
        {
            data = repository;
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Library.Drive>> Get(int id)
        //{
        //    data.getdr
        //}

        [HttpGet("{company}")]
        public async Task<ActionResult<List<Library.Drive>>> Get(string company)
        {
            return Ok(await data.GetDrives(company));
        }

    }
}
