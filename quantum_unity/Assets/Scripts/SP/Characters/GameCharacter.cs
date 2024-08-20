namespace CosmicraftsSP {

using UnityEngine;
/* 
 * This is the parent class for characters controllers 
 */
public class GameCharacter : MonoBehaviour
{
    //Key NFT ID
    public string IdKey;
    //Character Icon
    public Sprite Icon;

    //Do something when player deploy units
    public virtual void DeployUnit(Unit unit)
    {
        //Stay empty, write code in child class
    }
    //Do something when player casts spells
    public virtual void DeploySpell(Spell spell)
    {
        //Stay empty, write code in child class
    }
}
}