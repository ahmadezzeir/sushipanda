using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserCreationDto userCreationDto)
        {
            var result = await _usersService.CreateUserAsync(userCreationDto);

            var json = new
            {
                message = string.Join(", ", result.Errors.Select(x => x.Description))
            };

            return Ok(json);
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _usersService.ForgotPassword(email);
            return Ok();
        }

        // Example for exception catch with sentry.io
        [HttpGet]
        [Route("exception")]
        public async Task<IActionResult> Put()
        {
            throw null;

            return Ok();
        }

        [HttpPost]
        [Route("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto dto)
        {
            await _usersService.ChangeEmailAsync(dto);
            return Ok();
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
