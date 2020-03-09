using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Paint.Models;
using Paint.Models.Home;

namespace Paint.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index(string LinkOrig, string LinkLogin,string IP,string ChapId,string ChapChallenge,string Error)
        {
            ViewBag.LinkLogin = LinkLogin;
            ViewBag.LinkOrig = LinkOrig;
            ViewBag.IP = IP;
            ViewBag.ChapId = ChapId;
            ViewBag.ChapChallenge = ChapChallenge;
            ViewBag.Error = Error;
            return View();
        }
        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        public IActionResult LoginF()
        {
            var _linkLogin = HttpContext.Request.Form["link-login"];
            var _linkOrig = HttpContext.Request.Form["link-orig"];
            var _IP = HttpContext.Request.Form["IP"];
            var _chapId = HttpContext.Request.Form["chap-id"];
            var _chapChallenge = HttpContext.Request.Form["chap-challenge"];
            var _error = HttpContext.Request.Form["error"];
            return RedirectToAction("Index", new
            {
                LinkLogin = _linkLogin,
                LinkOrig = _linkOrig,
                IP = _IP,
                ChapId = _chapId,
                ChapChallenge = _chapChallenge,
                Error = _error,
            });
        }
        [HttpPost]
        public IActionResult LoginPost(LoginModel model)
        {
            try
            {
                var code = HttpContext.Session.GetString("LoginCaptcha");
                if (code == null || code != model.CaptchaCode)
                {
                    ModelState.AddModelError("captcha", "");
                    HttpContext.Session.SetString("LoginCaptcha", null);
                }

                if (ModelState.IsValid)
                {
                    //login code
                    var values = new Dictionary<string, string>
                    {
                       { "username", model.UserName },
                       { "password", model.Password },
                       { "dst", model.LinkOrig }
                    };

                    var content = new FormUrlEncodedContent(values);
                    var client = _httpClientFactory.CreateClient("SomeCustomAPI");
                    var task = Task.Run<HttpResponseMessage>(async () => await client.PostAsync(model.LinkLogin, content));
                    if (task.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return Ok();
                    }
                    else
                        throw new Exception("Failed");
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("InternalServerError", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public IActionResult GetCaptchaImage()
        {
            var code = CaptchaImage.RandomString(4);
            var x = new CaptchaImage(code, 250, 100);
            HttpContext.Session.SetString("LoginCaptcha", code);
            var y = ImageToByte2(x.Image);
            return File(y, "image/png");
        }
        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
