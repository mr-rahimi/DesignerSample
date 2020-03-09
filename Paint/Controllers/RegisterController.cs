using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using Paint.Contracts;
using Paint.Models;
using Paint.Models.Register;
using static ServiceReference1.ServiceSoapClient;

namespace Paint.Controllers
{
    public class RegisterController : Controller
    {
        private IRegisterationBusiness _business;
        private dynamic[] services;
        public RegisterController(IRegisterationBusiness business)
        {
            _business = business;
            services =new dynamic[]{
                new { Id = "1660", Speed = "128 kb/s", Price = 0, Duration=1 },
                new { Id = "1661", Speed = "512 kb/s", Price = 54500, Duration=5 },
                new { Id = "1662", Speed = "2 mb/s", Price = 218000, Duration=5 },
                new { Id = "1663", Speed = "5 mb/s", Price = 436000, Duration=5 }
            };
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllServices()
        {
            
            return Json(services);
        }
        [HttpPost]
        public async Task<IActionResult> ReceiveForm(RegisterFormModel model)
        {
            var result = "success";
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ServiceId != "1660")
                    {
                        var orderID = await _business.Reserve("TelNumber", model.ServiceId, model.NationalCode, model.Mobile, model.FirstName, model.LastName, "ChanelRegister", "IPUser");
                        HttpContext.Session.SetString("OrderID", orderID);
                        var price = services.Where(x => x.Id == model.ServiceId).FirstOrDefault().Price;
                        BypassCertificateError();
                        string TerminalID = "1111";
                        string UserName = "2121";
                        string Password = "0000";
                        var payment = new ServiceReference2.PaymentGatewayClient();
                        result = payment.bpPayRequestAsync(
                            Int64.Parse(TerminalID),
                            UserName,
                           Password,
                            long.Parse(orderID),
                            price,
                            GetDate(),
                            GetTime(),
                            "خرید طرح وای فای مخابرات",
                            "/Register/AfterPaid",
                           Int64.Parse("0")
                        );
                    }
                    else
                        await _business.FreeRegister(model.Mobile, model.ServiceId, model.NationalCode, model.Mobile, model.FirstName, model.LastName, "ChanelRegister", "IPUser");
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest("Internalserver Error");
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AfterPaid(RegisterFormModel model)
        {
            try
            {
                ViewBag.Bank = "ملت";
                MellatReturn();
                return View();
            }
            catch (Exception)
            {
                return BadRequest("Internalserver Error");
            }
            return Ok(model);
        }
        private void MellatReturn()
        {

            BypassCertificateError();

            //if (string.IsNullOrEmpty(Request.Form["SaleReferenceId"]))
            //{
            //    //ResCode=StatusPayment
            //    if (!string.IsNullOrEmpty(Request.Params["ResCode"]))
            //    {
            //        ViewBag.Message = PaymentResult.MellatResult(Request.Params["ResCode"]);
            //        ViewBag.SaleReferenceId = "**************";

            //    }
            //    else
            //    {
            //        ViewBag.Message = "رسید قابل قبول نیست";
            //        ViewBag.SaleReferenceId = "**************";

            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        string TerminalId = "1224";
            //        string UserName = "3569";
            //        string UserPassword = "5465";



            //        long SaleOrderId = 0;  //PaymentID 
            //        long SaleReferenceId = 0;
            //        string RefId = null;

            //        try
            //        {
            //            SaleOrderId = long.Parse(Request.Params["SaleOrderId"].TrimEnd());
            //            SaleReferenceId = long.Parse(Request.Params["SaleReferenceId"].TrimEnd());
            //            RefId = Request.Params["RefId"].TrimEnd();
            //        }
            //        catch (Exception ex)
            //        {
            //            ViewBag.message = ex + "<br/>" + " وضعیت:مشکلی در پرداخت بوجود آمده ، در صورتی که وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد ";
            //            ViewBag.SaleReferenceId = "**************";
            //            return;
            //        }

            //        var bpService = new ir.shaparak.bpm.PaymentGatewayImplService();

            //        string Result;
            //        Result = bpService.bpVerifyRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

            //        if (!string.IsNullOrEmpty(Result))
            //        {
            //            if (Result == "0")
            //            {
            //                string IQresult;
            //                IQresult = bpService.bpInquiryRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

            //                if (IQresult == "0")
            //                {

            //                    long paymentID = Convert.ToInt64(SaleOrderId);


            //                    UpdatePayment(paymentID, Result, SaleReferenceId, RefId, true);

            //                    ViewBag.Message = "پرداخت با موفقیت انجام شد.";
            //                    ViewBag.SaleReferenceId = SaleReferenceId;


            //                    // پرداخت نهایی
            //                    string Sresult;

            //                    // تایید پرداخت
            //                    Sresult = bpService.bpSettleRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

            //                    if (Sresult != null)
            //                    {
            //                        if (Sresult == "0" || Sresult == "45")
            //                        {
            //                            //تراکنش تایید و ستل شده است 
            //                        }
            //                        else
            //                        {
            //                            //تراکنش تایید شده ولی ستل نشده است
            //                        }
            //                    }
            //                }
            //                else
            //                {

            //                    string Rvresult;
            //                    //عملیات برگشت دادن مبلغ
            //                    Rvresult = bpService.bpReversalRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

            //                    ViewBag.Message = "تراکنش بازگشت داده شد";
            //                    ViewBag.SaleReferenceId = "**************";



            //                    long paymentId = Convert.ToInt64(SaleOrderId);

            //                    UpdatePayment(paymentId, IQresult, SaleReferenceId, RefId, false);
            //                }
            //            }
            //            else
            //            {
            //                ViewBag.Message = PaymentResult.MellatResult(Result);
            //                ViewBag.SaleReferenceId = "**************";

            //                long paymentId = Convert.ToInt64(SaleOrderId);


            //                UpdatePayment(paymentId, Result, SaleReferenceId, RefId, false);
            //            }
            //        }
            //        else
            //        {
            //            ViewBag.Message = "شماره رسید قابل قبول نیست";
            //            ViewBag.SaleReferenceId = "**************";
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        string errors = ex.Message;
            //        ViewBag.Message = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
            //        ViewBag.SaleReferenceId = "**************";

            //    }
            //}
        }
        void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }
        private string GetDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
        }
        private string GetTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

    }
}
