using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.Helpers
{
    public class SmsService : ISmsService
    {
        private readonly TwilioSettings _options;

        public SmsService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }

        public MessageResource SendSms(SmsMessage message)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);
            var result = MessageResource.Create(
                body: message.Body, 
                from: new Twilio.Types.PhoneNumber(_options.TwilioPhone), 
                to: message.PhoneNumber
                );
            return result;
        }
    }
}
