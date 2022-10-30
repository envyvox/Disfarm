using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities
{
	public class Achievement : ICreatedEntity
	{
		public Enums.Achievement Type { get; set; }
		public AchievementRewardType RewardType { get; set; }
		public uint RewardNumber { get; set; }
		public uint Points { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}

	public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
	{
		public void Configure(EntityTypeBuilder<Achievement> builder)
		{
			builder.HasKey(x => x.Type);
			builder.HasIndex(x => x.Type).IsUnique();

			builder.Property(x => x.RewardType).IsRequired();
			builder.Property(x => x.RewardNumber).IsRequired();
			builder.Property(x => x.Points).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
		}
	}
}