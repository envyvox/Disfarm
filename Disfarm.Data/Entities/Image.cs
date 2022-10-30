using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities
{
	public class Image : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
	{
		public Guid Id { get; set; }
		public Enums.Image Type { get; set; }
		public Language Language { get; set; }
		public string Url { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
	}

	public class ImageConfiguration : IEntityTypeConfiguration<Image>
	{
		public void Configure(EntityTypeBuilder<Image> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => new { x.Type, x.Language });

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.Type).IsRequired();
			builder.Property(x => x.Language).IsRequired();
			builder.Property(x => x.Url).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();
		}
	}
}