using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkJunior.Mod;
using System.Web;

namespace WorkJunior.Controls
{
    [Route("api/[controller]")]
    public class GetSession : Controller
    {
        private DBookingTickets repo;

        public GetSession(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var res = HttpContext.Request.Cookies["user"];

            if (res != null)
            {
                var user = JsonConvert.DeserializeObject<User>(res);
                return Ok(user);
            }
            return BadRequest("NoSession");
        }
    }

    [Route("api/[controller]")]
    public class SetCookie : Controller
    {
        private DBookingTickets repo;
        public SetCookie(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Post(UserAuthorization query)
        {
            var res = repo.User.FirstOrDefault(a => a.Mail == query.Mail && a.Password == query.Password);
            if (res == null)
                return BadRequest("Wrong password or email");

            CookieBuilder f = new CookieBuilder();

            f.HttpOnly = false;
            f.SameSite = SameSiteMode.None;
            f.SecurePolicy = CookieSecurePolicy.None;
            f.Expiration = TimeSpan.FromDays(20);

            HttpContext.Response.Cookies.Append("user", JsonConvert.SerializeObject(res), f.Build(this.HttpContext));

            return Ok(res);
        }
    }

    [Route("api/[controller]")]
    public class DeleteCookie : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            HttpContext.Response.Cookies.Delete("user");          

            return Ok();
        }
    }
}
