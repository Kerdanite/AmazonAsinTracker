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
        public async Task<ActionResult> TrackAsin(string asinCode)
        {
            await _mediator.Send(new TrackProductsByAsinCodeCommand
            {
                ProductAsins = new List<string>(){asinCode}
            });

            return Accepted();
        }

    }
}