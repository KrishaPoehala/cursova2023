using AutoMapper;
using AutoMapper.QueryableExtensions;
using Invoicing.Application.Common.Interfaces;
using Invoicing.Application.Invoicing.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Application.Bills.Queries.RetreiveBill;
public class RetreiveBillQuery: IRequest<BillPaymentDto>
{
    public long BillId { get; set; }
}

public class RetreiveBillQueryHandler : IRequestHandler<RetreiveBillQuery, BillPaymentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RetreiveBillQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BillPaymentDto> Handle(RetreiveBillQuery request, CancellationToken cancellationToken)
    {
        var bill = await _context.Bills
            .Include(x => x.Invoice)
            .Where(x => x.Id == request.BillId)
            .ProjectTo<BillPaymentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return bill;
    }
}
