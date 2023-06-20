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
                // Create the session cookie. This will also verify the ID token in the process.
                // The session cookie will have the same claims as the ID token.
                var sessionCookie = await FirebaseAuth.DefaultInstance
                    .CreateSessionCookieAsync(request.IdToken, options);

                // Set cookie policy parameters as required.
                var cookieOptions = new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.Add(options.ExpiresIn),
                    HttpOnly = true,
                    Secure = true,
                };
                this.Response.Cookies.Append("session", sessionCookie, cookieOptions);
                this.Response.Cookies.Append("journalsession", sessionCookie, cookieOptions);
                return this.Ok();
            }
            catch (FirebaseAuthException)
            {
                return this.Unauthorized("Failed to create a session cookie");
            }
        }
    }
}
