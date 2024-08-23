namespace CosmicraftsSP {
    using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * This is the Tutorial controller
 * Controls the events in the game and the character´s dialogs
 * Limits the player to do only specific things
 */

public class GameTutorial : MonoBehaviour
{
    //Enum with the diferents status of the tutorial
    public enum tevent
    {
        dialog,
        tooltip,
        objective
    }

    //Deck Units for the player
    public ShipsDataBase[] DeckUnits = new ShipsDataBase[8];
    //UI cards references
    UIGameCard[] GameCards;
    //UI dialogs game object
    public GameObject Dialog;
    //UI image of the speaking character
    public Image IconCharacter;
    //UI text for dialogs
    public Text DialogText;

    //Characters speed of the typewriter effect
    [Range(1, 120)]
    public int CharactersPerSec = 30;

    //List of events (game flow)
    tevent[] Events = new tevent[23]
    {
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.tooltip,
        tevent.tooltip,
        tevent.tooltip,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.tooltip,
        tevent.objective,
        tevent.dialog,
        tevent.objective,
        tevent.dialog,
        tevent.objective,
        tevent.tooltip,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.tooltip
    };
    //Array with each dialog of the tutorial (synchronized with the events)
    string[] Dialogs;

    //Array with each sprite for each dialog (synchronized with the dialogs)
    public Sprite[] Characters = new Sprite[23];
    //Array with the reference of each tool tip in the game
    public GameObject[] Tips = new GameObject[5];
    //Array with the reference of each objective of the game
    public Text[] Objectives = new Text[3];
    //Reference of the alert object (used when the tutorial updates the objective)
    public GameObject ObjectiveAlert;

    //Reference of the parent object with the alliad units
    public GameObject AliadUnits;
    //Reference of the parent object with the enemy units
    public GameObject EnemyUnits;

    //The partial current string dialog (showing in the ui)
    string CurrentString;
    //The complete current string dialog (necessary for the typewriter effect)
    string TargetString;

    //Current index character of the TargetString
    int ichar;
    //Current index event of the tutorial
    int ievent;
    //Time speed of the typewriter effect
    float delaychar;

    //The showing time of mission update alert
    WaitForSeconds alertDelay;
    //The delayed spawn time between every unit (when all the units appear)
    WaitForSeconds deployDelay;

    //The enemy's base station
    Unit EnemyBaseStation;

    // Start is called before the first frame update
    void Start()
    {
        //Init all the dialogs
        InitDialogs();
        //Set te current and target string
        CurrentString = string.Empty;
        TargetString = Dialogs[0];
        //Start all de indexes at 0
        ichar = 0;
        ievent = 0;
        //Calculate the speed of the typewriter effect
        delaychar = 1f / (float)CharactersPerSec;
        //Desactive all the ui cards and then active only the first one
        GameCards = GameMng.UI.UIDeck;
        foreach (UIGameCard card in GameCards)
        {
            card.gameObject.SetActive(false);
        }
        GameCards[0].gameObject.SetActive(true);
        //Set the player energy and disable the gameplay
        GameMng.P.CurrentEnergy = 10;
        GameMng.P.SetInControl(false);
        //Set all the base station has invincibles
        GameMng.GM.Targets[1].IsInmortal = true;
        EnemyBaseStation = GameMng.GM.Targets[0];
        EnemyBaseStation.IsInmortal = true;
        //Set the time delays for alerts and massive deploys
        alertDelay = new WaitForSeconds(3f);
        deployDelay = new WaitForSeconds(0.5f);
        //Show the first objective and the mission update alert
        Objectives[0].gameObject.SetActive(true);
        StartCoroutine(ShowAlert());
    }

    void InitDialogs()
    {
        //Get and save the dialog's traductions
        Dialogs = new string[23]
        {
            Lang.GetText("arg_tut_01"),
            Lang.GetText("arg_tut_02"),
            Lang.GetText("arg_tut_03"),
            string.Empty,
            string.Empty,
            string.Empty,
            Lang.GetText("arg_tut_04"),
            Lang.GetText("arg_tut_05"),
            Lang.GetText("arg_tut_06"),
            Lang.GetText("arg_tut_07"),
            Lang.GetText("arg_tut_08"),
            Lang.GetText("arg_tut_09"),
            string.Empty,
            string.Empty,
            Lang.GetText("arg_tut_10"),
            string.Empty,
            Lang.GetText("arg_tut_11"),
            string.Empty,
            string.Empty,
            Lang.GetText("arg_tut_12"),
            Lang.GetText("arg_tut_13"),
            Lang.GetText("arg_tut_14"),
            string.Empty,
        };
        //Set the name of the player in dialogs
        for (int i = 0; i < Dialogs.Length; i++)
        {
          //  Dialogs[i] = Dialogs[i].Replace("$PLAYER", GameMng.PlayerData.NikeName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Delay the time to add a new character
        if (delaychar > 0)
        {
            delaychar -= Time.deltaTime;
        }
        else //the delays ends
        {
            //Reset the delay
            delaychar = 1f / (float)CharactersPerSec;
            //Check is necessary add a new character
            if (ichar < TargetString.Length)
            {
                //Add a new character
                ichar++;
                CurrentString = TargetString.Substring(0, ichar);
            }
            //Check the status of the current state
            CheckForObjectives();
        }
        //Update the ui with the dialog's text
        DialogText.text = CurrentString;
    }

    //Called when the player wants to go next dialog
    public void NextDialog()
    {
        //Chech if are more events
        if (ievent < Events.Length)
        {
            //If the current dialog in ui is complete, go for the next event
            if (ichar >= TargetString.Length)
            {
                GoNextEvent();
            }
            else //If the current dialog is uncomplete, complete the dialogo
            {
                ichar = TargetString.Length;
                CurrentString = TargetString;
            }
        }
    }

    //Check and set the current status of the game
    void CheckForEvents()
    {
        switch (ievent)
        {
            case 3: //Show the first tip
                {
                    Tips[0].SetActive(true);
                    GameMng.P.SetInControl(true);
                    Objectives[0].color = Color.gray;
                    Objectives[1].gameObject.SetActive(true);
                    StartCoroutine(ShowAlert());
                }
                break;
            case 4: //Show the 2° tip
                {
                    Tips[1].SetActive(true);
                }
                break;
            case 5: //Show the 3° tip
                {
                    Tips[2].SetActive(true);
                }
                break;
            case 12: //Show the 4° tip, show the 2° card and update the objective
                {
                    Tips[3].SetActive(true);
                    GameCards[1].gameObject.SetActive(true);
                    Objectives[1].color = Color.gray;
                    Objectives[2].gameObject.SetActive(true);
                    StartCoroutine(ShowAlert());
                }
                break;
            case 15: //Show the Enemy fleet
                {
                    StartCoroutine(DeployEnnemyFleet());
                }
                break;
            case 17: //Show the alliad fleet
                {
                    StartCoroutine(DeployAlliadFleet());
                }
                break; //Show the rest of the cards
            case 18:
                {
                    GameCards[2].gameObject.SetActive(true);
                    GameCards[3].gameObject.SetActive(true);
                }
                break;
            case 19: //Stop the battle
                {
                    Unit[] units = FindObjectsOfType<Unit>();

                    foreach (Unit unit in units)
                    {
                        unit.DisableUnit();
                    }
                    Objectives[2].color = Color.gray;
                }
                break;
            case 22: //Show the congratulations
                {
                    GameMng.GM.EndGame(Team.Blue);

                    foreach (Text objective in Objectives)
                    {
                        objective.gameObject.SetActive(false);
                    }

                    Animator anim = EnemyBaseStation.GetAnimator();
                    anim.speed = 0;
                }
                break;
        }
    }

    //Check if the current event is finished or if the player completes the objective
    void CheckForObjectives()
    {
        switch (ievent)
        {
            case 3: //Check if the player is preparing a deploy
                {
                    if (GameMng.P.IsPreparingDeploy())
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 4: //Check if the player deployed a unit
                {
                    if (GameMng.MT.GetDeploys() > 0)
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 5: //Check if the player deployed 2 or more unit
                {
                    if (GameMng.MT.GetDeploys() > 1)
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 12: //Check if the player deployed 3 or more unit
                {
                    if (GameMng.MT.GetDeploys() > 3)
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 13: //Check if the enemy base stations has less than %50 hp
                {
                    if (EnemyBaseStation.HitPoints < EnemyBaseStation.GetMaxHitPoints() / 2)
                    {
                        Unit[] units = FindObjectsOfType<Unit>();

                        foreach (Unit unit in units)
                        {
                            unit.DisableUnit();
                        }
                        EnemyBaseStation.HitPoints = EnemyBaseStation.GetMaxHitPoints() / 2;
                        EnemyBaseStation.Shield = EnemyBaseStation.GetMaxShield();
                        GoNextEvent();
                    }
                }
                break;
            case 18: //Check if the enemy base stations has less than %25 hp
                {
                    if (EnemyBaseStation.HitPoints < EnemyBaseStation.GetMaxHitPoints() / 4)
                    {
                        GoNextEvent();
                    }
                }
                break;
        }
    }
    //Move to the next event
    void GoNextEvent()
    {
        //Restart the typewriter effect
        ievent++;
        ichar = 0;
        CurrentString = string.Empty;
        //Set the next dialog and character image
        TargetString = Dialogs[ievent];
        IconCharacter.sprite = Characters[ievent];
        //Hide all the tips
        foreach (GameObject tip in Tips)
        {
            tip.SetActive(false);
        }
        //Active the dialog's box if the next event demands it
        Dialog.SetActive(Events[ievent] == tevent.dialog);
        //Active the gameplay if the next event demands it
        GameMng.P.SetInControl(Events[ievent] != tevent.dialog);

        CheckForEvents();
    }

    //Show an objective update alert, then hide it after x time
    IEnumerator ShowAlert()
    {
        ObjectiveAlert.SetActive(true);
        yield return alertDelay;
        ObjectiveAlert.SetActive(false);
    }

    //Deploy the enemy fleet and go for the next event
    IEnumerator DeployEnnemyFleet()
    {
        GameMng.P.SetInControl(false);

        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            unit.DisableUnit();
        }

        foreach (Transform child in EnemyUnits.transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Unit>().DisableUnit();
            yield return deployDelay;
        }

        GoNextEvent();
    }

    //Deploy the alliad fleet
    IEnumerator DeployAlliadFleet()
    {
        GameMng.P.SetInControl(false);

        foreach (Transform child in AliadUnits.transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Unit>().DisableUnit();
            yield return deployDelay;
        }

        GoNextEvent();

        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            unit.EnableUnit();
        }
    }

    //Ends the scene and returns to menu
    public void EndTutorial()
    {
        SceneManager.LoadScene(1);
    }
}
}