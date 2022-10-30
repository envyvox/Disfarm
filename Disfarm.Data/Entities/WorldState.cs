using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities
{
	public class WorldState : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
	{
		public Guid Id { get; set; }
		public Season CurrentSeason { get; set; }
		public Weather WeatherToday { get; set; }
		public Weather WeatherTomorrow { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
	}

	public class WorldStateConfiguration : IEntityTypeConfiguration<WorldState>
	{
		public void Configure(EntityTypeBuilder<WorldState> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.CurrentSeason).IsRequired();
			builder.Property(x => x.WeatherToday).IsRequired();
			builder.Property(x => x.WeatherTomorrow).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();
		}
	}
}