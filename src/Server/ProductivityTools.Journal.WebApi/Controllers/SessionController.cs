using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using FirebaseAdmin.Auth;

namespace ProductivityTools.Journal.WebApi.Controllers
{
    public class LoginRequest
    {
        public string IdToken { get; set; }
    }
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            // Set session expiration to 5 days.
            var options = new SessionCookieOptions()
            {
                ExpiresIn = TimeSpan.FromDays(5),
            };

            try
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                cookieOptions.SameSite = SameSiteMode.None;
                //cookieOptions.Path = "/";
                cookieOptions.Secure = true;
                cookieOptions.HttpOnly = true;
                Response.Cookies.Append("fsdafa", "Fsad", cookieOptions);
                //Response.Cookies.Append("Bearer", request.IdToken, cookieOptions);
                Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                Response.Headers.Add("access-control-expose-headers", "Set-Cookie");
                Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000");
                return this.Ok();
            }
            catch (FirebaseAuthException)
            {
                return this.Unauthorized("Failed to create a session cookie");
            }
        }

        [HttpGet]
        [Route("LoginGet")]
        public async Task<ActionResult> LoginGet(string token)
        {
            //var bearer = Request.Headers.Authorization[0].Replace("Bearer ", "");

            // Set session expiration to 5 days.
            var options = new SessionCookieOptions()
            {
                ExpiresIn = TimeSpan.FromDays(5),
            };

            try
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                cookieOptions.SameSite = SameSiteMode.None;
                //cookieOptions.Path = "/";
                cookieOptions.Secure = true;
                cookieOptions.HttpOnly = true;
                Response.Cookies.Append("token", token, cookieOptions);
                //Response.Cookies.Append("Bearer", request.IdToken, cookieOptions);
                Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                //Response.Headers.Add("access-control-expose-headers", "Set-Cookie");
                Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000");
                return this.Ok();
            }
            catch (FirebaseAuthException)
            {
                return this.Unauthorized("Failed to create a session cookie");
            }
        }
    }
}
