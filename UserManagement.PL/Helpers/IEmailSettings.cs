using DataAccessLayer.Entities;

namespace UserManagement.PL.Helpers
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
