using System.Collections.Generic;
using UnityEngine;

namespace CosmicraftsSP
{
    [CreateAssetMenu(fileName = "NewCharacterBase", menuName = "Cosmicrafts/CharacterBase")]
    public class CharacterBaseSO : ScriptableObject
    {
        #region DataBase

        //----Prefab-----------------------------------------------------------
        [Tooltip("Reference to the base prefab")]
        [Header("Base Prefab")]
        [SerializeField]
        private GameObject basePrefab;

        //----HP Override-----------------------------------------------------------
        [Tooltip("Override for unit HP, -1 means no override")]
        [Header("HP Override")]
        [Range(1, 9999)]
        [SerializeField]
        private int hpOverride = -1;

        //----Shield Override-----------------------------------------------------------
        [Tooltip("Override for unit Shield, -1 means no override")]
        [Header("Shield Override")]
        [Range(1, 9999)]
        [SerializeField]
        private int shieldOverride = -1;

        //----Damage Override-----------------------------------------------------------
        [Tooltip("Override for unit Bullet Damage, -1 means no override")]
        [Header("Bullet Damage Override")]
        [Range(1, 9999)]
        [SerializeField]
        private int bulletDamageOverride = -1;

        #endregion

        #region Skills

        [Tooltip("List of character skills")]
        [Header("Character Skills")]
        [SerializeField]
        private List<CharacterSkill> skills = new List<CharacterSkill>();

        #endregion

        #region Variables de Lectura

        public GameObject BasePrefab => basePrefab;

        public int HpOverride => hpOverride;

        public int ShieldOverride => shieldOverride;

        public int BulletDamageOverride => bulletDamageOverride;

        public List<CharacterSkill> Skills => skills;

        #endregion

        #region Methods

        // Method to apply overrides to the instantiated unit
        public void ApplyOverridesToUnit(Unit unitComponent)
        {
            if (unitComponent != null)
            {
                if (hpOverride >= 0)
                {
                    unitComponent.SetMaxHitPoints(hpOverride);
                    unitComponent.HitPoints = hpOverride;
                }
                if (shieldOverride >= 0)
                {
                    unitComponent.SetMaxShield(shieldOverride);
                    unitComponent.Shield = shieldOverride;
                }
                if (bulletDamageOverride >= 0)
                {
                    Shooter shooterComponent = unitComponent.GetComponent<Shooter>();
                    if (shooterComponent != null)
                    {
                        shooterComponent.BulletDamage = bulletDamageOverride;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Unit component not found on the instantiated base prefab.");
            }
        }

        // Method to apply skills when a unit is deployed
        public void ApplySkillsOnDeploy(Unit unit)
        {
            foreach (var skill in skills)
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
            foreach (var skill in skills)
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
            foreach (var skill in skills)
            {
                if (skill.ApplicationType == SkillApplicationType.GameplayModifier)
                {
                    skill.ApplyGameplayModifier(); // Applies the gameplay modifier
                }
            }
        }

        #endregion
    }
}
