using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.Resource
{
    public class Seed : IUniqueIdentifiedEntity, INamedEntity, IPricedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Season Season { get; set; }
        public TimeSpan Growth { get; set; }
        public TimeSpan? ReGrowth { get; set; }
        public bool IsMultiply { get; set; }
        public uint Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Crop Crop { get; set; }
    }

    public class SeedConfiguration : IEntityTypeConfiguration<Seed>
    {
        public void Configure(EntityTypeBuilder<Seed> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Season).IsRequired();
            builder.Property(x => x.Growth).IsRequired();
            builder.Property(x => x.ReGrowth);
            builder.Property(x => x.IsMultiply).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
        }
    }
}