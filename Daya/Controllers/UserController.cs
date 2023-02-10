using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Interface;
using UserService.Model;
using UserService.Model.Dto;
using Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Daya.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo ur;
        private readonly JwtTokenCreator tokenCreator;
        public UserController(IUserRepo ur, JwtTokenCreator tokenCreator)
        {
            this.ur = ur;
            this.tokenCreator = tokenCreator;
        }

        [HttpGet , Authorize(Policy = "JustActive")]
        public IActionResult GetAll()
        {
            List<User> all = ur.GetAll();

            if (all is null)
                return StatusCode(404, new StandardResult() { Success = true, Message = "there is no user signed up yet" });

            return StatusCode(200, new StandardResult<List<User>>() { Success = true, Message = "All users returned .", Result = all });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = ur.GetById(id);
            if (user is null)
                return StatusCode(404, new StandardResult() { Success = false, Message = "User not found ." });

            return StatusCode(200, new StandardResult<User>() { Success = true, Message = "User founded .", Result = user });
        }

        [HttpPost]
        public IActionResult Create(AddUserDto user)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new StandardResult { Success = false, Message = "model not valid" });

            (bool IsDone, Guid id) = ur.Insert(user);

            if (IsDone)
                return StatusCode(200, new StandardResult<User> { Success = true, Message = "User created.", Result = ur.GetById(id) });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPut]
        public IActionResult Update(UpdateUserDto user)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new StandardResult { Success = false, Message = "model not valid" });

            if (ur.Update(user))
                return StatusCode(200, new StandardResult<User> { Success = true, Message = "User updated .", Result = ur.GetById(user.Id) });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (ur.Delete(id))
                return StatusCode(200, new StandardResult { Success = true, Message = "User deleted." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPatch("{id}")]
        public IActionResult ActiveUser(Guid id)
        {
            var res = ur.SetActivity(id, true);
            if (res)
                return StatusCode(200, new StandardResult { Success = true, Message = "User Activated." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPatch,Authorize]
        public IActionResult ChangePassword(Guid id, string password)
        {
            var res = ur.ChangePassword(id, password);

            if (res)
                return StatusCode(200, new StandardResult { Success = true, Message = "password changed." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPatch("{id}")]
        public IActionResult DeactiveUser(Guid id)
        {
            var res = ur.SetActivity(id, false);
            if (res)
                return StatusCode(200, new StandardResult { Success = true, Message = "User Deactivated." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = ur.LoginCheck(login.UserName, login.Password);
            if (user is not null)
            {
                // create token and save it in cookie

                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, user.Id.ToString()),
                    new ("IsActive" , user.IsActive? "true" : "false")
                };

                var token = tokenCreator.CreateAccessToken(claims);

                if (!user.IsActive)
                    return StatusCode(401, new StandardResult<string> { Success = false, Message = "your account is not Active." , Result = token});

                return StatusCode(200, new StandardResult<string> { Success = true, Message = "you are Loged in", Result = token });
            }

            return StatusCode(401, new StandardResult { Success = true, Message = "login Failed." });
        }

    }
}
