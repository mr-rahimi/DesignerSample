using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Paint.Models;
using Paint.Models.Home;

namespace Paint.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                var code = HttpContext.Session.GetString("LoginCaptcha");
                if (code != null && code != model.CaptchaCode)
                {
                    ModelState.AddModelError("captcha", "");
                    HttpContext.Session.SetString("LoginCaptcha", null);
                }

                if (ModelState.IsValid)
                    //login code
                    return Ok(model);
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("InternalServerError", ex.Message);
                return BadRequest(ModelState);
            }
        }
        
    }
}
