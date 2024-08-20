namespace CosmicraftsSP {/* 
 * This is the 4° in-game character controller 
 * Sotzeer
 */
public class Character_6 : GameCharacter
{
    //Add 2 shield points and movement speed to units
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.CoolDown *= 0.9f; //Increment Speed attack 10% (Skill 2)
        }
        
        Ship ship = unit.GetComponent<Ship>();
        if (ship != null)
        {
            ship.MaxSpeed *= 1.80f; //Increment move speed in 80% (Skill 1)
        }

        unit.Size *= 0.8f; //decrease size spaceship to 80% (Skill 3)
    }
}
}