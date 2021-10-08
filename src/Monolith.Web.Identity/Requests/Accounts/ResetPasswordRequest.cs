namespace Monolith.Web.Identity.Requests.Accounts
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
