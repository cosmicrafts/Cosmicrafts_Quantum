using System.Collections.Generic;
using UnityEngine;

namespace CosmicraftsSP
{
    [CreateAssetMenu(fileName = "NewCharacterBase", menuName = "Cosmicrafts/CharacterBase")]
    public class CharacterBaseSO : ScriptableObject
    {
        public GameObject BasePrefab;  // Reference to the base prefab (corrected to match the expected field name)
        public int hpOverride = -1;    // Override for unit HP, -1 means no override
        public int shieldOverride = -1; // Override for unit Shield, -1 means no override
        public int damageOverride = -1; // Override for unit Damage, -1 means no override

        // Dynamic list of character skills
        public List<CharacterSkill> Skills = new List<CharacterSkill>();

        // Method to apply skills when a unit is deployed
        public void ApplySkillsOnDeploy(Unit unit)
        {
            foreach (var skill in Skills)
            {
                if (skill.ApplicationType == SkillApplicationType.OnDeployUnit)
                {
                    skill.ApplySkill(unit); // Applies the skill to the unit
                }
            }
        }

        // Method to apply skills when a spell is deployed
        public void ApplySkillsOnDeploy(Spell spell)
        {
            foreach (var skill in Skills)
            {
                if (skill.ApplicationType == SkillApplicationType.OnDeployUnit)
                {
                    skill.ApplySkill(spell); // Applies the skill to the spell
                }
            }
        }

        // Method to apply broader gameplay modifiers
        public void ApplyGameplayModifiers()
        {
            foreach (var skill in Skills)
            {
                if (skill.ApplicationType == SkillApplicationType.GameplayModifier)
                {
                    skill.ApplyGameplayModifier(); // Applies the gameplay modifier
                }
            }
        }
    }
}
