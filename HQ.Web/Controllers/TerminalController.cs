

using MediatR;
using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using HQ.UseCases.Terminal.Queries.GetTerminal;
using HQ.UseCases.Terminal.Queries.GetExternalPrinters;

using HQ.UseCases.Terminal.Commands.Create;
using HQ.UseCases.Terminal.Commands.Update;
using HQ.UseCases.Terminal.Commands.Delete;
using HQ.UseCases.Terminal.Commands.CreateRequest;
using HQ.UseCases.Terminal.Commands.PrintRequest;

using HQ.Application.Printer;

namespace HQ.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TerminalController : ControllerBase
{
    private readonly IMediator _mediatr;

    public TerminalController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TerminalResponse>> Get(Guid id)
    {
        ErrorOr<TerminalResponse> terminal = await _mediatr.Send(new GetTerminalQuery(id), HttpContext.RequestAborted);
        return terminal.MatchFirst<ActionResult<TerminalResponse>>(
            terminal => Ok(terminal),
            error => NotFound()
        );
    }

    [HttpGet("external-printers")]
    public async Task<ActionResult<TerminalResponse>> GetExternalPrinters()
    {
        ErrorOr<List<Printer>> printers = await _mediatr.Send(new GetExternalPrintersQuery(), HttpContext.RequestAborted);
        return printers.MatchFirst<ActionResult<TerminalResponse>>(
            printers => Ok(printers),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateNew([FromBody] CreateTerminalCommand createTerminalCommand)
    {
        ErrorOr<Created> result = await _mediatr.Send(createTerminalCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateTerminalCommand updateTerminalCommand)
    {
        ErrorOr<Updated> result = await _mediatr.Send(updateTerminalCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            updated => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        ErrorOr<Deleted> result = await _mediatr.Send(new DeleteTerminalCommand(id), HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            deleted => NoContent(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("create-request")]
    public async Task<ActionResult<TerminalRequestResponse>> CreateRequest([FromBody] TerminalCreateRequestCommand createRequestCommand)
    {
        ErrorOr<TerminalRequestResponse> request = await _mediatr.Send(createRequestCommand, HttpContext.RequestAborted);
        return request.MatchFirst<ActionResult<TerminalRequestResponse>>(
            request => Ok(request),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("print-request")]
    public async Task<ActionResult> PrintRequest([FromBody] TerminalPrintRequestCommand printRequestCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(printRequestCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            success => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }


}