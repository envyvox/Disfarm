using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disfarm.Data.Entities.User
{
	public class User : ICreatedEntity, IUpdatedEntity
	{
		public long Id { get; set; }
		public string About { get; set; }
		public uint Level { get; set; }
		public uint Xp { get; set; }
		public Title Title { get; set; }
		public Fraction Fraction { get; set; }
		public Location Location { get; set; }
		public string CommandColor { get; set; }
		public bool IsPremium { get; set; }
		public Language Language { get; set; }
		public CubeType CubeType { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
	}

	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => x.Id).IsUnique();

			builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
			builder.Property(x => x.About);
			builder.Property(x => x.Level).IsRequired();
			builder.Property(x => x.Xp).IsRequired();
			builder.Property(x => x.Title).IsRequired();
			builder.Property(x => x.Fraction).IsRequired();
			builder.Property(x => x.Location).IsRequired();
			builder.Property(x => x.CommandColor).IsRequired();
			builder.Property(x => x.IsPremium).IsRequired();
			builder.Property(x => x.Language).IsRequired();
			builder.Property(x => x.CubeType).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.UpdatedAt).IsRequired();
		}
	}
}