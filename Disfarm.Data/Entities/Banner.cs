using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities
{
	public class Banner : IUniqueIdentifiedEntity, INamedEntity, IPricedEntity, ICreatedEntity, IUpdatedEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public BannerRarity Rarity { get; set; }
		public uint Price { get; set; }
		public string Url { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
	}

	public class BannerConfiguration : IEntityTypeConfiguration<Banner>
	{
		public void Configure(EntityTypeBuilder<Banner> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => x.Name).IsUnique();

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.Name).IsRequired();
			builder.Property(x => x.Rarity).IsRequired();
			builder.Property(x => x.Price).IsRequired();
			builder.Property(x => x.Url).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();
		}
	}
}