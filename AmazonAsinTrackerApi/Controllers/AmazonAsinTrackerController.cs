using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AmazonAsinTracker.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmazonAsinTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmazonAsinTrackerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AmazonAsinTrackerController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> TrackAsin(IEnumerable<string> asinCodes)
        {
            await _mediator.Send(new TrackProductsByAsinCodeCommand
            {
                ProductAsins = asinCodes
            });

            return Accepted();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetLastReviews(string asinCode)
        {
            return Ok(await _mediator.Send(new GetLastProductReviewByAsinCodeQuery(asinCode)));
        }

    }
}