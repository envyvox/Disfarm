using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Game.Seed.Commands;
using MediatR;

namespace Disfarm.Services.Seeder
{
    public record SeedSeedsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeedSeedsHandler : IRequestHandler<SeedSeedsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeedSeedsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedSeedsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateSeedCommand[]
            {
                new("GreenBeanSeeds", Season.Spring, TimeSpan.FromDays(3), TimeSpan.FromDays(2), false, 60),
                new("PotatoSeeds", Season.Spring, TimeSpan.FromDays(3), null, true, 50),
                new("StrawberrySeeds", Season.Spring, TimeSpan.FromDays(3), TimeSpan.FromDays(2), true, 30),
                new("KaleSeeds", Season.Spring, TimeSpan.FromDays(3), null, false, 70),
                new("ParsnipSeeds", Season.Spring, TimeSpan.FromDays(3), null, false, 20),
                new("RhubarbSeeds", Season.Spring, TimeSpan.FromDays(4), null, false, 100),
                new("CauliflowerSeeds", Season.Spring, TimeSpan.FromDays(3), null, false, 80),
                new("GarlicSeeds", Season.Spring, TimeSpan.FromDays(3), null, false, 40),
                new("MelonSeeds", Season.Summer, TimeSpan.FromDays(4), null, false, 80),
                new("HotPepperSeeds", Season.Summer, TimeSpan.FromDays(3), TimeSpan.FromDays(2), true, 40),
                new("RedCabbageSeeds", Season.Summer, TimeSpan.FromDays(4), null, false, 100),
                new("CornSeeds", Season.Summer, TimeSpan.FromDays(4), TimeSpan.FromDays(2), false, 150),
                new("TomatoSeeds", Season.Summer, TimeSpan.FromDays(3), TimeSpan.FromDays(2), true, 50),
                new("WheatSeeds", Season.Summer, TimeSpan.FromDays(3), null, false, 10),
                new("RadishSeeds", Season.Summer, TimeSpan.FromDays(4), null, false, 40),
                new("HopsSeeds", Season.Summer, TimeSpan.FromDays(3), TimeSpan.FromDays(1), false, 60),
                new("BlueberrySeeds", Season.Summer, TimeSpan.FromDays(4), TimeSpan.FromDays(2), true, 80),
                new("AmaranthSeeds", Season.Autumn, TimeSpan.FromDays(3), null, false, 70),
                new("ArtichokeSeeds", Season.Autumn, TimeSpan.FromDays(3), null, false, 30),
                new("EggplantSeeds", Season.Autumn, TimeSpan.FromDays(2), TimeSpan.FromDays(3), false, 20),
                new("YamSeeds", Season.Autumn, TimeSpan.FromDays(3), null, true, 60),
                new("BokChoySeeds", Season.Autumn, TimeSpan.FromDays(3), null, false, 50),
                new("GrapeSeeds", Season.Autumn, TimeSpan.FromDays(3), TimeSpan.FromDays(2), false, 60),
                new("CranberrySeeds", Season.Autumn, TimeSpan.FromDays(3), null, true, 240),
                new("BeetSeeds", Season.Autumn, TimeSpan.FromDays(3), null, false, 20),
                new("PumpkinSeeds", Season.Autumn, TimeSpan.FromDays(3), null, false, 100),
                new("SunflowerSeeds", Season.Summer, TimeSpan.FromDays(2), null, true, 50),
                new("RiceSeeds", Season.Spring, TimeSpan.FromDays(2), null, false, 75),
                new("CoffeeBeanSeeds", Season.Spring, TimeSpan.FromDays(4), TimeSpan.FromDays(2), true, 700)
            };

            foreach (var command in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(command);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}