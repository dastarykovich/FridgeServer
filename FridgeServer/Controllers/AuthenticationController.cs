using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Filters.ActionFilters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace FridgeServer.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;

        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILoggerManager loggerManager, IMapper mapper,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IAuthenticationManager authenticationManager)
        {
            _loggerManager = loggerManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody]
            UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            var notExistRoles = new StringBuilder();
            foreach (var role in userForRegistration.Roles!)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    notExistRoles.Append(role + ", ");
                }
            }

            if (notExistRoles.Length != 0)
            {
                notExistRoles.Remove(notExistRoles.Length - 2, 2);
                return BadRequest($"The {notExistRoles} roles is not in the database.");
            }

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

            await _authenticationManager.ValidateUser(_mapper.Map<UserForAuthenticationDto>(user));

            return Ok(await _authenticationManager.CreateToken());
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationManager.ValidateUser(user))
            {
                _loggerManager.LogWarn($"{nameof(Authenticate)}: Authentication failed." +
                    $" Wrong user name or password.");
                return Unauthorized();
            }

            return Ok(await _authenticationManager.CreateToken());
        }
    }
}
