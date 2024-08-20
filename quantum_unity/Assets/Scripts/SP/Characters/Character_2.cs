namespace CosmicraftsSP {/* 
 * This is the 2° in-game character controller 
 * Dofshlu
 */
public class Character_2 : GameCharacter
{
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.criticalStrikeChance += 0.1f; //Increment Chance critical strike by 10% (Skill 1)
            shooter.BulletDamage = (int)(shooter.BulletDamage * 1.2f); //Increment Damage bullet by 20% (Skill 2)
        }
        unit.DodgeChance += 0.1f; //Increment dodge chance in 100% (skill 3)
    }
    
}
}
