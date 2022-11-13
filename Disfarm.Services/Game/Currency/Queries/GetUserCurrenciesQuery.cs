using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Currency.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Currency.Queries
{
    public record GetUserCurrenciesQuery(long UserId) : IRequest<Dictionary<Data.Enums.Currency, UserCurrencyDto>>;

    public class GetUserCurrenciesHandler
        : IRequestHandler<GetUserCurrenciesQuery, Dictionary<Data.Enums.Currency, UserCurrencyDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserCurrenciesHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Dictionary<Data.Enums.Currency, UserCurrencyDto>> Handle(
            GetUserCurrenciesQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserCurrencies
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToDictionaryAsync(x => x.Type);

            return _mapper.Map<Dictionary<Data.Enums.Currency, UserCurrencyDto>>(entities);
        }
    }
}