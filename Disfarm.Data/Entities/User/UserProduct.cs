using System;
using Disfarm.Data.Entities.Resource;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.User
{
	public class UserProduct : IUniqueIdentifiedEntity, IAmountEntity, ICreatedEntity, IUpdatedEntity
	{
		public Guid Id { get; set; }
		public long UserId { get; set; }
		public Guid ProductId { get; set; }
		public uint Amount { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }

		public User User { get; set; }
		public Product Product { get; set; }
	}

	public class UserProductConfiguration : IEntityTypeConfiguration<UserProduct>
	{
		public void Configure(EntityTypeBuilder<UserProduct> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => new {x.UserId, x.ProductId}).IsUnique();

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.Amount).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();

			builder
				.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);

			builder
				.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);
		}
	}
}