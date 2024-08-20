namespace CosmicraftsSP {/* 
 * This is the 4° in-game character controller 
 * Sotzeer
 */
public class Character_4 : GameCharacter
{
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.CoolDown *= 0.8f; //Increment Speed attack 20% (Skill 1)
        }
        
        Ship ship = unit.GetComponent<Ship>();
        if (ship != null)
        {
            ship.MaxSpeed *= 1.25f; //Increment move speed in 25% (Skill 2)
        }

        unit.HitPoints = (int)(unit.HitPoints * 1.2f); //Increment HP in 20%(Skill 3)
        unit.SetMaxHitPoints(unit.HitPoints);
    }
}
}