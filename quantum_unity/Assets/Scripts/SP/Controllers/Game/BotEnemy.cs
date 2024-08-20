namespace CosmicraftsSP {
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

/* 
 * This is the IA controller
 * Works with a timing loop and has 3 behaviour modes
 * Use his own energy and deck
 * The positions to deply units are predefined
 */
public class BotEnemy : MonoBehaviour
{
    //Bot IA modes
    /*public enum BotMode
    {
        Pasive,
        Aggressive,
        Random,
        Easy,
        Medium,
        Hard,
        Impossible,
    }
    
    //IA mode (change in inspector)
    public BotMode Mode = BotMode.Easy;
    
    public void SetBotType(BotMode botMode)
    {
        Mode = botMode;
    }
    
    switch (Mode)
            {
                case BotMode.Aggressive: //Select the most expensive card
                    {
                        if (CurrentEnergy < MaxCostUnit)
                        {
                            continue;
                        }
                        SelectedUnit = DeckUnits.FirstOrDefault(f => f.cost <= MaxCostUnit);
                    }
                    break;
                case BotMode.Pasive: //Select the cheapest card
                    {
                        SelectedUnit = DeckUnits.FirstOrDefault(f => f.cost <= MinCostUnit);
                    }
                    break;


                // Easy Mode// 
                case BotMode.Easy: 
                    if (CurrentEnergy < MaxCostUnit)
                    {
                        continue;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        SelectedUnit = DeckUnits[Random.Range(0, DeckUnits.Length)];
                        if (SelectedUnit.cost <= CurrentEnergy)
                        {
                            break;
                        }
                    }
                    SpeedEnergy = .25f;
                    break; 


                // medium Mode//
                    case BotMode.Medium:
                    if (CurrentEnergy < MaxCostUnit)
                    {
                        continue;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        SelectedUnit = DeckUnits[Random.Range(0, DeckUnits.Length)];
                        if (SelectedUnit.cost <= CurrentEnergy)
                        {
                            break;
                        }
                    }
                    SpeedEnergy = 0.5f;
                    MaxEnergy = 10f;
                    CurrentEnergy = 4f;
                    break; 


                 //Hard Mode//
                case BotMode.Hard:
                    if (CurrentEnergy < MaxCostUnit)
                    {
                        continue;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        SelectedUnit = DeckUnits[Random.Range(0, DeckUnits.Length)];
                        if (SelectedUnit.cost <= CurrentEnergy)
                        {
                            break;
                        }
                    }
                    SpeedEnergy = .5f;
                    break; 


                    //Impossible Mode//
                    case BotMode.Impossible: 
                    if (CurrentEnergy < MaxCostUnit)
                    {
                        continue;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        SelectedUnit = DeckUnits[Random.Range(0, DeckUnits.Length)];
                        if (SelectedUnit.cost <= CurrentEnergy)
                        {
                            break;
                        }
                    }
                    SpeedEnergy = 0.5f;
                    MaxEnergy = 10f;
                    CurrentEnergy = 10f;
                    break; 

                default: //Select a random card
                    {
                        for(int i=0; i<10; i++)
                        {
                            SelectedUnit = DeckUnits[Random.Range(0, DeckUnits.Length)];
                            if (SelectedUnit.cost <= CurrentEnergy)
                            {
                                break;
                            }
                        }
                    }
                    MaxEnergy = 10f;
                    break;
            }
    
    */
    

    //Bot player Name 
    public string botName = "DefaultScriptName";
    //Bot player Lv 
    public int botLv = 5;
    //Bot player Avatar 
    public int botAvatar = 1;
    
    //Bot player ID (always 2)
    [HideInInspector]
    public readonly int ID = 2;

    //Bot game team (always red)
    [HideInInspector]
    public readonly Team MyTeam = Team.Red;

    //Prefab Base Station (assign in inspector)
    public GameObject prefabBaseStation;

    //Prefabs Deck units (assign in inspector)
    public ShipsDataBase[] DeckUnits = new ShipsDataBase[8];
    
    //Current Energy
    [Range(0, 99)]
    public float CurrentEnergy = 30;

    //Max Energy
    [Range(0, 99)]
    public float MaxEnergy = 30;

    //Energy regeneration speed
    [Range(0, 99)]
    public float SpeedEnergy = 5;

    //Random Number created for the time between AI Spawn Units
    [Range(0, 3)]
    public float waitSpawn = 0.75f;

    //Delta time to make a decision
    WaitForSeconds IADelta;

    //Current own units in the game
    List<Unit> MyUnits;

    //NFTs data
    Dictionary<ShipsDataBase, NFTsUnit> DeckNfts;

    //Bot´s enemy base station
    Unit TargetUnit;

    //The cost of the most expensive card
    int MaxCostUnit;
    //The cost of the sheapest card
    int MinCostUnit;

    //Random class service
    private System.Random rng;

    //Allows to generate energy
    bool CanGenEnergy;
    private void Awake() { }
    // Start is called before the first frame update
    void Start()
    {
        //Init Basic variables
        IADelta = new WaitForSeconds(waitSpawn);
        MyUnits = new List<Unit>();
        CanGenEnergy = true;
        rng = new System.Random();

        //Add bot´s base station to bot's units list and set the bot´s enemy base station
        MyUnits.Add(GameMng.GM.Targets[0]);
        TargetUnit = GameMng.GM.Targets[1];

        //Init Deck Cards info with the units prefabs info
        DeckNfts = new Dictionary<ShipsDataBase, NFTsUnit>();
        for (int i = 0; i < DeckUnits.Length; i++)
        {
            if (DeckUnits[i] != null)
            {
                NFTsUnit nFTsCard = DeckUnits[i].ToNFTCard();
                GameMng.GM.AddNftCardData(nFTsCard, 2);
                DeckNfts.Add(DeckUnits[i], nFTsCard);
            }
        }

        //Set the max and min cost of the bot´s deck
        MaxCostUnit = DeckUnits.Max(f => f.cost);
        MinCostUnit = DeckUnits.Min(f => f.cost);

        //Start IA loop
        StartCoroutine(IA());
    }

    // Update is called once per frame
    void Update()
    {
        //Generate energy if the bot's can
        if (!CanGenEnergy)
            return;
        
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += Time.deltaTime * SpeedEnergy;
        }
        else if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }

    //Set if the bot can generate energy
    public void SetCanGenEnergy(bool can)
    {
        CanGenEnergy = can;
    }

    //IA Decision algoritm
    IEnumerator IA()
    {
        //Check if the game is not ended
        while(!GameMng.GM.IsGameOver())
        {
            yield return IADelta;//Add a delay time to think
            //Check if the game is not ended and the player base station still exist
            if (TargetUnit == null || GameMng.GM.IsGameOver())
            {
                break;
            }
            //Select first unit has default to spawn
            ShipsDataBase SelectedUnit = DeckUnits[0];
            //Mix game cards
            DeckUnits.OrderBy(r => rng.Next());
            //Select a unit depending the ia mode and current energy

            if (CurrentEnergy < MaxCostUnit)
            {
                continue;
            }
            
            for (int i = 0; i < 10; i++)
            {
                SelectedUnit = DeckUnits[Random.Range(0, DeckUnits.Length)];
                if (SelectedUnit.cost <= CurrentEnergy)
                {
                    break;
                }
            }

            //Check if the bot have enough energy
            if (SelectedUnit.cost <= CurrentEnergy && GameMng.GM.CountUnits(Team.Red) < 30)
            {
                //Select a random position (check the childs game objects of the bot)
                Vector3 PositionSpawn = transform.GetChild(Random.Range(0, transform.childCount)).position;

                //Spawn selected unit and rest energy
                Unit unit = GameMng.GM.CreateUnit(SelectedUnit.prefab,
                                                 PositionSpawn,
                                                 MyTeam,
                                                 DeckNfts[SelectedUnit].KeyId, 2);

                CurrentEnergy -= SelectedUnit.cost;
            }
        }
    }
}
}