using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paint.Contracts
{
    public interface IRegisterationBusiness
    {
        Task<string> Reserve(string phoneNumber, string tarhNumber, string nationalCode, string mobileNumber, string firstName, string familyName, string chanelRegister, string iPUser, int marketId = 1);
        Task<string> FreeRegister(string phoneNumber, string tarhNumber, string nationalCode, string mobileNumber, string firstName, string familyName, string chanelRegister, string iPUser, int marketId = 1);
        Task<string> Register(string reserveId, string payStatus, string refId, bool prePaid);
    }
}
