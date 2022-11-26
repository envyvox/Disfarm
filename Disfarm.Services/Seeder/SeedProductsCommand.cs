using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Util;
using Disfarm.Services.Game.Product.Commands;
using MediatR;

namespace Disfarm.Services.Seeder
{
    public record SeedProductsCommand : IRequest<TotalAndAffectedCountDto>;
    
    public class SeedProductsHandler : IRequestHandler<SeedProductsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeedProductsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedProductsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateProductCommand[]
            {
                new("Egg", 72),
                new("Milk", 157),
                new("WheatFlour", 150),
                new("Oil", 72),
                new("Vinegar", 192),
                new("Sugar", 198),
                new("Mayonnaise", 173),
                new("Cheese", 189)
            };
            
            foreach (var createFoodCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createFoodCommand);

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