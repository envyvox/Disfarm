using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.User
{
	public class UserBuilding : IUniqueIdentifiedEntity, ICreatedEntity
	{
		public Guid Id { get; set; }
		public long UserId { get; set; }
		public Building Building { get; set; }
		public DateTimeOffset CreatedAt { get; set; }

		public User User { get; set; }
	}

	public class UserBuildingConfiguration : IEntityTypeConfiguration<UserBuilding>
	{
		public void Configure(EntityTypeBuilder<UserBuilding> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => new { x.UserId, x.Building }).IsUnique();

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.Building).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();

			builder
				.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);
		}
	}
}