using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Base;

namespace RyazanSpace.DAL.Entities.Credentials
{
    public class ResetPasswordSession : Entity
    {
        public User Owner { get; set; }

        public int VerificationCode { get; set; }

        public DateTimeOffset DateExpire { get; set; } = DateTimeOffset.Now.AddMinutes(5);
    }
}
