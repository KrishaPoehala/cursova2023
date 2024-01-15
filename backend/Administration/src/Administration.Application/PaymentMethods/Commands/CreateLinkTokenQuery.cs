using Administration.Application.Common.Exceptions;
using Administration.Application.Common.Interfaces;
using Going.Plaid;
using Going.Plaid.Entity;
using Going.Plaid.Link;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Administration.Application.PaymentMethods.Commands;
public class CreateLinkTokenQuery: IRequest<LinkTokenCreateResponse>
{
}

public class CreateLinkTokenQueryHandler : IRequestHandler<CreateLinkTokenQuery, LinkTokenCreateResponse>
{
    private readonly ICurrentUserService _userService;
    private readonly PlaidClient _plaidClient;
    private readonly IConfiguration _configuration;

    public CreateLinkTokenQueryHandler(ICurrentUserService userService, PlaidClient plaidClient, IConfiguration configuration)
    {
        _userService = userService;
        _plaidClient = plaidClient;
        _configuration = configuration;
    }

    public async Task<LinkTokenCreateResponse> Handle(CreateLinkTokenQuery request, CancellationToken cancellationToken)
    {
        var b = _configuration.GetSection("Plaid:Secret").Value;
        var t = _configuration.GetSection("Plaid:ClientId").Value;
        var resut = await _plaidClient.LinkTokenCreateAsync(
            new LinkTokenCreateRequest()
            {
                Secret = _configuration.GetSection("Plaid:Secret").Value,
                ClientId = _configuration.GetSection("Plaid:ClientId").Value,
                User = new LinkTokenCreateRequestUser { ClientUserId = _userService.Companyid.ToString() },
                ClientName = "Billimo",
                Products = new Products[] { Products.Auth, Products.Transfer, Products.Transactions },
                Language = Language.English,
                CountryCodes = new CountryCode[] { CountryCode.Us, CountryCode.Ca},
                Webhook = _configuration.GetSection("Plaid:WebhookUri").Value
            });

        return resut;
    }
}
