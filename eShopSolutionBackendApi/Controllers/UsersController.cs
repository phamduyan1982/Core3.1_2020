﻿using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolutionBackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            var resultToken = await _userService.Authenticate(request);
            if(string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("UserName and Password is incorrect");
            }            
            return Ok(resultToken);
        }
        //
        [HttpPost]  //("register")
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
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
       // http://localhost/api/users/paging?pagingIndex=1&pageSize=10
        [HttpGet("paging")]  // alias  public_paging/
        public async Task<IActionResult> GetAllPaging([FromQuery]GetUserPagingRequest request)
        {
            var product = await _userService.GetUsersPaging(request);
            return Ok(product);
        }
        //
    }
}