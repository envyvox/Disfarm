using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Commands;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Title.Commands;
using Disfarm.Services.Game.User.Models;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.User.Commands
{
    public record CreateUserCommand(long UserId) : IRequest<UserDto>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly ILogger<CreateUserHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateUserHandler> logger,
            IMapper mapper,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await EntityFrameworkQueryableExtensions.AnyAsync(db.Users, x => x.Id == request.UserId);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already exist");
            }

            var created = await db.CreateEntity(new Data.Entities.User.User
            {
                Id = request.UserId,
                About = null,
                Level = 1,
                Xp = 0,
                Title = Data.Enums.Title.Newbie,
                Fraction = Fraction.Neutral,
                Location = Location.Neutral,
                CommandColor = EmbedBuilderExtensions.DefaultEmbedColor,
                IsPremium = false,
                Language = Language.English,
                CubeType = CubeType.D6,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            var banners = await _mediator.Send(new GetBannersQuery());
            var banner = banners.Single(x => x.Name == "NightCity");
            var currencyAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.EconomyStartupCapital));

            await _mediator.Send(new AddTitleToUserCommand(request.UserId, Data.Enums.Title.Newbie));
            await _mediator.Send(new AddBannerToUserCommand(
                request.UserId, banner.Id, null, true));
            await _mediator.Send(new AddCurrencyToUserCommand(
                request.UserId, Data.Enums.Currency.Token, currencyAmount));

            _logger.LogInformation(
                "Created user entity {@Entity}",
                created);

            return _mapper.Map<UserDto>(created);
        }
    }
}