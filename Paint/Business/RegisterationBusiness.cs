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
        public RegisterationBusiness()
        {
            throw new NotImplementedException();
        }

        public async Task<string> FreeRegister(string phoneNumber, string tarhNumber, string nationalCode, string mobileNumber, string firstName, string lastName, string chanelRegister, string iPUser, int marketId = 1)
        {
            throw new NotImplementedException();

        }

        public async Task<string> Register(string reserveId, string payStatus, string refId, bool prePaid)
        {
            throw new NotImplementedException();

        }

        public async Task<string> Reserve(string phoneNumber, string tarhNumber, string nationalCode, string mobileNumber, string firstName, string familyName, string chanelRegister, string iPUser, int marketId = 1)
        {
            throw new NotImplementedException();

        }
    }
}
