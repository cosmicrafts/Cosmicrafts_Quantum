namespace CosmicraftsSP {/* 
 * This is the first in-game character controller 
 * Wegnar
 */
public class Character_1 : GameCharacter
{
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.criticalStrikeChance += 0.2f; //Increment Chance critical strike by 20% (Skill 1)
            shooter.CoolDown *= 0.8f; //Increment Speed attack to Units (Skill 2)
        }
        unit.Shield = (int)(unit.Shield * 1.3f); //Increment shield by 30% (Skill 3)
        unit.SetMaxShield(unit.Shield);
    }
}

//unit.ShieldDelay *= 0.75f; //Reduce time to recover shields to units
//shooter.BulletDamage = (int)((float)shooter.BulletDamage * 1.25f); //Increment Damage

// Player begins with more energy
        /*void Start()
        {
            GameMng.P.MaxEnergy += 5f;
        }*/}