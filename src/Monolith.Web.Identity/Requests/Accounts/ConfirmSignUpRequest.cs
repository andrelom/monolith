namespace Monolith.Web.Identity.Requests.Accounts
{
    public class ConfirmSignUpRequest
    {
        public string Token { get; set; }

        public string Username { get; set; }
    }
}
