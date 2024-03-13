using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SharedData.Authentication;

namespace Server.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationSvc;
        private readonly ICheckingService _checker;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationSvc,
            ICheckingService checker,
            ILogger<AuthenticationController> logger)
        {
            _authenticationSvc = authenticationSvc;
            _checker = checker;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register(AuthenticationRequest request)
        {
            if (!_checker.Check(request.Login, request.Password))
                return BadRequest(new AuthenticationResponse());
            
            (bool success, AuthenticationErrorType error) =
                _authenticationSvc.Register(request.Login, request.Password);
            if (!success)
                return BadRequest(new AuthenticationResponse { Error = error });

            _logger.LogInformation("{Login} user is registered", request.Login);
            return Login(request);
        }

        [HttpPost("login")]
        public IActionResult Login(AuthenticationRequest request)
        {
            AuthenticationResponse response = _authenticationSvc.Login(request.Login, request.Password);
            if (response.Error != AuthenticationErrorType.None)
                return BadRequest(response);

            _logger.LogInformation("{Login} user is logged in", request.Login);
            return Ok(response);
        }

        [HttpDelete("logout")]
        public IActionResult Logout()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest();
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.NameIdentifier));
            return Ok();
        }
    }
}