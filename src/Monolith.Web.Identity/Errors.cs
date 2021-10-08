namespace Monolith.Web.Identity
{
    public static class Errors
    {
        //
        // Common

        public const string Validation = nameof(Validation);

        //
        // Account, User

        public const string UserNotFound = nameof(UserNotFound);

        public const string TokenExpired = nameof(TokenExpired);

        //
        // Account

        public const string InvalidCredentials = nameof(InvalidCredentials);

        public const string UsernameAlreadyInUse = nameof(UsernameAlreadyInUse);

        public const string EmailAlreadyInUse = nameof(EmailAlreadyInUse);

        public const string EmailNotConfirmed = nameof(EmailNotConfirmed);
    }
}
