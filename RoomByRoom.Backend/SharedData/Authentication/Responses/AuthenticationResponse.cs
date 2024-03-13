namespace SharedData.Authentication
{
    public class AuthenticationResponse
    {
        public AuthenticationErrorType Error;
        public string Token { get; set; }
    }
}