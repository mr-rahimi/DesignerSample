using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Paint.ActionFilters
{
    public class DefaultCultureActionFilter : ResultFilterAttribute
    {
        public DefaultCultureActionFilter()
        {
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var c = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key.Equals(CookieRequestCultureProvider.DefaultCookieName));
            if (c.Key == null)
            {
                context.HttpContext.Response.Cookies.Append(
                 CookieRequestCultureProvider.DefaultCookieName,
                 CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("fa-IR")),
                 new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                 );
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
            }
            base.OnResultExecuting(context);
        }
    }
}
