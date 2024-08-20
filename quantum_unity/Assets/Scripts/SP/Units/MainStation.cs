namespace CosmicraftsSP {
using UnityEngine;

/*
 * This is the base station script (for unique behaviors) 
 */

public class MainStation : MonoBehaviour
{
    //Unit data reference
    Unit MyUnit;

    private void Start()
    {
        MyUnit = GetComponent<Unit>();
    }
}
}