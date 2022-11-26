using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.Resource.Ingredient
{
    public class FoodIngredient : IUniqueIdentifiedEntity, IAmountEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }

        public IngredientCategory Category { get; set; }

        /// <summary>
        /// Represents ingredient id (crop id, etc).
        /// </summary>
        public Guid IngredientId { get; set; }

        public uint Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Guid FoodId { get; set; }
        public Food Food { get; set; }
    }

    public class FoodIngredientConfiguration : IEntityTypeConfiguration<FoodIngredient>
    {
        public void Configure(EntityTypeBuilder<FoodIngredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.FoodId, x.Category, x.IngredientId });

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.IngredientId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.Food)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.FoodId);
        }
    }
}