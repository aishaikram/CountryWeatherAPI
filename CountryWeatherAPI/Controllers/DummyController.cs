using CountryWeatherAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWeatherAPI.Controllers
{
    [ApiController]
    public class DummyController: Controller
    {
        readonly CityInfoContext _ctx;
       
        public DummyController(CityInfoContext context)
        {
            _ctx = context;
        }
        [HttpGet]
        [Route("api/TestDatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }

    }
}
