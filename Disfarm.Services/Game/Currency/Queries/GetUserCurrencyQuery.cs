using System;
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
    public record GetUserCurrencyQuery(long UserId, Data.Enums.Currency Type) : IRequest<UserCurrencyDto>;

    public class GetUserCurrencyHandler : IRequestHandler<GetUserCurrencyQuery, UserCurrencyDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserCurrencyHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public async Task<UserCurrencyDto> Handle(GetUserCurrencyQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserCurrencies
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            return entity is null
                ? new UserCurrencyDto(Guid.Empty, request.Type, 0, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                : _mapper.Map<UserCurrencyDto>(entity);
        }
    }
}