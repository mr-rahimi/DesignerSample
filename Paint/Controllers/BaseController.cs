using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Paint.Models;

namespace Paint.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult SetLanguage(string culture,string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
        [HttpGet]
        public IActionResult GetAllCulture()
        {
            var cultures = Startup.supportedCultures.Select(x => new {
                Name=x.Name,
                IsRtl=x.TextInfo.IsRightToLeft,
                nativeName= x.NativeName
            });
            return Json(cultures);
        }

    }
}
