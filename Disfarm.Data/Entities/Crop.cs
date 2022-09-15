using System;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities
{
    public class Crop : IUniqueIdentifiedEntity, INamedEntity, IPricedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Price { get; set; }

        public Guid SeedId { get; set; }
        public Seed Seed { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class CropConfiguration : IEntityTypeConfiguration<Crop>
    {
        public void Configure(EntityTypeBuilder<Crop> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.Seed)
                .WithOne(x => x.Crop)
                .HasForeignKey<Crop>(x => x.SeedId);
        }
    }
}