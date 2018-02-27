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
    
    public class IdentityController : Controller
    {
        // GET: api/Identity
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
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
