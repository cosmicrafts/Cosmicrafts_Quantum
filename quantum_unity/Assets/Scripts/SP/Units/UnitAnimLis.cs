namespace CosmicraftsSP {
    using UnityEngine;

/*
 * The animation controller script for units
 */

public class UnitAnimLis : MonoBehaviour
{
    //Unit data reference
    Unit MyUnit;

    // Start is called before the first frame update
    void Start()
    {
        //Get unit data
        MyUnit = transform.parent.GetComponent<Unit>();
        //Set the attack animation speed
        AnimationClip attack_clip = MyUnit.GetAnimationClip("Attack");
        Shooter shooter = transform.parent.GetComponent<Shooter>();
        if (attack_clip != null && shooter != null)
            MyUnit.GetAnimator().SetFloat("AttackSpeed", attack_clip.length / shooter.CoolDown * 2);
    }

    //Called when the deth animation ends
    public void AE_EndDeath()
    {
        //Kill the unit
        MyUnit.DestroyUnit();
    }

    //Called an explosion effect
    public void AE_BlowUpUnit()
    {
        //Kill the unit
        MyUnit.BlowUpEffect();
    }
}
}