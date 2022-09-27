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

        [HttpPost, Route("crops")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedCrops()
        {
            return Ok(await _mediator.Send(new SeedCropsCommand()));
        }

        [HttpPost, Route("fishes")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedFishes()
        {
            return Ok(await _mediator.Send(new SeedFishesCommand()));
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

        [HttpPost, Route("seeds")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedSeeds()
        {
            return Ok(await _mediator.Send(new SeedSeedsCommand()));
        }

        [HttpPost, Route("world-properties")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> SeedWorldProperties()
        {
            return Ok(await _mediator.Send(new SeedWorldPropertiesCommand()));
        }
    }
}