using AuthorizeNet.Api.Contracts.V1;
using System;
using System.Threading.Tasks;

namespace HoleyForkingShirt
{
    public interface IPayment
    {
        string Run(string cardtype, customerAddressType details);
        public customerAddressType GetAddress(string firstname, string lastname, string address, string city, string state, string country);
    }
}