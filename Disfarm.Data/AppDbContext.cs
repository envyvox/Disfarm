using System;
using Disfarm.Data.Converters;
using Disfarm.Data.Entities;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.UseEntityTypeConfiguration<AppDbContext>();
            modelBuilder.UseSnakeCaseNamingConvention();
            modelBuilder.UseValueConverterForType<DateTime>(new DateTimeUtcKindConverter());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserBanner> UserBanners { get; set; }
        public DbSet<UserBuilding> UserBuildings { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<UserContainer> UserContainers { get; set; }
        public DbSet<UserCooldown> UserCooldowns { get; set; }
        public DbSet<UserCrop> UserCrops { get; set; }
        public DbSet<UserCurrency> UserCurrencies { get; set; }
        public DbSet<UserDailyReward> UserDailyRewards { get; set; }
        public DbSet<UserEffect> UserEffects { get; set; }
        public DbSet<UserFarm> UserFarms { get; set; }
        public DbSet<UserFish> UserFishes { get; set; }
        public DbSet<UserMovement> UserMovements { get; set; }
        public DbSet<UserReferrer> UserReferrers { get; set; }
        public DbSet<UserSeed> UserSeeds { get; set; }
        public DbSet<UserStatistic> UserStatistics { get; set; }
        public DbSet<UserTitle> UserTitles { get; set; }

        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Fish> Fishes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<Seed> Seeds { get; set; }
        public DbSet<WorldProperty> WorldProperties { get; set; }
        public DbSet<WorldState> WorldStates { get; set; }
    }
}