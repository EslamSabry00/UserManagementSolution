using Twilio.Rest.Api.V2010.Account;
using UserManagement.DAL.Models;

namespace UserManagement.PL.Helpers
{
    public interface ISmsService
    {
        MessageResource SendSms(SmsMessage message);
    }
}
