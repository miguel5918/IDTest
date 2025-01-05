using API.Models;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace API.Service
{
    public class TwilioService
    {
        public static async Task<MessageResource> SendMessageAsync(TwilioCredentials credentials, string toPhoneNumber, string messageBody)
        {
            // CConfigure  Twilio credentials
            TwilioClient.Init(credentials.Accountid, credentials.AuthToken);

            var message = await MessageResource.CreateAsync(
                to: new PhoneNumber(toPhoneNumber),
                from: new PhoneNumber(credentials.FromNumber),
                body: messageBody
            );

            return message; //SID SEND.
        }
    }
}

