using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Server.Database;
using SharedData.Authentication;

namespace Server.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthDataProvider _dataProvider;

        public AuthenticationService(IAuthDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public (bool success, AuthenticationErrorType error) Register(string login, string password)
        {
            if (_dataProvider.HasUserByLogin(login))
                return (false, AuthenticationErrorType.ExistingLogin);

            EncryptedData encrypted = new EncryptedData();
            encrypted.ProvideSaltAndHash(password);

            _dataProvider.AddUser(new UserTable
            {
                Name = login,
                Password = encrypted.Password,
                Salt = encrypted.Salt
            });

            return (true, AuthenticationErrorType.None);
        }

        public AuthenticationResponse Login(string login, string password)
        {
            UserTable? user = _dataProvider.GetUser(login);
            if (user == null || user.Password != AuthenticationHelpers.ComputeHash(password, user.Salt))
                return new AuthenticationResponse { Error = AuthenticationErrorType.InvalidLoginOrPassword };

            return new AuthenticationResponse
            {
                Error = AuthenticationErrorType.None,
                Token = GenerateJwtBearer(AssemblyClaimsIdentity(user))
            };
        }

        private string GenerateJwtBearer(ClaimsIdentity subject)
        {
            var jwt = new JwtSecurityToken(
                issuer: AuthenticationOptions.ISSUER,
                audience: AuthenticationOptions.AUDIENCE,
                claims: subject.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity AssemblyClaimsIdentity(UserTable user)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            });
        }
    }
}