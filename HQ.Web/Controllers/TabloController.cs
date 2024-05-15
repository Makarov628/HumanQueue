

using MediatR;
using Microsoft.AspNetCore.Mvc;

using ErrorOr;
using HQ.UseCases.Service.Queries.GetRequestsForTablo;

namespace HQ.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TabloController : ControllerBase
{
    private readonly IMediator _mediatr;

    public TabloController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("{queueId}")]
    public async Task<ActionResult<TabloResponse>> Get(Guid queueId)
    {
        ErrorOr<TabloResponse> tablo = await _mediatr.Send(new GetRequestsForTabloQuery(queueId), HttpContext.RequestAborted);
        return tablo.MatchFirst<ActionResult<TabloResponse>>(
            tablo => Ok(tablo),
            error => NotFound()
        );
    }

}