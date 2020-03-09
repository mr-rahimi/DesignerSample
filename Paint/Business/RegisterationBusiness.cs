using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Paint.Contracts;

namespace Paint.Business
{
    public class RegisterationBusiness : IRegisterationBusiness
    {
        private ServiceSoapClient _service;
        public RegisterationBusiness()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress address = new EndpointAddress("http://ws_xdsl.tce.local/service.asmx");
            _service = new ServiceSoapClient(binding, address);
        }

        public async Task<string> FreeRegister(string phoneNumber, string tarhNumber, string nationalCode, string mobileNumber, string firstName, string lastName, string chanelRegister, string iPUser, int marketId = 1)
        {
            return await _service.create_postpaid_customer_with_marketerAsync(phoneNumber, tarhNumber, nationalCode, mobileNumber, firstName, lastName, "wifi", "37.255.176.36", 0);
            //return await Task<String>.Run(() =>
            //{
            //    return "1234";
            //});
        }

        public async Task<string> Register(string reserveId, string payStatus, string refId, bool prePaid)
        {
            return await _service.register_adsl_customer_with_marketerAsync(reserveId, payStatus, refId, prePaid);
            //return await Task<string>.Run(() =>
            //{
            //    return "1234";
            //});
        }

        public async Task<string> Reserve(string phoneNumber, string tarhNumber, string nationalCode, string mobileNumber, string firstName, string familyName, string chanelRegister, string iPUser, int marketId = 1)
        {
            return await _service.reserve_adsl_customer_with_marketerAsync(phoneNumber, tarhNumber, nationalCode, mobileNumber, firstName, familyName, chanelRegister, "37.255.176.36", marketId);
            //return await Task<string>.Run(() =>
            //{
            //    return "1234";
            //});
        }
    }
}
