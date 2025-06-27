using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;

namespace Altura.BidManagement.Application.Features.Bids.Queries.GetBid;

public class GetBidsQuery : IQuery<Response<List<BidDto>>>;