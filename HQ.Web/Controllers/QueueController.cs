

using MediatR;
using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using HQ.UseCases.Queue.Queries.GetQueues;
using HQ.UseCases.Queue.Queries.GetQueue;

using HQ.UseCases.Queue.Commands.Create;
using HQ.UseCases.Queue.Commands.Delete;
using HQ.UseCases.Queue.Commands.ResetQueue;
using HQ.UseCases.Queue.Commands.UpdateName;

namespace HQ.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueueController : ControllerBase
{
    private readonly IMediator _mediatr;

    public QueueController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<ActionResult<List<QueuesResponse>>> GetAll()
    {
     ErrorOr<List<QueuesResponse>> queue = await _mediatr.Send(new GetQueuesQuery(), HttpContext.RequestAborted);
        return queue.MatchFirst<ActionResult<List<QueuesResponse>>>(
            queue => Ok(queue),
            error => NotFound()
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QueueResponse>> Get(Guid id)
    {
        ErrorOr<QueueResponse> queue = await _mediatr.Send(new GetQueueQuery(id), HttpContext.RequestAborted);
        return queue.MatchFirst<ActionResult<QueueResponse>>(
            queue => Ok(queue),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateNew([FromBody] CreateQueueCommand createQueueCommand)
    {
        ErrorOr<Created> result = await _mediatr.Send(createQueueCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            created => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPut]
    public async Task<ActionResult> UpdateName([FromBody] UpdateQueueNameCommand updateQueueNameCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(updateQueueNameCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            success => Ok(),
            error => NotFound()
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        ErrorOr<Deleted> result = await _mediatr.Send(new DeleteQueueCommand(id), HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            deleted => NoContent(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }

    [HttpPost("reset")]
    public async Task<ActionResult> Reset([FromBody] ResetQueueCommand resetQueueCommand)
    {
        ErrorOr<Success> result = await _mediatr.Send(resetQueueCommand, HttpContext.RequestAborted);
        return result.MatchFirst<ActionResult>(
            success => Ok(),
            error => Problem(detail: error.Description, statusCode: 400)
        );
    }


}