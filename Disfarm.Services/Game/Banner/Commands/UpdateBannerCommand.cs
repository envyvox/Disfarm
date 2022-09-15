using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Banner.Commands
{
    public record UpdateBannerCommand(BannerDto UpdatedBanner) : IRequest;

    public class UpdateBannerHandler : IRequestHandler<UpdateBannerCommand>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateBannerHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateBannerHandler(
            DbContextOptions options,
            IMapper mapper,
            ILogger<UpdateBannerHandler> logger)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
        {
            var updated = await _db.UpdateEntity(_mapper.Map<UserBanner>(request.UpdatedBanner with
            {
                UpdatedAt = DateTimeOffset.UtcNow
            }));

            _logger.LogInformation(
                "Updated banner entity {@Entity}",
                updated);

            return Unit.Value;
        }
    }
}