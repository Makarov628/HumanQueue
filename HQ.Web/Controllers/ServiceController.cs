using MediatR;
using Microsoft.AspNetCore.Mvc;


using HQ.UseCases.Service.Commands.Common.Create;
using HQ.UseCases.Service.Commands.Common.CreateChildService;
using HQ.UseCases.Service.Commands.Common.UpdateName;
using HQ.UseCases.Service.Commands.Common.UpdateLiteral;
using HQ.UseCases.Service.Commands.Common.Delete;

using HQ.UseCases.Service.Commands.WindowLink.AttachWindow;
using HQ.UseCases.Service.Commands.WindowLink.DeattachWindow;

using HQ.UseCases.Service.Queries.GetRequest;
using HQ.UseCases.Service.Commands.Request.ChangeStatusWaiting;

using ErrorOr;

namespace HQ.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IMediator _mediatr;

    public ServiceController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost]
    public async Task<ActionResult> CreateNew([FromBody] CreateServiceCommand createServiceCommand)
    {
        ErrorOr<Created> result = await _mediatr.Send(createServiceCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("child")]
    public async Task<ActionResult> CreateNewChildService([FromBody] CreateChildServiceCommand createChildServiceCommand)
    {
        ErrorOr<Created> result = await _mediatr.Send(createChildServiceCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPut("update-name")]
    public async Task<ActionResult> UpdateName([FromBody] UpdateServiceNameCommand updateServiceNameCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(updateServiceNameCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            success => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPut("update-literal")]
    public async Task<ActionResult> UpdateLiteral([FromBody] UpdateServiceLiteralCommand updateServiceLiteralCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(updateServiceLiteralCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            success => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        ErrorOr<Deleted> result = await _mediatr.Send(new DeleteServiceCommand(id), HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            deleted => NoContent(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("attach-window")]
    public async Task<ActionResult> AttachWindow([FromBody] AttachWindowCommand attachWindowCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(attachWindowCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("deattach-window")]
    public async Task<ActionResult> DeattachWindow([FromBody] DeattachWindowCommand deattachWindowCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(deattachWindowCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    // Для использования в QR 
    [HttpGet("request/{id}")]
    public async Task<ActionResult<RequestResponse>> GetRequestAndChangeToWaitingStatus(Guid id) 
    {
        await _mediatr.Send(new ChangeRequestStatusWaitingCommand(id), HttpContext.RequestAborted);

        ErrorOr<RequestResponse> request = await _mediatr.Send(new GetRequestQuery(id), HttpContext.RequestAborted);
        if (request.IsError)
            return NotFound();

        return Ok(request.Value);
    }
}