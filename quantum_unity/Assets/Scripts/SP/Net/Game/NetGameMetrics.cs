using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGameMetrics
{
    //Energy variables
    public float EnergyUsed { get; set; }
    public float EnergyGenerated { get; set; }
    public float EnergyWasted { get; set; }
    public float EnergyChargeRatePerSec { get; set; }

    //Battle variables
    public float Damage { get; set; }
    public int Kills { get; set; }
    public int Deploys { get; set; }
}
