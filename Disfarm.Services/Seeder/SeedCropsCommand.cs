using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Util;
using Disfarm.Services.Game.Crop.Commands;
using Disfarm.Services.Game.Seed.Queries;
using MediatR;

namespace Disfarm.Services.Seeder
{
    public record SeedCropsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeedCropsHandler : IRequestHandler<SeedCropsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeedCropsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedCropsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var seeds = await _mediator.Send(new GetSeedsQuery());

            foreach (var seed in seeds)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(new CreateCropCommand(seed.Name.Replace("Seeds", ""), 999, seed.Id));

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