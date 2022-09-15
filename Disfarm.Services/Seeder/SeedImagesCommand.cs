using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Discord.Image.Commands;
using MediatR;

namespace Disfarm.Services.Seeder
{
    public record SeedImagesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeedImagesHandler : IRequestHandler<SeedImagesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeedImagesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedImagesCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateImageCommand[]
            {
                new(Image.Placeholder, Language.English,
                    "https://cdn.discordapp.com/attachments/929693044054294578/929693137075597402/unknown.png"),
                new(Image.Placeholder, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/929693137075597402/unknown.png"),
                new(Image.Fishing, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233643318738974/Fishing.png"),
                new(Image.Farm, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233643612368986/Harvesting.png"),
                new(Image.Container, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233643851436122/OpenBox.png"),
                new(Image.PrivateRoom, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233644266668122/PrivateRoom.png"),
                new(Image.Relationship, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233644690284634/Relationship.png"),
                new(Image.DailyRide, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233644996472852/Ride.png"),
                new(Image.ShopBanner, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233645269114900/ShopBanner.png"),
                new(Image.Casino, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233645717880852/Casino.png"),
                new(Image.Contract, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233645998907402/Contract.png"),
                new(Image.Vendor, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/940120669105045555/Vendor.png"),
                new(Image.ShopSeed, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233711090307113/ShopSeed.png"),
                new(Image.UserAchievements, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233711602016256/UserAchievements.png"),
                new(Image.UserBanners, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233711832694884/UserBanners.png"),
                new(Image.UserCollection, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233712075984896/UserCollection.png"),
                new(Image.UserInventory, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233712356986900/UserInventory.png"),
                new(Image.WorldInfo, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233736319062066/WorldInfo.png"),
                new(Image.UserTitles, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233736650391633/UserTitles.png"),
                new(Image.ShopRole, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935233710884782150/ShopRole.png"),
                new(Image.GetPremium, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/935632605750128650/GetPremium.png"),
                new(Image.PremiumInfoRole, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/935632606232453171/PremiumInfoRole.png"),
                new(Image.PremiumInfoWardrobe, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/935632606446354483/PremiumInfoWardrobe.png"),
                new(Image.PremiumInfoCommandColor, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/935632606005973002/PremiumInfoCommandColor.png"),
                new(Image.Rating, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/935676150636748830/Rating.png"),
                new(Image.DonateInfo, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/935671070017613834/DonateInfo.png"),
                new(Image.DailyReward, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/940068963134631997/DailyReward.png"),
                new(Image.DailyRewardPremium, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/940068963348537364/DailyRewardPremium.png"),
                new(Image.ReferralRewards, Language.Russian,
                    "https://cdn.discordapp.com/attachments/931165008262492160/940939305516408842/ReferralRewards.png"),
                new(Image.Referral, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/940939388387467325/Referral.png"),
                new(Image.NotExpectedException, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/942788475466428476/NotExpectedException.gif"),
                new(Image.ExpectedException, Language.Russian,
                    "https://cdn.discordapp.com/attachments/929693044054294578/942794923432878160/Error.png")
            };

            foreach (var createImageCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createImageCommand);

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