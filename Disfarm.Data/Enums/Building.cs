#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Disfarm.Data.Enums
{
    public enum Building : byte
    {
        /// <summary>
        /// Represents nothing.
        /// </summary>
        Undefined,
        Farm,
        FarmUpgradeL1,
        FarmUpgradeL2,
        FarmUpgradeL3,
        FarmUpgradeL4,
        FarmUpgradeL5,
        FarmUpgradeL6,
        FarmUpgradeL7,
        FarmUpgradeL8,
        FarmUpgradeL9,
        FarmUpgradeL10,
        FarmUpgradeL11,
        FarmUpgradeL12,
        FarmUpgradeL13,
        FarmUpgradeL14,
        FarmUpgradeL15,
        FarmUpgradeL16,
        FarmUpgradeL17,
        FarmUpgradeL18,
        FarmUpgradeL19
    }

    public static class BuildingHelper
    {
        public static BuildingCategory Category(this Building building)
        {
            return building switch
            {
                Building.Undefined => BuildingCategory.Undefined,
                Building.Farm => BuildingCategory.Farm,
                Building.FarmUpgradeL1 => BuildingCategory.Farm,
                Building.FarmUpgradeL2 => BuildingCategory.Farm,
                Building.FarmUpgradeL3 => BuildingCategory.Farm,
                Building.FarmUpgradeL4 => BuildingCategory.Farm,
                Building.FarmUpgradeL5 => BuildingCategory.Farm,
                Building.FarmUpgradeL6 => BuildingCategory.Farm,
                Building.FarmUpgradeL7 => BuildingCategory.Farm,
                Building.FarmUpgradeL8 => BuildingCategory.Farm,
                Building.FarmUpgradeL9 => BuildingCategory.Farm,
                Building.FarmUpgradeL10 => BuildingCategory.Farm,
                Building.FarmUpgradeL11 => BuildingCategory.Farm,
                Building.FarmUpgradeL12 => BuildingCategory.Farm,
                Building.FarmUpgradeL13 => BuildingCategory.Farm,
                Building.FarmUpgradeL14 => BuildingCategory.Farm,
                Building.FarmUpgradeL15 => BuildingCategory.Farm,
                Building.FarmUpgradeL16 => BuildingCategory.Farm,
                Building.FarmUpgradeL17 => BuildingCategory.Farm,
                Building.FarmUpgradeL18 => BuildingCategory.Farm,
                Building.FarmUpgradeL19 => BuildingCategory.Farm,
                _ => throw new ArgumentOutOfRangeException(nameof(building), building, null)
            };
        }

        public static string EmoteName(this Building building)
        {
            return building switch
            {
                Building.Undefined => "Blank",
                Building.Farm => "Farm",
                Building.FarmUpgradeL1 => "FarmUpgradeSlot",
                Building.FarmUpgradeL2 => "FarmUpgradeSlot",
                Building.FarmUpgradeL3 => "FarmUpgradeSpeed",
                Building.FarmUpgradeL4 => "FarmUpgradeSlot",
                Building.FarmUpgradeL5 => "FarmUpgradeSlot",
                Building.FarmUpgradeL6 => "FarmUpgradeSpeed",
                Building.FarmUpgradeL7 => "FarmUpgradeSlot",
                Building.FarmUpgradeL8 => "FarmUpgradeSlot",
                Building.FarmUpgradeL9 => "FarmUpgradeSpeed",
                Building.FarmUpgradeL10 => "FarmUpgradeSlot",
                Building.FarmUpgradeL11 => "FarmUpgradeSlot",
                Building.FarmUpgradeL12 => "FarmUpgradeSpeed",
                Building.FarmUpgradeL13 => "FarmUpgradeSlot",
                Building.FarmUpgradeL14 => "FarmUpgradeSlot",
                Building.FarmUpgradeL15 => "FarmUpgradeSpeed",
                Building.FarmUpgradeL16 => "FarmUpgradeSlot",
                Building.FarmUpgradeL17 => "FarmUpgradeSlot",
                Building.FarmUpgradeL18 => "FarmUpgradeSpeed",
                Building.FarmUpgradeL19 => "FarmUpgradeSlot",
                _ => throw new ArgumentOutOfRangeException(nameof(building), building, null)
            };
        }

        public static IEnumerable<uint>? NewCells(this Building building)
        {
            if (building.Category() is not BuildingCategory.Farm)
            {
                throw new ArgumentException("Building category is not farm");
            }

            return building switch
            {
                Building.Farm => new uint[] { 1, 2, 3 },
                Building.FarmUpgradeL1 => new uint[] { 4 },
                Building.FarmUpgradeL2 => new uint[] { 5 },
                Building.FarmUpgradeL4 => new uint[] { 6 },
                Building.FarmUpgradeL5 => new uint[] { 7 },
                Building.FarmUpgradeL7 => new uint[] { 8 },
                Building.FarmUpgradeL8 => new uint[] { 9, 10 },
                Building.FarmUpgradeL10 => new uint[] { 11 },
                Building.FarmUpgradeL11 => new uint[] { 12 },
                Building.FarmUpgradeL13 => new uint[] { 13 },
                Building.FarmUpgradeL14 => new uint[] { 14, 15 },
                Building.FarmUpgradeL16 => new uint[] { 16 },
                Building.FarmUpgradeL17 => new uint[] { 17 },
                Building.FarmUpgradeL19 => new uint[] { 18, 19, 20 },
                _ => null
            };
        }

        public static uint? SpeedBonusPercent(this Building building)
        {
            if (building.Category() is not BuildingCategory.Farm)
            {
                throw new ArgumentException("Building category is not farm");
            }

            return building switch
            {
                Building.FarmUpgradeL3 => 2,
                Building.FarmUpgradeL6 => 3,
                Building.FarmUpgradeL9 => 5,
                Building.FarmUpgradeL12 => 2,
                Building.FarmUpgradeL15 => 3,
                Building.FarmUpgradeL18 => 5,
                _ => null
            };
        }

        public static Building CurrentUpgrade(IList<Building> buildings)
        {
            if (buildings.Contains(Building.Undefined))
            {
                throw new ArgumentException("undefined building type");
            }

            return buildings.Max();
        }

        public static Building NextUpgrade(this Building building)
        {
            if (building is Building.Undefined)
            {
                throw new ArgumentException("undefined building type");
            }

            var maxValue = Enum.GetValues(typeof(Building)).Length - 1;
            var nextUpgradeHashcode = building.GetHashCode() + 1;

            if (nextUpgradeHashcode > maxValue) return Building.Undefined;

            var nextUpgrade = (Building)nextUpgradeHashcode;
            return building.Category() == nextUpgrade.Category() ? nextUpgrade : Building.Undefined;
        }
    }
}