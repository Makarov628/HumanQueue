// using HQ.Application.Services.CurrentUser;
// using MediatR;

// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace HQ.Web.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize(Roles = "Audit")]
//     public class AuditController : ControllerBase
//     {
//         private readonly IMediator _mediatr;
//         private readonly CurrentUserService _currentUser;
//         public AuditController(IMediator mediatr, CurrentUserService currentUser)
//         {
//             _mediatr = mediatr; 
//             _currentUser = currentUser;
//         }

//         [HttpGet("list")]
//         public async Task<IActionResult> GetList()
//         {
//             return Ok(new { });
//         }

//         [HttpGet("get/{id}")]
//         public async Task<IActionResult> Get(int id)
//         {
//             return Ok(new { id = id });
//         }
//     }
// }
