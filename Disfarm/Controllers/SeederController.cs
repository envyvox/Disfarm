using System.Threading.Tasks;
using Disfarm.Data.Util;
using Disfarm.Services.Seeder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Disfarm.Controllers
{
    [ApiController, Route("seeder")]
    public class SeederController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("achievements")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedAchievements()
        {
            return Ok(await _mediator.Send(new SeedAchievementsCommand()));
        }

        [HttpPost, Route("banners")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedBanners()
        {
            return Ok(await _mediator.Send(new SeedBannersCommand()));
        }

        [HttpPost, Route("images")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedImages()
        {
            return Ok(await _mediator.Send(new SeedImagesCommand()));
        }

        [HttpPost, Route("localizations")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedLocalizations()
        {
            return Ok(await _mediator.Send(new SeedLocalizationsCommand()));
        }

        [HttpPost, Route("world-properties")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedWorldProperties()
        {
            return Ok(await _mediator.Send(new SeedWorldPropertiesCommand()));
        }

        [HttpPost, Route("resource/crops")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedCrops()
        {
            return Ok(await _mediator.Send(new SeedCropsCommand()));
        }

        [HttpPost, Route("resource/fishes")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedFishes()
        {
            return Ok(await _mediator.Send(new SeedFishesCommand()));
        }

        [HttpPost, Route("resource/foods")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedFoods()
        {
            return Ok(await _mediator.Send(new SeedFoodsCommand()));
        }

        [HttpPost, Route("resource/products")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedProducts()
        {
            return Ok(await _mediator.Send(new SeedProductsCommand()));
        }

        [HttpPost, Route("resource/seeds")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedSeeds()
        {
            return Ok(await _mediator.Send(new SeedSeedsCommand()));
        }
    }
}