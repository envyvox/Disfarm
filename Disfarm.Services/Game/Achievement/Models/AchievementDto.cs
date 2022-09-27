using System;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Achievement.Models
{
    public record AchievementDto(
        Data.Enums.Achievement Type,
        AchievementRewardType RewardType,
        uint RewardNumber,
        uint Points,
        DateTimeOffset CreatedAt);

    public class AchievementToDtoProfile : Profile
    {
        public AchievementToDtoProfile() => CreateMap<Data.Entities.Achievement, AchievementDto>();
    }

    public class DtoToAchievementProfile : Profile
    {
        public DtoToAchievementProfile() => CreateMap<AchievementDto, Data.Entities.Achievement>();
    }
}