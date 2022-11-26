using System;
using System.Collections.Generic;
using Disfarm.Data.Entities.Resource.Ingredient;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.Resource
{
    public class Food : IUniqueIdentifiedEntity, INamedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool RecipeSellable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public List<FoodIngredient> Ingredients { get; set; }
    }

    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.RecipeSellable).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
        }
    }
}