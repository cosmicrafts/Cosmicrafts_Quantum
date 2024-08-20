namespace CosmicraftsSP {/* 
 * This is the 4° in-game character controller 
 * Sotzeer
 */
public class Character_5 : GameCharacter
{
    void Start()
        {
            GameMng.GM.BOT.SpeedEnergy *= 0.8f; //Decrement Energy recovery bot in 20% (skill 1)
            GameMng.P.SpeedEnergy *= 1.1f; //Increment Energy recovery in 10% (skill 2)
        }
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        unit.DodgeChance += 0.1f; //Increment dodge chance in 100% (skill 3)
    }
}
}