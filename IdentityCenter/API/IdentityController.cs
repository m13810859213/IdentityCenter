using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCenter.API
{
    [Produces("application/json")]
    [Route("api/Identity")]
    //[Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class IdentityController : Controller
    {
        // GET: api/Identity
        [HttpGet]
        public IActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToArray();
            return Ok(new { message = "Hello API", claims });
        }

        // GET: api/Identity/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Identity
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Identity/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
