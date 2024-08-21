using UnityEngine;

namespace CosmicraftsSP
{
    public class GameCharacter : MonoBehaviour
    {
        public CharacterBaseSO characterBaseSO;

        // Initialize the character using the SO
        public void InitializeCharacter(CharacterBaseSO characterSO)
        {
            characterBaseSO = characterSO;

            // Apply gameplay modifiers as soon as the character is initialized
            ApplyGameplayModifiers();
        }

        // Method to deploy units and apply relevant skills
        public void DeployUnit(Unit unit)
        {
            if (characterBaseSO != null)
            {
                foreach (var skill in characterBaseSO.Skills)
                {
                    if (skill.ApplicationType == SkillApplicationType.OnDeployUnit)
                    {
                        skill.ApplySkill(unit); // Apply the skill to the unit
                    }
                }
            }
        }

        // Method to deploy spells and apply relevant skills
        public void DeploySpell(Spell spell)
        {
            if (characterBaseSO != null)
            {
                foreach (var skill in characterBaseSO.Skills)
                {
                    if (skill.ApplicationType == SkillApplicationType.OnDeployUnit)
                    {
                        skill.ApplySkill(spell); // Apply the skill to the spell (if applicable)
                    }
                }
            }
        }

        // Apply broader gameplay modifiers at the start or when relevant
        public void ApplyGameplayModifiers()
        {
            if (characterBaseSO != null)
            {
                foreach (var skill in characterBaseSO.Skills)
                {
                    if (skill.ApplicationType == SkillApplicationType.GameplayModifier)
                    {
                        skill.ApplyGameplayModifier(); // Apply the gameplay modifier
                    }
                }
            }
        }
    }
}
