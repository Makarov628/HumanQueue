

using MediatR;
using Microsoft.AspNetCore.Mvc;

using HQ.UseCases.Window.Queries.GetWindow;

using HQ.UseCases.Window.Commands.Create;
using HQ.UseCases.Window.Commands.Delete;

using HQ.UseCases.Window.Commands.ChangeStatusCalled;
using HQ.UseCases.Window.Commands.ChangeStatusLosted;
using HQ.UseCases.Window.Commands.ChangeStatusWorkStarted;
using HQ.UseCases.Window.Commands.ChangeStatusWorkEnded;

using ErrorOr;

namespace HQ.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WindowController : ControllerBase
{
    private readonly IMediator _mediatr;

    public WindowController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WindowResponse>> Get(Guid id)
    {
        ErrorOr<WindowResponse> window = await _mediatr.Send(new GetWindowQuery(id), HttpContext.RequestAborted);
        return window.MatchFirst<ActionResult<WindowResponse>>(
            window => Ok(window),
            error => NotFound()
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateNew([FromBody] CreateWindowCommand createWindowCommand)
    {
        ErrorOr<Created> result = await _mediatr.Send(createWindowCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        ErrorOr<Deleted> result = await _mediatr.Send(new DeleteWindowCommand(id), HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            deleted => NoContent(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("request/status-called")]
    public async Task<ActionResult> RequestStatusCalled([FromBody] ChangeRequestStatusCalledCommand changeRequestStatusCalledCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(changeRequestStatusCalledCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("request/status-losted")]
    public async Task<ActionResult> RequestStatusLosted([FromBody] ChangeRequestStatusLostedCommand changeRequestStatusLostedCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(changeRequestStatusLostedCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("request/status-work-started")]
    public async Task<ActionResult> RequestStatusWorkStarted([FromBody] ChangeRequestStatusWorkStartedCommand changeRequestStatusWorkStartedCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(changeRequestStatusWorkStartedCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("request/status-work-ended")]
    public async Task<ActionResult> RequestStatusWorkEnded([FromBody] ChangeRequestStatusWorkEndedCommand changeRequestStatusWorkEndedCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(changeRequestStatusWorkEndedCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

}