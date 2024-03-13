using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Database;
using SharedData.Profile;

namespace Server.Profile
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileDataProvider _provider;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IProfileDataProvider provider, ILogger<ProfileController> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        [HttpGet("progress")]
        public IActionResult GetProgress()
        {
            Claim user = GetUserClaim();
            ProfileTable profile = _provider.GetProfile(user.Value);
            _logger.LogInformation("Progress of the user with {User} name was received", profile.User.Name);
            return Ok(new ProgressResponse { Progress = profile.Progress ?? "" });
        }

        [HttpPut("progress")]
        public IActionResult UpdateProgress(ProgressRequest request)
        {
            ProfileTable profile = GetUserProfile();
            profile.Progress = request.Progress;
            _provider.UpdateProfile(profile);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteProfile()
        {
            Claim user = GetUserClaim();
            _provider.DeleteUser(user.Value);
            
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest();
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.NameIdentifier));
            return Ok();
        }

        private ProfileTable GetUserProfile()
        {
            Claim user = GetUserClaim();
            return _provider.GetProfile(user.Value);
        }

        private Claim GetUserClaim()
        {
            Claim user = User.FindFirst(ClaimTypes.Name) ?? throw new ArgumentNullException();
            _logger.LogInformation("{Claim}", user.Value);
            return user;
        }
    }
}