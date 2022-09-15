using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Statistic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Statistic.Queries
{
    public record GetUserStatisticQuery(
            long UserId,
            StatisticPeriod Period,
            Data.Enums.Statistic Type)
        : IRequest<UserStatisticDto>;

    public class GetUserStatisticHandler : IRequestHandler<GetUserStatisticQuery, UserStatisticDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserStatisticHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserStatisticDto> Handle(GetUserStatisticQuery request, CancellationToken ct)
        {
            var entity = await _db.UserStatistics
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Period == request.Period &&
                    x.Type == request.Type);

            return entity is null
                ? new UserStatisticDto(Guid.Empty, request.Period, request.Type, 0, DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow)
                : _mapper.Map<UserStatisticDto>(entity);
        }
    }
}