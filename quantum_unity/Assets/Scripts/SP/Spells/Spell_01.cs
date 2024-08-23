namespace CosmicraftsSP {
    
using System.Collections.Generic;
using UnityEngine;
/*
 * Spell 1 Laser Beam
 */
public class Spell_01 : Spell
{
    //List of the affected units
    List<Unit> Targets;
    //Damage delays
    float delaydmg;
    //Line renderer reference
    public LineRenderer Lazer;
    //The start of the laser
    public GameObject StartLazer;
    //The end of the laser
    public GameObject EndLazer;

    // Start is called before the first frame update
    protected override void Start()
    {
        //Initialize basic variables
        base.Start();
        Targets = new List<Unit>();
        delaydmg = 0.25f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 50f, 0f));

        //Set the position and orientation of the lazer
        if (GameMng.GM.MainStationsExist())
        {
            Vector3 target = GameMng.GM.Targets[MyTeam == Team.Blue ? 0 : 1].transform.position;
            Vector3 origin = GameMng.GM.Targets[MyTeam == Team.Blue ? 1 : 0].transform.position;
            Lazer.SetPosition(0, origin);
            Lazer.SetPosition(1, target);
            StartLazer.transform.position = origin;
            EndLazer.transform.position = target;
            StartLazer.transform.localRotation = Quaternion.Euler(new Vector3(0, MyTeam == Team.Blue ? 90 : 270, 0));
        }
    }

    protected override void Update()
    {
        base.Update();

        //Damage time delay 
        if (delaydmg > 0f)
        {
            delaydmg -= Time.deltaTime;
        } else
        {
            //Apply damage to the targets
            delaydmg = 0.25f;
            foreach(Unit unit in Targets)
            {
                unit.AddDmg(10, TypeDmg.Shield);
            }
        }
    }

    //Add enemy´s units as targets when they collide with the laser
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit unit = other.gameObject.GetComponent<Unit>();
            if (!Targets.Contains(unit) && !unit.IsMyTeam(MyTeam))
            {
                Targets.Add(unit);
            }
        }
    }

    //Delete targets when they get out from the laser
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit unit = other.gameObject.GetComponent<Unit>();
            if (Targets.Contains(unit))
            {
                Targets.Remove(unit);
            }
        }
    }
}}