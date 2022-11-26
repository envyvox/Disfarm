using System;
using AutoMapper;

namespace Disfarm.Services.Game.Product.Models
{
    public record ProductDto(
        Guid Id,
        string Name,
        uint Price,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class ProductToDtoProfile : Profile
    {
        public ProductToDtoProfile() => CreateMap<Data.Entities.Resource.Product, ProductDto>();
    }

    public class DtoToProductProfile : Profile
    {
        public DtoToProductProfile() => CreateMap<ProductDto, Data.Entities.Resource.Product>();
    }
}