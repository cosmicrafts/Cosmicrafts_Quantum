namespace CosmicraftsSP {
    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{

    public static WaveController instance;

    public GameObject[] waves;
    public GameObject[] BSwaves;
    private int actualWave = 0;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); }  { instance = this; }
    }

    private void Start()
    {
        GameMng.GM.Targets[0].gameObject.SetActive(false);
        waves[actualWave].SetActive(true);
        GameMng.GM.Targets[0] = BSwaves[0].GetComponent<Unit>();;
        GameMng.GM.Targets[0].IsBaseStation = true;
        GameMng.GM.Targets[0].gameObject.AddComponent<MainStation>();
        GameMng.GM.Targets[0].MyTeam = Team.Red; GameMng.GM.Targets[0].PlayerId = 2;
        
    }

    public void OnBaseDestroyed()
    {
        waves[actualWave].SetActive(false);
           
        actualWave += 1;
        if (waves.Length > actualWave)
        {
            waves[actualWave].SetActive(true);
            GameMng.GM.Targets[0] = BSwaves[actualWave].GetComponent<Unit>();; //GameMng.GM.Targets[0] = Instantiate(BSwaves[actualWave], GameMng.GM.BS_Positions[0], Quaternion.identity).GetComponent<Unit>();
            GameMng.GM.Targets[0].IsBaseStation = true;
            GameMng.GM.Targets[0].gameObject.AddComponent<MainStation>();
            GameMng.GM.Targets[0].MyTeam = Team.Red; GameMng.GM.Targets[0].PlayerId = 2;
            
            Ship[] ships = FindObjectsOfType<Ship>();
            foreach(Ship ship in ships) { if (ship.MyTeam == Team.Blue) { 
                Destroy(ship.gameObject);
                GameMng.GM.DeleteUnit(ship);
            } }
            
            //LLamar UI De Wave Complete
            
        }
        else
        {
            //End the game
            GameMng.GM.EndGame(Team.Blue);
        }
    }
   
}
}