using System;
using AutoMapper;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Statistic.Models
{
    public record UserStatisticDto(
        Guid Id,
        StatisticPeriod Period,
        Data.Enums.Statistic Type,
        uint Amount,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserStatisticToDtoProfile : Profile
    {
        public UserStatisticToDtoProfile() => CreateMap<UserStatistic, UserStatisticDto>();
    }

    public class DtoToUserStatisticProfile : Profile
    {
        public DtoToUserStatisticProfile() => CreateMap<UserStatisticDto, UserStatistic>();
    }
}