using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Entities.Library;
using DAL.Entities.System;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public AdminController(DataBaseContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<List<Administrator>> Get()
        {
            var admins = _context.Administrators.ToList();
            return admins;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
