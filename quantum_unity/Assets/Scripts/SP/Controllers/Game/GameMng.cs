using IngameDebugConsole;

namespace CosmicraftsSP {
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BotEnemy;

/*
 * This is the in-game main controller
 * Initialize the essential data and controls the game flow
 * Synchronize network data and connects the other controllers
 */

public class GameMng : MonoBehaviour
{
    //STATIC UNIQUE SYSTEMS CONTROLLERS
    public static GameMng GM;
    public static UIGameMng UI;
    public static Player P;
    public static GameMetrics MT;
    public static Config CONFIG;
    public static User PlayerData;
    public static UserProgress PlayerProgress;
    public static UserCollection PlayerCollection;
    public static NFTsCharacter PlayerCharacter;

    //OPTIONAL SYSTEMS CONTROLLERS
    [HideInInspector]
    public GameTutorial GT;
    [HideInInspector]
    public BotEnemy BOT;
    public GameObject BotPrefab;
    //Code of the base stations
    [HideInInspector]
    public Unit[] Targets;
    //Base stations initial positions (set in inspector)
    public Vector3[] BS_Positions;
    
    //Bg Maps
    public GameObject[] BgGameObjects;

    //Storage all game units and spells
    List<Unit> Units;
    List<Spell> Spells;
    //List of requested units for multiplayer
    List<NetUnitPack> RequestedUnits;
    //ID of the deleted and created units (necessary for multiplayer)
    List<int> DeletedUnits;
    List<int> CreatedUnits;
    //IDs auto increments for units
    int IdCounter;
    int IdRequestCounter;

    //Size of the map (set in inspector)
    public float MapWidth = 60;
    public float MapHeigth = 48;

    //Color Oultiners Ships (set in inspector)
    public Color ownUnitColor = Color.blue;
    public Color alliedUnitColor = Color.yellow;
    public Color enemyUnitColor = Color.red;
    
    //Game over status
    bool GameOver = false;
    //Team winner
    [HideInInspector]
    public Team Winner;
    //Set if the time out is running
    bool RunTime;

    //Time out controller
    TimeSpan TimeOut;
    //Date and time when the game begins
    DateTime StartTime;

    //clicks collider detection (necessary for spawn units)
    [HideInInspector]
    public BoxCollider GridColl;

    //Delta multiplayer refresh
    WaitForSeconds dnet;
    float dwebgl;

    //Dictionary of NFTs data of the units <NFTkey, NFTdata> (work in progress)
    Dictionary<string, NFTsCard> AllPlayersNfts;

    //Testing mode
    public bool Testing;
    public bool TestingDeck = false;
    
    bool InitRedy = false;

    private void Awake()
    {
        Debug.Log("--GAME MANAGER AWAKE--");
        //Manager is initielizing variables
        InitRedy = false;
        //Check for the global manager
        if (GlobalManager.GMD == null)
        {
            Instantiate(ResourcesServices.LoadGlobalManager());
            GlobalManager.GMD.CurrentMatch = Match.testing;
        }
        //Init static unique controllers
        GM = this;
        CONFIG = GlobalManager.GMD.GetConfig();
        PlayerData = GlobalManager.GMD.GetUserData();
        PlayerProgress = GlobalManager.GMD.GetUserProgress();
        PlayerCollection = GlobalManager.GMD.GetUserCollection();
        PlayerCharacter = GlobalManager.GMD.GetUserCharacter();
        //Check production
        if (!GlobalManager.GMD.DebugMode)
            Testing = false;
        //Check Default Player units
        if (Testing)
        {
            if (PlayerCollection.Deck == null)
                PlayerCollection.AddUnitsAndCharactersDefault();
        }
        //Delete debug Manager in production
        //if (!Debug.isDebugBuild)
        //{
        //    GameObject debugConsole = FindObjectOfType<DebugLogManager>().gameObject;
        //    if (debugConsole != null)
        //    {
        //        Destroy(debugConsole);
        //    }
        //}
        Debug.Log($"-- game mode:{GlobalManager.GMD.CurrentMatch}, testing: {Testing}, debug: {GlobalManager.GMD.DebugMode} --");
        Debug.Log("--GAME DEBUGS READY--");
        //init Basic variables and basic storages list
        GameOver = false;
        RunTime = !Testing;
        Units = new List<Unit>();
        Spells = new List<Spell>();
        RequestedUnits = new List<NetUnitPack>();
        DeletedUnits = new List<int>();
        CreatedUnits = new List<int>();
        dwebgl = 0.0f;
        Debug.Log("--GAME VARIABLES READY--");
        //Default Base Positions
        if (BS_Positions == null)
        {
            //From world reference
            BS_Positions = new Vector3[2];
            BS_Positions[0] = new Vector3(0, 0f, 0f);
            BS_Positions[1] = new Vector3(0f, 0f, -100f);
        }
        Debug.Log("--GAME POSITIONS READY--");
        //init metrics controller
        MT = new GameMetrics();
        MT.InitMetrics();
        Debug.Log("--GAME METRICS READY--");
        //Init players NFTs dictionary
        AllPlayersNfts = new Dictionary<string, NFTsCard>();
        //Init Ids counters
        IdCounter = IdRequestCounter = 0;
        //Init array of base stations
        Targets = new Unit[2];
        //Search Bot Prefab
        if (GlobalManager.GMD.CurrentMatch == Match.bots)
        {
            int mode = 0;
            int dificulty = 0;
            if (PlayerPrefs.HasKey("BotMode")) { mode = PlayerPrefs.GetInt("BotMode");}
            if (PlayerPrefs.HasKey("Dificulty")) { dificulty = PlayerPrefs.GetInt("Dificulty");}
            
            BotPrefab = ResourcesServices.LoadBot(mode, dificulty); Debug.Log($"Searching Bot_{mode}_{dificulty}");
            if (BotPrefab == null) { BotPrefab = ResourcesServices.LoadBot(0,0); Debug.Log($"Not found Bot_{mode}_{dificulty}"); }
            if (BotPrefab == null) { Debug.Log("Not found Bot Default"); }
        }

        //Set random BG
        foreach (GameObject bg in BgGameObjects) { bg.SetActive(false); }
        BgGameObjects[ UnityEngine.Random.Range(0, BgGameObjects.Length)].SetActive(true);
        
        Debug.Log("--GAME MANAGER END AWAKE--");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("--GAME MANAGER START--");
        //Set the time count down (5 minutes) and when the game begins
        TimeOut = new TimeSpan(0, 5, 0);
        StartTime = DateTime.Now;
        //Set Defautl Manager position
        transform.position = new Vector3(0f, 0f, 0f);
        //Set the size of the clicks and taps collider (the area where player can spawn cards)
        GridColl = GetComponentInChildren<BoxCollider>();
        GridColl.transform.localPosition = new Vector3(0f, 0f, 0f);
       // GridColl.size = new Vector3(MapWidth * f, 0.1f, MapHeigth * 2f);
        //Check which game mode was selected
        switch (GlobalManager.GMD.CurrentMatch)
        {
            case Match.bots: //VS IA
                {
                    //Instantiate BOT prefab
                    if (FindObjectOfType<BotEnemy>() == null)
                    {
                        BOT = Instantiate(BotPrefab).GetComponent<BotEnemy>();
                    }
                }
                break;
            case Match.tutorial: //TUTORIAL
                {
                    //Instantiate the game tutorial obj
                    GameObject gt = ResourcesServices.LoadTutorial();
                    GT = Instantiate(gt).GetComponent<GameTutorial>();
                    //Stop the time count down
                    RunTime = false;
                    UI.UpdateTimeOut("--");
                }
                break;
            case Match.multi: //MULTIPLAYER
                {
                    GameNetwork.JSGameStarts();
                    //IF IM THE MASTER...
                    if (GlobalManager.GMD.ImMaster)
                    {
                        //Add the client NFT data
                        List<NetCardNft> vsNFTs = GameNetwork.GetVSnftDeck();
                        if (vsNFTs != null)
                        {
                            int vsId = P.GetVsId();
                            foreach(NetCardNft card in vsNFTs)
                            {
                                AddNftCardData(new NFTsUnit()
                                {
                                    ID = card.ID,
                                    Name = card.Name,
                                    NameID = card.NameID,
                                    Description = card.Description,
                                    EntType = card.EntType,
                                    Faction = card.Faction,
                                    TypePrefix = NFTsCollection.NFTsPrefix[card.EntType],
                                    FactionPrefix = NFTsCollection.NFTsFactionsPrefixs[(Factions)card.Faction],
                                    Level = card.Level,
                                    LocalID = card.LocalID,
                                    Rarity = card.Rarity,
                                    Shield = card.Shield,
                                    Speed = card.Speed,
                                    EnergyCost = card.EnergyCost,
                                    HitPoints = card.HitPoints,
                                    Dammage = card.Dammage
                                }, vsId);
                            }
                            //Debug.Log($"--CLIENT DECK DONE ({vsNFTs.Count} cards)--");
                        }
                        //Set the delta time async (0.33 sec)
                        dnet = new WaitForSeconds(1f / 3f);
                        //Set and send the game status and game start datetime
                        GameNetwork.SetGameStart(DateTime.Now);
                        GameNetwork.SetGameStatus(NetGameStep.InGame);
                        SyncNetData();
                    } else //IF IM THE CLIENT
                    {
                        //Set the delta time async (5 sec)
                        dnet = new WaitForSeconds(5f);
                        //Set the base stations has fake (controlled by master data)
                        for (int i = 0; i < Targets.Length; i++)
                        {
                            Targets[i].setHasFake();
                        }
                    }
                    //Start the sync loop of multiplayer
                    StartCoroutine(LoopGameNetAsync());
                }
                break;
        }
        Debug.Log("--GAME MANAGER READY--");
    }

    // Update is called once per frame
    void Update()
    {
        //If game is already ended, don´t do nothing
        if (GameOver)
            return;

        //WEB GL DELAY SYNC
        if (dwebgl > 0f)
            dwebgl -= Time.deltaTime;

        //Time count down controller
        TimeControl();
    }

    public void TimeControl()
    {
        //Check if the time count down is active
        if (RunTime)
        {
            //Calculate the current time remaining
            TimeSpan CurrentTime = TimeOut.Add(StartTime - DateTime.Now);
            //Update the UI
            UI.UpdateTimeOut(CurrentTime.ToString(@"m\:ss"));
            //If time out...
            if (CurrentTime.TotalSeconds <= 0)
            {
                //Stop the count down
                RunTime = false;
                //Update UI
                UI.UpdateTimeOut("0:00");
                //Player and bot can´t generate energy
                P.SetCanGenEnergy(false);
                if (GlobalManager.GMD.CurrentMatch == Match.bots && BOT != null)
                {
                    BOT.SetCanGenEnergy(false);
                }
                //Check winner
                GameDraw();
            }
        }
    }

    //Enable or disable Count down
    public void EnableCountDown(bool enable)
    {
        RunTime = enable;
        if (!enable)
            UI.UpdateTimeOut(string.Empty);
    }

    //Get if the count down is enable
    public bool GetCountDownIsRuning()
    {
        return RunTime;
    }

    //Check winner
    void GameDraw()
    {
        //If im the client, don´t do nothing
        if (P.ImFake())
            return;

        //Check if the base stations have the same HP
        if (Targets[0].HitPoints == Targets[1].HitPoints)
        {
            //SAME HP, so the base stations can´t atack now, only the rest of units (final fight)
            Targets[0].GetComponent<Shooter>().StopAttack();
            Targets[1].GetComponent<Shooter>().StopAttack();
        }
        else if (Targets[0].HitPoints > Targets[1].HitPoints)
        {
            //WIN P2 IF P1 BASE STATION HAS LESS HP
            KillUnit(Targets[1]);
        }
        else
        {
            //WIN P1 IF P2 BASE STATION HAS LESS HP
            KillUnit(Targets[0]);
        }
    }

    //Get the transform enemy base station of X team
    public Transform GetFinalTransformTarget(Team team)
    {
        if (GameOver)
            return transform;

        return Targets[(int)team].transform;
    }

    //Get the transform enemy base station of X team
    public Vector3 GetDefaultTargetPosition(Team team)
    {
        return BS_Positions[(int)team];
    }
    
    //Get if the game is over
    public bool IsGameOver()
    {
        return GameOver;
    }
    
    //Get the remaining seconds of the game
    public int GetRemainingSecs()
    {
        return TimeOut.Add(StartTime - DateTime.Now).Seconds;
    }
    
    //Get a color for a unit depending the team and id player
    public Color GetColorUnit(Team team, int playerId)
    {
        //BLUE (MY OWN UNIT)
        //YELLOW (ALLIED UNIT)
        //RED (ENEMY UNIT)
        return P.MyTeam != team ? enemyUnitColor :
                    (P.ID == playerId ? ownUnitColor : alliedUnitColor);
    }
    
    //Create base stations (from player´s script)
    public void InitBaseStations()
    {
        //PLAYER STATION
        int PIn = P.MyTeam == Team.Blue ? 1 : 0; //Player base station index
        int VsIn = P.MyTeam == Team.Red ? 1 : 0; //Enemy base station index
        //Create the player´s base station
        Targets[PIn] = Instantiate(ResourcesServices.LoadBaseStationPrefab(PlayerCharacter.KeyId),
            BS_Positions[PIn], Quaternion.identity).GetComponent<Unit>();


        //Create the enemy´s base station
        GameObject VsStation = GlobalManager.GMD.CurrentMatch == Match.multi ?
            ResourcesServices.LoadBaseStationPrefab(GameNetwork.GetVSnftCharacter().KeyId ):
            BotPrefab.GetComponent<BotEnemy>().prefabBaseStation;
            
        Targets[VsIn] = Instantiate(VsStation, BS_Positions[VsIn], Quaternion.identity).GetComponent<Unit>();


        //Set variables for enemy´s base station
        Targets[0].PlayerId = 2;
        Targets[0].MyTeam = Team.Red;
        //Set the IDs of the base stations
        for (int i = 0; i < Targets.Length; i++)
        {
            Targets[i].setId(GenerateUnitId());
        }
        //Game is ready to start
        InitRedy = true;
    }
    
    //Deply a unit using a prefab game object
    public Unit CreateUnit(GameObject obj, Vector3 position, Team team, string nftKey = "none", int playerId = -1)
    {
        Unit unit = Instantiate(obj, position, Quaternion.identity).GetComponent<Unit>();
        unit.MyTeam = team;
        unit.PlayerId = playerId == -1 ? P.ID : playerId;
        unit.setId(GenerateUnitId());
        unit.SetNfts(GetNftCardData(nftKey, unit.PlayerId) as NFTsUnit);
        return unit;
    }
    
    //Deply a unit using a nft Key
    public Unit CreateUnit(string nftKey, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, false);
        if (obj == null)
            return null;
        return CreateUnit(obj, new Vector3(x, 0, z), (Team)team, nftKey, playerId);
    }
    
    //Deply a fake unit using a nft key (for multiplayer client)
    public Unit CreateFakeUnit(string nftKey, int Id, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, false);
        if (obj == null)
            return null;
        Unit unit = Instantiate(obj, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Unit>();
        unit.MyTeam = (Team)team;
        unit.PlayerId = playerId == -1 ? P.ID : playerId;
        unit.setId(Id);
        unit.SetNfts(null);
        unit.setHasFake();
        return unit;
    }
    
    //Cast a spell using a prefab game object
    public Spell CreateSpell(GameObject obj, Vector3 position, Team team, string nftKey = "none", int playerId = -1)
    {
        Spell spell = Instantiate(obj, position, Quaternion.identity).GetComponent<Spell>();
        spell.MyTeam = team;
        spell.PlayerId = playerId == -1 ? P.ID : playerId;
        spell.setId(GenerateUnitId());
        spell.SetNfts(GetNftCardData(nftKey, spell.PlayerId) as NFTsSpell);
        return spell;
    }
    
    //Cast a spell using a nft key
    public Spell CreateSpell(string nftKey, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, true);
        if (obj == null)
            return null;
        return CreateSpell(obj, new Vector3(x, 0, z), (Team)team, nftKey, playerId);
    }
    
    //Cast a fake spell using a nft key (for multiplayer client)
    public Spell CreateFakeSpell(string nftKey, int id, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, true);
        if (obj == null)
            return null;
        Spell spell = Instantiate(obj, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Spell>();
        spell.MyTeam = (Team)team;
        spell.PlayerId = playerId == -1 ? P.ID : playerId;
        spell.setId(id);
        spell.SetNfts(null);
        spell.setHasFake();
        return spell;
    }
    
    //Add a unit to the unit's list
    public void AddUnit(Unit unit)
    {
        if (!Units.Contains(unit))
        {
            Units.Add(unit);
        }
    }
    
    //Add a spell to the spell's list
    public void AddSpell(Spell spell)
    {
        if (!Spells.Contains(spell))
        {
            Spells.Add(spell);
        }
    }
    
    //Delete a unit to the unit's list
    public void DeleteUnit(Unit unit)
    {
        if (Units.Contains(unit))
        {
            Units.Remove(unit);
            DeletedUnits.Add(unit.getId());
        }
    }
    
    //Delete a spell to the spell's list
    public void DeleteSpell(Spell spell)
    {
        if (Spells.Contains(spell))
        {
            Spells.Remove(spell);
        }
    }
    
    //Kill a specific unit
    public void KillUnit(Unit unit)
    {
        if (unit != null)
            unit.Die();
    }
    
    //Request a deply of a specific unit or spell to the master (used by the client)
    public void RequestUnit(NetUnitPack unit)
    {
        RequestedUnits.Add(unit);
        GameNetwork.SetClientLastUpdate(DateTime.Now);
        GameNetwork.SetRequestedGameUnits(RequestedUnits);
        if (GlobalManager.GMD.IsProductionWeb())
        {
            GameNetwork.JSSendClientData(GameNetwork.GetJsonClientGameNetPack());
        }
    }
    
    //Returns how many units exist in the game
    public int CountUnits()
    {
        return Units.Count;
    }
    
    //Returns how many units of a team exist in the game
    public int CountUnits(Team team)
    {
        return Units.Where(f => f.IsMyTeam(team)).Count();
    }
    
    //Ends the game scene and returns the player to the main menu scene
    public void EndScene()
    {
        if (GlobalManager.GMD.CurrentMatch == Match.multi && GlobalManager.GMD.IsProductionWeb())
        {
            GameNetwork.JSExitGame();
        }

        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
    
    //Multiplayer data async loop (Send game data)
    IEnumerator LoopGameNetAsync()
    {
        while (true)
        {
            yield return dnet;//delta time
            SyncNetData();//Send game data
        }
    }

    //GAME OVER
    public void EndGame(Team winner)
    {
        //Set the team winner and end the game
        GameOver = true;
        Winner = winner;
        //Player has not control now
        P.SetInControl(false);
        //Set the first game has false
        //PlayerData.FirstGame = false;
        //Disable all the units
        foreach (Unit unit in Units)
        {
            unit.DisableUnit();
        }
        
        //Stop Couroutines
        StopAllCoroutines();
        //Show Game Over Screen
        StartCoroutine(ShowGameOver());
    }

    //Show game results
    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(0.1f);
        //Check if the game mode is multiplayer
        if (GlobalManager.GMD.CurrentMatch == Match.multi)
        {
            //if im master...
            if (GlobalManager.GMD.ImMaster)
            {
                //Set the winner in network and end the game in backend
                if (GameNetwork.GetWinner() == 0)
                {
                    GameNetwork.SetWinner(Winner == P.MyTeam ? 1 : 2);
                }
                GameNetwork.SetGameStatus(NetGameStep.Results);
                //Sync Winer
                SyncNetData();
            }
        }
        yield return new WaitForSeconds(0.1f);
        //Update UI
        UI.SetGameOver(Winner);
    }

    //Send game data
    public void SyncNetData()
    {
        //Master send main data
        if (GlobalManager.GMD.ImMaster)
        {
            //Prepare the units and spells data
            List<NetUnitPack> upack = Units.Select(s => new NetUnitPack
            {
                id = s.getId(),
                key = s.getKey(),
                pos_x = s.transform.position.x,
                pos_z = s.transform.position.z,
                rot_y = s.transform.rotation.eulerAngles.y,
                max_hp = s.GetMaxHitPoints(),
                max_sh = s.GetMaxShield(),
                hp = s.HitPoints,
                sh = s.Shield,
                team = (int)s.MyTeam,
                player_id = s.PlayerId,
                id_target = s.GetComponent<Shooter>() == null ? 0 : s.GetComponent<Shooter>().GetIdTarget()
            }).ToList();
            var spack = Spells.Select(s => new NetUnitPack
            {
                id = s.getId(),
                key = s.getKey(),
                pos_x = s.transform.position.x,
                pos_z = s.transform.position.z,
                rot_y = s.transform.rotation.eulerAngles.y,
                team = (int)s.MyTeam,
                player_id = s.PlayerId,
                is_spell = true
            });
            upack.AddRange(spack);
            //Make a game data package with currents entities and deleted entities
            GameNetwork.SetGameUnits(upack);
            GameNetwork.SetGameDeletedUnits(DeletedUnits);
            //Send metrics
            GameNetwork.SetMasterScore(MT.GetScore());
            //Set the last master comunication update
            GameNetwork.SetMasterLastUpdate(DateTime.Now);
            //Try to send the data to the back end
            try
            {
                //Send the data only if the game is runing has production on web
                if (GlobalManager.GMD.IsProductionWeb())
                {
                    //Send the data
                    GameNetwork.JSSendMasterData(GameNetwork.GetJsonGameNetPack());
                }
            }
            catch (Exception e)
            {
                //In case of error, log the error
                Debug.LogError($"CATCH ERROR: {e.Message}");
            }
        } else //Client send data
        {
            //Send metrics
            GameNetwork.SetClientScore(MT.GetScore());
            //Set the last client comunication update
            GameNetwork.SetClientLastUpdate(DateTime.Now);
            //Send the data only if the game is runing has production on web
            if (GlobalManager.GMD.IsProductionWeb())
            {
                //Send the data
                GameNetwork.JSSendClientData(GameNetwork.GetJsonClientGameNetPack());
            }
        }
    }
    
    //Sync the recived data from client (called from back end)
    public void GL_SyncMaster(string json)
    {
        if (dwebgl > 0f || GameOver)
            return;
        dwebgl = 0.3f;
        Debug.Log($"GL SYNC MASTER DATA (GAME ID {GameNetwork.GetId()})");
        //Check if data is redy
        if (!InitRedy)
            return;
        //Sync data if im master and data is not empty
        if (GlobalManager.GMD.ImMaster && !string.IsNullOrEmpty(json))
        {
            //Update local network data
            GameNetwork.UpdateClientGameData(json);
            //Unpackage the data
            List<NetUnitPack> UnitsRequested = GameNetwork.GetClientGameUnitsRequested();
            //Loop every  requested unit or spell
            if (UnitsRequested == null)
                return;
            foreach (NetUnitPack unit in UnitsRequested)
            {
                //Create real Unit or spell if doesn't exist already
                if (!CreatedUnits.Contains(unit.id))
                {
                    if (unit.is_spell)
                    {
                        CreateSpell(unit.key, unit.pos_x, unit.pos_z, P.GetVsTeamInt(), P.GetVsId());
                    } else
                    {
                        CreateUnit(unit.key, unit.pos_x, unit.pos_z, P.GetVsTeamInt(), P.GetVsId());
                    }
                    CreatedUnits.Add(unit.id);
                }
            }
        }
    }
    
    //Sync the recived data from master (called from back end)
    public void GL_SyncClient(string json)
    {
        if (dwebgl > 0f || GameOver)
            return;
        dwebgl = 0.3f;
        Debug.Log($"GL SYNC CLIENT DATA (GAME ID {GameNetwork.GetId()})");
        //Check if data is redy
        if (!InitRedy)
            return;
        //Sync if im the client
        if (!GlobalManager.GMD.ImMaster)
        {
            //Update local network data
            GameNetwork.UpdateGameData(json);
            //Set the start date time of the game
            StartTime = GameNetwork.GetStartTime();
            //Unpack units and spells data
            List<NetUnitPack> units = GameNetwork.GetGameUnits();
            //Unpack the deleted units and spells
            List<int> deleted = GameNetwork.GetGameUnitsDeleted();
            //Check For Units
            if (units != null)
            {
                //Loop every entitie
                foreach (NetUnitPack unit in units)
                {
                    //Check if the entitie is a spell
                    if (unit.is_spell)
                    {
                        //Check if the spell doesn´t exist already
                        Spell find = Spells.FirstOrDefault(f => f.getId() == unit.id);
                        //Create fake spell
                        if (find == null)
                        {
                            CreateFakeSpell(unit.key, unit.id, unit.pos_x, unit.pos_z, unit.team, unit.player_id);
                        }
                    }
                    else//The entitie is a unit
                    {
                        //Check if the unit doesn´t exist already
                        Unit find = Units.FirstOrDefault(f => f.getId() == unit.id);
                        //Create fake unit
                        if (find == null)
                        {
                            if (!string.IsNullOrEmpty(unit.key))
                            {
                                CreateFakeUnit(unit.key, unit.id, unit.pos_x, unit.pos_z, unit.team, unit.player_id);
                            }
                        }
                        else //Sync data if the unit exist
                        {
                            //Check if the unit is a ship (can move) or a station
                            Ship ship = find as Ship;
                            if (ship != null)
                            {
                                //The unit is a ship so set the destination from master data
                                ship.SetFakeDestination(new Vector3(unit.pos_x, 0f, unit.pos_z));
                            }
                            //Update metrics
                            if (!find.IsMyTeam(P.MyTeam) && unit.hp < find.HitPoints)
                            {
                                int hp_dif = find.HitPoints - unit.hp;
                                MT.AddDamage(hp_dif);
                            }
                            //Set the position, rotation and other variables from master data
                            find.SetFakeRotation(Quaternion.Euler(0f, unit.rot_y, 0f));
                            find.SetFakeHp(unit.hp, unit.max_hp);
                            find.SetFakeShield(unit.sh, unit.max_sh);
                            //Set the shooting target if the unit can attack
                            if (unit.id_target > 0)
                            {
                                Unit target = Units.FirstOrDefault(f => f.getId() == unit.id_target);
                                if (target != null)
                                {
                                    Shooter shooter = find.GetComponent<Shooter>();
                                    if (shooter != null)
                                    {
                                        shooter.SetFakeTarget(target);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Check the deleted units from master and delete the same local units
            if (deleted != null)
            {
                foreach (int unitId in deleted)
                {
                    Unit find = Units.FirstOrDefault(f => f.getId() == unitId);
                    if (find != null)
                    {
                        find.SetFakeHp(0, find.GetMaxHitPoints());
                        KillUnit(find);
                    }
                }
            }
            //Check if exist a default winner
            CheckMultiplayerWinner();
        }
    }
    
    //Sync winner data
    public void GL_SyncWinner(int winner)
    {
        //check if the game is not over but the back end has a winner
        if (!GameOver && winner != 0)
        {
            GameNetwork.SetWinner(winner);
            CheckMultiplayerWinner();
        }
    }
    
    //Check if some player wins the game
    //(used when some player disconets from the game to much time and the other player wins by defaut)
    public void CheckMultiplayerWinner()
    {
        //Only works for multiplayer game mode
        if (GlobalManager.GMD.CurrentMatch != Match.multi)
            return;
        //Get the winner from network data
        int winner = GameNetwork.GetWinner();
        //check if the game is not over but the back end has a winner
        if (!GameOver && winner != 0)
        {
            //The game has a winner
            if (winner == 1)
            {
                //The master wins the game so we destroy the client´s base station
                KillUnit(Targets[0]);
                return;
            }
            if (winner == 2)
            {
                //The client wins the game so we destroy the master´s base station
                KillUnit(Targets[1]);
                return;
            }
        }
    }
    
    //Returns if the main stations still exist
    public bool MainStationsExist()
    {
        return Targets[0] != null && Targets[1] != null;
    }
    
    //Generate an unic ID for units or spells
    public int GenerateUnitId()
    {
        IdCounter++;
        return IdCounter;
    }
    
    //Generate an unic ID for requested units or spells
    public int GenerateUnitRequestId()
    {
        IdRequestCounter++;
        return IdRequestCounter;
    }

    //Gets the NFT card data from player
    public NFTsCard GetNftCardData(string nftKey, int playerId)
    {
        string finalKey = $"{playerId}_{nftKey}";
        return AllPlayersNfts.ContainsKey(finalKey) ? AllPlayersNfts[finalKey] : null;
    }

    //Adds a new NFT card data for player
    public void AddNftCardData(NFTsCard nFTsCard, int playerId)
    {
        string finalKey = $"{playerId}_{nFTsCard.KeyId}";
        if (!AllPlayersNfts.ContainsKey(finalKey))
        {
            AllPlayersNfts.Add(finalKey, nFTsCard);
        }
    }
}
}