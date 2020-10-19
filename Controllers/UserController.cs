//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_cdgen.Models;

namespace net_cdgen.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult<User> GetAll()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public ActionResult<User> Get(string id)
        {
            return Ok();
        }
        //[AllowAnonymous]
        [HttpPost("{id}")]
        public ActionResult<User> Create([FromBody] User user)
        {
            return Ok();
        }
        [HttpPut("{id}")]
        public ActionResult<User> Update(int id,[FromBody] User user)
        {
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult<User> Update(int id)
        {
            return Ok();
        }
    }
}