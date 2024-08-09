using System.Collections.Generic;
using System.Linq;

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
                CategoryTag.Character => "Character",
                CategoryTag.Chest => "Chest",
                CategoryTag.Trophy => "Trophy",
                CategoryTag.Unit => "Unit",
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
        public static GeneralInfo ConvertToGeneralInfo(this GeneralMetadata general)
        {
            return new GeneralInfo
            {
                UnitId = (int)general.Id,
                Class = general.Name,
                Rarity = general.Rarity.HasValue ? (int)general.Rarity.GetValueOrDefault() : 0,
                Faction = general.Faction.HasValue ? general.Faction.GetValueOrDefault().ToString() : null,
                Name = general.Name,
                Description = general.Description,
                Icon = int.Parse(general.Image),
                SkinsText = general.Category.HasValue ? general.Category.GetValueOrDefault().ConvertToCategoryName() : null
            };
        }

        // Extension method to convert SkillMetadata to a list of Skills
        public static List<Skill> ConvertToSkills(this SkillMetadata skill)
        {
            var skillList = new List<Skill>();

            switch (skill.Tag)
            {
                case SkillMetadataTag.CriticalStrike:
                    skillList.Add(new Skill { SkillName = "Critical Strike", SkillValue = 0 });
                    break;
                case SkillMetadataTag.Evasion:
                    skillList.Add(new Skill { SkillName = "Evasion", SkillValue = 0 });
                    break;
                case SkillMetadataTag.Shield:
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
                    SkinId = (int)skin.General.Id,
                    SkinName = skin.General.Name,
                    SkinDescription = skin.General.Description,
                    SkinIcon = skin.General.Image,
                    SkinRarity = skin.General.Rarity.HasValue ? (int)skin.General.Rarity.GetValueOrDefault() : 0
                }
            };
        }
    }
}
