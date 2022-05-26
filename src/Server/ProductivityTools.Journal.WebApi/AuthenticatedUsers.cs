using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace ProductivityTools.Journal.WebApi
{
    public class AuthenticatedUsersAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public AuthenticatedUsersAttribute()
        {
        }
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var x1 = context.HttpContext.User;
            var identity = (ClaimsIdentity)x1.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var email = claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            if (email == "pwujczyk@gmail.com" || email=="pwujczyk@google.com" || email == "grzegorz.opara@gmail.com")
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }
}
