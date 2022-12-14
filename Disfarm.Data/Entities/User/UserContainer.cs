using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.User
{
	public class UserContainer : IUniqueIdentifiedEntity, IAmountEntity, ICreatedEntity, IUpdatedEntity
	{
		public Guid Id { get; set; }
		public long UserId { get; set; }
		public Container Type { get; set; }
		public uint Amount { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }

		public User User { get; set; }
	}

	public class UserContainerConfiguration : IEntityTypeConfiguration<UserContainer>
	{
		public void Configure(EntityTypeBuilder<UserContainer> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => new { x.UserId, x.Type }).IsUnique();

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.Type).IsRequired();
			builder.Property(x => x.Amount).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();

			builder
				.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);
		}
	}
}