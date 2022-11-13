using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Cooldown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Cooldown.Queries
{
    public record GetUserCooldownQuery(long UserId, Data.Enums.Cooldown Type) : IRequest<UserCooldownDto>;

    public class GetUserCooldownHandler : IRequestHandler<GetUserCooldownQuery, UserCooldownDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserCooldownHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<UserCooldownDto> Handle(GetUserCooldownQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserCooldowns
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            return entity is null
                ? new UserCooldownDto(request.UserId, request.Type, DateTimeOffset.UtcNow)
                : _mapper.Map<UserCooldownDto>(entity);
        }
    }
}