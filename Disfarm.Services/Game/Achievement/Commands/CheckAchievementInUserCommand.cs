using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Achievement.Queries;
using Disfarm.Services.Game.Collection.Queries;
using Disfarm.Services.Game.Crop.Queries;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Statistic.Queries;
using MediatR;

namespace Disfarm.Services.Game.Achievement.Commands
{
	public record CheckAchievementInUserCommand(
			long UserId,
			Data.Enums.Achievement.Achievement Type)
		: IRequest;

	public class CheckAchievementInUserHandler : IRequestHandler<CheckAchievementInUserCommand>
	{
		private readonly IMediator _mediator;

		public CheckAchievementInUserHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<Unit> Handle(CheckAchievementInUserCommand request, CancellationToken ct)
		{
			var exist = await _mediator.Send(new CheckUserHasAchievementQuery(request.UserId, request.Type));

			if (exist) return Unit.Value;

			switch (request.Type)
			{
				// one-step achievements
				case Data.Enums.Achievement.Achievement.FirstFish:
				case Data.Enums.Achievement.Achievement.FirstPlant:
				case Data.Enums.Achievement.Achievement.FirstBet:
				case Data.Enums.Achievement.Achievement.FirstJackpot:
				case Data.Enums.Achievement.Achievement.FirstLotteryTicket:
				case Data.Enums.Achievement.Achievement.FirstVendorDeal:
				case Data.Enums.Achievement.Achievement.CatchEpicFish:
				case Data.Enums.Achievement.Achievement.CatchMythicalFish:
				case Data.Enums.Achievement.Achievement.CatchLegendaryFish:
					{
						await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						break;
					}

				// statistic achievements
				case Data.Enums.Achievement.Achievement.Catch50Fish:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CatchFish));

						if (stat.Amount >= 50)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Catch100Fish:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CatchFish));

						if (stat.Amount >= 100)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Catch300Fish:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CatchFish));

						if (stat.Amount >= 300)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Plant25Seed:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.SeedPlanted));

						if (stat.Amount >= 25)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Plant50Seed:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.SeedPlanted));

						if (stat.Amount >= 50)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Plant150Seed:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.SeedPlanted));

						if (stat.Amount >= 150)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Collect50Crop:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CropHarvested));

						if (stat.Amount >= 50)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Collect100Crop:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CropHarvested));

						if (stat.Amount >= 100)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Collect300Crop:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CropHarvested));

						if (stat.Amount >= 300)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Casino33Bet:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CasinoBet));

						if (stat.Amount >= 33)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Casino333Bet:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CasinoBet));

						if (stat.Amount >= 333)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Casino777Bet:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CasinoBet));

						if (stat.Amount >= 777)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Casino22LotteryBuy:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CasinoLotteryBuy));

						if (stat.Amount >= 22)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Casino99LotteryBuy:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CasinoLotteryBuy));

						if (stat.Amount >= 99)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Casino15LotteryGift:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.CasinoLotteryGift));

						if (stat.Amount >= 15)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Vendor100Sell:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.VendorSell));

						if (stat.Amount >= 100)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Vendor777Sell:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.VendorSell));

						if (stat.Amount >= 777)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Vendor1500Sell:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.VendorSell));

						if (stat.Amount >= 1500)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.Vendor3500Sell:
					{
						var stat = await _mediator.Send(new GetUserStatisticQuery(
							request.UserId, StatisticPeriod.General, Data.Enums.Statistic.VendorSell));

						if (stat.Amount >= 3500)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}

				// collection achievements
				case Data.Enums.Achievement.Achievement.CompleteCollectionFish:
					{
						var userCollection = await _mediator.Send(new GetUserCollectionsQuery(
							request.UserId, CollectionCategory.Fish));
						var fishes = await _mediator.Send(new GetFishesQuery());

						if (userCollection.Count >= fishes.Count)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				case Data.Enums.Achievement.Achievement.CompleteCollectionCrop:
					{
						var userCollection = await _mediator.Send(new GetUserCollectionsQuery(
							request.UserId, CollectionCategory.Fish));
						var crops = await _mediator.Send(new GetCropsQuery());

						if (userCollection.Count >= crops.Count)
						{
							await _mediator.Send(new CreateUserAchievementCommand(request.UserId, request.Type));
						}

						break;
					}
				default: throw new ArgumentOutOfRangeException();
			}

			return Unit.Value;
		}
	}
}