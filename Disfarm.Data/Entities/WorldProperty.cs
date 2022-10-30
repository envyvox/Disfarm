using System;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities
{
	public class WorldProperty : ICreatedEntity, IUpdatedEntity
	{
		public Enums.WorldProperty Type { get; set; }
		public uint Value { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
	}

	public class WorldPropertyConfiguration : IEntityTypeConfiguration<WorldProperty>
	{
		public void Configure(EntityTypeBuilder<WorldProperty> builder)
		{
			builder.HasKey(x => x.Type);
			builder.HasIndex(x => x.Type).IsUnique();

			builder.Property(x => x.Type).IsRequired();
			builder.Property(x => x.Value).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();
		}
	}
}