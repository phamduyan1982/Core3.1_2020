﻿using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolutionBackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromForm]LoginRequest request)
        {
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var resultToken = await _userService.Authenticate(request);
            if(string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("UserName and Password is incorrect");
            }
            return Ok(new { token = resultToken});
        }
        //
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]RegisterRequest request)
        {
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register is unsuccessful ");
            }
            return Ok();
        }
        //
    }
}