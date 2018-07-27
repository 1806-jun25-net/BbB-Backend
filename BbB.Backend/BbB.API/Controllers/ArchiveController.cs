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
    public class ArchiveController : Controller
    {
        private readonly DataRepository data;
        
        public ArchiveController(DataRepository repository)
        {
            data = repository;
        }

    }
}
