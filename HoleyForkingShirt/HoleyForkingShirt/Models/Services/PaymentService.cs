using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Services
{
    public class PaymentService : IPayment
    {
        private IConfiguration _config;

        public PaymentService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string Run()
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = _config["AN-ApiLoginID"],
                ItemElementName = ItemChoiceType.transactionKey,
                Item = _config["AN-TransactionKey"]
            };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "1021",
                cardCode = "102"
            };

            customerAddressType billingAddress = GetAddress("someUserID");

            var paymentType = new paymentType { Item = creditCard };

            var requestType = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = 130.5m,
                payment = paymentType,
                billTo = billingAddress
            };

            var request = new createTransactionRequest { transactionRequest = requestType };

            var controller = new createTransactionController(request);

            controller.Execute();

            var response = controller.GetApiResponse();

            if(response != null)
            {
                if(response.messages.resultCode == messageTypeEnum.Ok)
                {
                    return "success!";
                }
            }
            return "fail!!";
        }

        public customerAddressType GetAddress(string userName)
        {
            customerAddressType address = new customerAddressType
            {
                address = "123 stuff street",
                city = "Seattle",
                zip = "98338",
                firstName = "bob",
                lastName = "builder"
            };

            return address;
        }

    }
}
