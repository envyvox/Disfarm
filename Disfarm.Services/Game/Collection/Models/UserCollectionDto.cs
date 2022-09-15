using System;
using AutoMapper;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Collection.Models
{
    public record UserCollectionDto(
        Guid Id,
        CollectionCategory Category,
        Guid ItemId,
        DateTimeOffset CreatedAt);

    public class UserCollectionToDtoProfile : Profile
    {
        public UserCollectionToDtoProfile() => CreateMap<UserCollection, UserCollectionDto>();
    }

    public class DtoToUserCollectionProfile : Profile
    {
        public DtoToUserCollectionProfile() => CreateMap<UserCollectionDto, UserCollection>();
    }
}