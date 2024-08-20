namespace CosmicraftsSP {/* 
 * This is the 3° in-game character controller 
 * Plagnor
 */
public class Character_3 : GameCharacter
{
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        
        Ship ship = unit.GetComponent<Ship>();
        if (ship != null)
        {
            ship.SpawnAreaSize += 2f; //Increment Spawn area in +2 (Skill 1)
        }
        
        unit.Shield = (int)(unit.Shield * 1.2f); //Increment shield by 20% (Skill 2)
        unit.SetMaxShield(unit.Shield);
        
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.RangeDetector += 2f;  //Increment range detection in+2 (Skill 3)
        }
    }
}
}