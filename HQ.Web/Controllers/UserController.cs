// using HQ.Application.Common.Interfaces;
// using HQ.Application.Services.CurrentUser;
// using HQ.UseCases.Auth.Dtos;
// using HQ.UseCases.Auth.Queries.GetUser;
// using HQ.UseCases.Auth.Queries.Login;
// using HQ.Web.Dtos.User;

// using ErrorOr;

// using MediatR;

// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;

// namespace HQ.Web.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class UserController : ControllerBase
//     {
//         private readonly IMediator _mediatr;
//         private readonly CurrentUserService _currentUser;

//         public UserController(IMediator mediatr, CurrentUserService currentUser)
//         {
//             _mediatr = mediatr;
//             _currentUser = currentUser;
//         }

//         [HttpPost("login")]
//         [AllowAnonymous]
//         public async Task<IActionResult> Login([FromBody] AuthDto dto)
//         {
//             User user = await _mediatr.Send(new GetUserQuery()
//             {
//                 Login = dto.Login,
//                 Password = dto.Password
//             });

//             if (user == null)
//             {
//                 return Unauthorized();
//             }

//             ErrorOr<string> accessToken = await _mediatr.Send(new LoginQuery()
//             {
//                 UserName = user.Username,
//                 Role = user.Role
//             });

//             return accessToken.Match(
//                 accessToken => Ok(
//                                     new
//                                     {
//                                         AccessToken = accessToken,
//                                         RefreshToken = string.Empty
//                                     }
//                                 ),
//                 errors => Problem(
//                     statusCode: StatusCodes.Status401Unauthorized,
//                     title: errors.FirstOrDefault().Description)
//             );
//         }

//         [Authorize]
//         [HttpGet("logout")]
//         public async Task<IActionResult> Logout()
//         {
//             return Ok();
//         }
//     }
// }