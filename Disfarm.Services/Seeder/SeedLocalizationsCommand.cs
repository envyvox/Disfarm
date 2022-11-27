using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Crop.Queries;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization.Commands;
using Disfarm.Services.Game.Product.Queries;
using Disfarm.Services.Game.Seed.Queries;
using MediatR;

namespace Disfarm.Services.Seeder
{
	public record SeedLocalizationsCommand : IRequest<TotalAndAffectedCountDto>;

	public class SeedLocalizationsHandler : IRequestHandler<SeedLocalizationsCommand, TotalAndAffectedCountDto>
	{
		private readonly IMediator _mediator;

		public SeedLocalizationsHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<TotalAndAffectedCountDto> Handle(SeedLocalizationsCommand request, CancellationToken ct)
		{
			var result = new TotalAndAffectedCountDto();
			var commands = new List<CreateLocalizationCommand>();

			foreach (var category in Enum
				         .GetValues(typeof(LocalizationCategory))
				         .Cast<LocalizationCategory>())
			{
				switch (category)
				{
					case LocalizationCategory.Basic:
					{
						commands.Add(new CreateLocalizationCommand(
							category, "Ruble", Language.English, "rubble", "rubbles", "rubbles"));
						commands.Add(new CreateLocalizationCommand(
							category, "Ruble", Language.Russian, "рубль", "рубля", "рублей"));

						break;
					}
					case LocalizationCategory.Currency:
					{
						commands.Add(new CreateLocalizationCommand(
							category, Currency.Token.ToString(), Language.English, "token", "tokens", "tokens"));
						commands.Add(new CreateLocalizationCommand(
							category, Currency.Token.ToString(), Language.Russian, "токен", "токена", "токенов"));
						commands.Add(new CreateLocalizationCommand(
							category, Currency.Chip.ToString(), Language.English, "chip", "chips", "chips"));
						commands.Add(new CreateLocalizationCommand(
							category, Currency.Chip.ToString(), Language.Russian, "чип", "чипа", "чипов"));

						break;
					}
					case LocalizationCategory.Container:
					{
						commands.Add(new CreateLocalizationCommand(
							category, Container.Token.ToString(), Language.English, "container with tokens",
							"containers with tokens", "containers with tokens"));
						commands.Add(new CreateLocalizationCommand(
							category, Container.Supply.ToString(), Language.English, "container with supplies",
							"containers with supplies", "containers with supplies"));
						commands.Add(new CreateLocalizationCommand(
							category, Container.Token.ToString(), Language.Russian, "контейнер с токенами",
							"контейнера с токенами", "контейнеров с токенами"));
						commands.Add(new CreateLocalizationCommand(
							category, Container.Supply.ToString(), Language.Russian, "контейнер с припасами",
							"контейнера с припасами", "контейнеров с припасами"));

						break;
					}
					case LocalizationCategory.Fish:
					{
						var fishes = await _mediator.Send(new GetFishesQuery());

						commands.AddRange(fishes.Select(fish => new CreateLocalizationCommand(
							category, fish.Name, Language.English, fish.Name, fish.Name, fish.Name)));
						commands.AddRange(fishes.Select(fish => new CreateLocalizationCommand(
							category, fish.Name, Language.Russian, fish.Name, fish.Name, fish.Name)));

						break;
					}
					case LocalizationCategory.Crop:
					{
						var crops = await _mediator.Send(new GetCropsQuery());

						commands.AddRange(crops.Select(crop => new CreateLocalizationCommand(
							category, crop.Name, Language.English, crop.Name, crop.Name, crop.Name)));
						commands.AddRange(crops.Select(crop => new CreateLocalizationCommand(
							category, crop.Name, Language.Russian, crop.Name, crop.Name, crop.Name)));

						break;
					}
					case LocalizationCategory.Seed:
					{
						var seeds = await _mediator.Send(new GetSeedsQuery());

						commands.AddRange(seeds.Select(seed => new CreateLocalizationCommand(
							category, seed.Name, Language.English, seed.Name, seed.Name, seed.Name)));
						commands.AddRange(seeds.Select(seed => new CreateLocalizationCommand(
							category, seed.Name, Language.Russian, seed.Name, seed.Name, seed.Name)));

						break;
					}
					case LocalizationCategory.Banner:
					{
						var banners = await _mediator.Send(new GetBannersQuery());

						commands.AddRange(banners.Select(banner => new CreateLocalizationCommand(
							category, banner.Name, Language.English, banner.Name, banner.Name, banner.Name)));
						commands.AddRange(banners.Select(banner => new CreateLocalizationCommand(
							category, banner.Name, Language.Russian, banner.Name, banner.Name, banner.Name)));

						break;
					}
					case LocalizationCategory.Product:
					{
						var products = await _mediator.Send(new GetProductsQuery());

						commands.AddRange(products.Select(product => new CreateLocalizationCommand(
							category, product.Name, Language.English, product.Name, product.Name, product.Name)));
						commands.AddRange(products.Select(product => new CreateLocalizationCommand(
							category, product.Name, Language.Russian, product.Name, product.Name, product.Name)));
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException();
					}
				}
			}

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