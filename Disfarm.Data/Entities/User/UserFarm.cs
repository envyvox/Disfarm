using System;
using Disfarm.Data.Entities.Resource;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.User
{
    public class UserFarm : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public uint Number { get; set; }
        public FieldState State { get; set; }
        public Guid? SeedId { get; set; }
        public bool InReGrowth { get; set; }
        public TimeSpan? BeenGrowingFor { get; set; }
        public DateTimeOffset? CompleteAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public User User { get; set; }
        public Seed Seed { get; set; }
    }

    public class UserFarmConfiguration : IEntityTypeConfiguration<UserFarm>
    {
        public void Configure(EntityTypeBuilder<UserFarm> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.Number }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.SeedId);
            builder.Property(x => x.InReGrowth).IsRequired();
            builder.Property(x => x.BeenGrowingFor);
            builder.Property(x => x.CompleteAt);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Seed)
                .WithMany()
                .HasForeignKey(x => x.SeedId);
        }
    }
}