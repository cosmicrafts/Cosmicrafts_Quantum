using System.Collections.Generic;

namespace Cosmicrafts.MainCanister.Models
{
    public static class MetadataExtensions
    {
        // Extension method to convert Category to a string name
        public static string ConvertToCategoryName(this Category category)
        {
            return category.Tag switch
            {
                CategoryTag.Avatar => "Avatar",
                CategoryTag.Chest => "Chest",
                CategoryTag.Trophy => "Trophy",
                CategoryTag.Unit => category.AsUnit().ToString(),
                _ => "Unknown",
            };
        }

        // Extension method to convert BasicMetadata to a list of BasicStats
        public static List<BasicStat> ConvertToBasicStats(this BasicMetadata basic)
        {
            return new List<BasicStat>
            {
                new BasicStat { StatName = "Damage", StatValue = (int)basic.Damage },
                new BasicStat { StatName = "Health", StatValue = (int)basic.Health },
                new BasicStat { StatName = "Level", StatValue = (int)basic.Level }
            };
        }

        // Extension method to convert GeneralMetadata to GeneralInfo
        public static GeneralInfo ConvertToGeneralInfo(this GeneralMetadata general, Category category)
        {
            int iconValue;
            if (!int.TryParse(general.Image, out iconValue))
            {
                iconValue = 0; // Default value in case of parsing failure
            }

            return new GeneralInfo
            {
                UnitId = (int)general.Id,
                Class = general.Name,
                Rarity = general.Rarity.HasValue ? (int)general.Rarity.GetValueOrDefault() : 0,
                Faction = general.Faction.HasValue ? general.Faction.GetValueOrDefault().ToString() : null,
                Name = general.Name,
                Description = general.Description,
                Icon = iconValue, // Use the parsed or default icon value
            };
        }


        // Extension method to convert SkillMetadata to a list of Skills
        public static List<Skill> ConvertToSkills(this SkillMetadata skill)
        {
            var skillList = new List<Skill>();

            switch (skill)
            {
                case SkillMetadata.CriticalStrike:
                    skillList.Add(new Skill { SkillName = "Critical Strike", SkillValue = 0 });
                    break;
                case SkillMetadata.Evasion:
                    skillList.Add(new Skill { SkillName = "Evasion", SkillValue = 0 });
                    break;
                case SkillMetadata.Shield:
                    skillList.Add(new Skill { SkillName = "Shield", SkillValue = 0 });
                    break;
            }

            return skillList;
        }

        // Extension method to convert SkinMetadata to a list of Skins
        public static List<Skin> ConvertToSkin(this SkinMetadata skin)
        {
            return new List<Skin>
            {
                new Skin
                {
                    SkinId = 0, // Assuming SkinId can be 0 or a default value
                    SkinName = "Default Skin", // Replace with actual name if necessary
                    SkinDescription = "Default Skin Description", // Replace with actual description if necessary
                    SkinIcon = "default_icon.png", // Replace with actual icon if necessary
                    SkinRarity = 0 // Assuming 0 is the default rarity
                }
            };
        }
    }
}
