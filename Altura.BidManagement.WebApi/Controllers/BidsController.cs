using Altura.BidManagement.Application.Features.Bids.Commands.CreateBid;
using Altura.BidManagement.Application.Features.Bids.Commands.DeleteBid;
using Altura.BidManagement.Application.Features.Bids.Commands.TransitionBid;
using Altura.BidManagement.Application.Features.Bids.Commands.UpdateBid;
using Altura.BidManagement.Application.Features.Bids.Queries.GetBid;
using Altura.BidManagement.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Altura.BidManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController(IMediator mediator) : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateBid([FromBody] 
            CreateBidRequest request, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new CreateBidCommand(request.Title, request.Amount, request.State),
                cancellationToken);
            
            return result.Status ? Ok(result)  : BadRequest(result);
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetAllBids(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetBidsQuery(), cancellationToken);
            return result.Status ? Ok(result) : BadRequest(result);
        }
        
      
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{bidId:guid}")]
        public async Task<IActionResult> GetBid(Guid bidId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetBidByIdQuery(bidId), cancellationToken);
            return result.Status ? Ok(result) : BadRequest(result);
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBid(Guid id, [FromBody] UpdateBidRequest request)
        {
            var result = await mediator.Send(
                new UpdateBidCommand(id, request.Title, request.Amount, request.State), 
                CancellationToken.None);
            
            return result.Status ? Ok(result) : BadRequest(result);
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("resourceId")]
        public async Task<IActionResult> DeleteBid(Guid id)
        {
           var result = await mediator.Send(new DeleteBidCommand(id), CancellationToken.None);
           return result.Status ? Ok(result) : BadRequest(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/transition")]
        public async Task<IActionResult> TransitionBid(Guid id, 
            [FromBody] string targetState, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new TransitionBidCommand(id, targetState), 
                cancellationToken);
            
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
