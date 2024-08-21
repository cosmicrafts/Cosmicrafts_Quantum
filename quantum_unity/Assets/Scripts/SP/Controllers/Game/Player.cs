namespace CosmicraftsSP {
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 ! This is the Player code
 ? Controls his energy, gameplay, deck, etc.
 * Contains the player data references and his ID and team on the game
 */
public class Player : MonoBehaviour
{
    //The ID of the player (or index)
    [HideInInspector]
    public int ID = 1;
    //The player´s team
    [HideInInspector]
    public Team MyTeam = Team.Blue;
    //Enables or disables the gameplay
    bool InControl;
    //Allows to generate energy
    bool CanGenEnergy;
    //Reference the drag and drop object
    DragUnitCtrl UnitDrag;
    //Stores the deck units prefabs
    [HideInInspector]
    public Dictionary<string, GameObject> DeckUnits;

    // Character to be assigned in the inspector
    [SerializeField] private GameObject characterPrefab;

    // Base station prefab to be assigned in the inspector
    [SerializeField] private GameObject baseStationPrefab;

    //Deck
    List<NFTsCard> PlayerDeck;
    //Stores the meshes and materials of the units, for drag and drop previews
    Mesh[] UnitsMeshs;
    Material[] UnitMaterials;
    GameObject[] ShipPreviews;
    //Stores the object with particules of the spells, for drag and drop previews
    GameObject[] SpellPreviews;
    //Reference the current dragin card
    int DragingCard;
    //Reference the current selected card
    int SelectedCard;
    //Reference the player´s character
    GameCharacter MyCharacter;

    //Curent energy
    [Range(0, 99)]
    public float CurrentEnergy = 5;
    //Max energy
    [Range(0, 99)]
    public float MaxEnergy = 10;
    //Energy regeneration speed 
    [Range(0, 99)]
    public float SpeedEnergy = 1;
    //Testing Deck
    public ShipsDataBase[] TestingDeck = new ShipsDataBase[8];
    //Hot Keys
    KeyCode[] Keys;

    private void Awake()
    {
        Debug.Log("--PLAYER AWAKES--");
        //Defines the current Player controler to the game manager
        GameMng.P = this;
        //Init Deck Units Array
        DeckUnits = new Dictionary<string, GameObject>();
        //Init defaults
        DragingCard = -1;
        SelectedCard = -1;

        // Assign the character and base station
        if (characterPrefab != null)
        {
            MyCharacter = Instantiate(characterPrefab, transform).GetComponent<GameCharacter>();
        }

        // Initialize base stations using the assigned base station prefab
        GameMng.GM.InitBaseStations(baseStationPrefab);

        // Rest of your initialization code...
        Debug.Log("--PLAYER END AWAKE--");
    }

    private void Start()
    {
        Debug.Log("--PLAYER STARTS--");
        //Init key codes
        Keys = new KeyCode[8] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8};
        //Get draging cards controller
        UnitDrag = FindObjectOfType<DragUnitCtrl>();
        UnitDrag.setMeshActive(false);
        //Enable the gameplay and the energy generation
        InControl = CanGenEnergy = true;

        //Load Deck from context
        if (GlobalManager.GMD.CurrentMatch == Match.tutorial)
        {
            if (GameMng.GM.GT != null)
            {
                //Load Tutorial NFTs Deck
                PlayerDeck = new List<NFTsCard>();
                foreach (ShipsDataBase card in GameMng.GM.GT.DeckUnits)
                {
                    PlayerDeck.Add(card.ToNFTCard());
                }
            }
            else
            {
                PlayerDeck = GameMng.PlayerCollection.Deck;
            }
        }
        else if (GameMng.GM.TestingDeck)
        {
            //Load Testing NFTs Deck
            PlayerDeck = new List<NFTsCard>();
            foreach (ShipsDataBase card in TestingDeck)
            {
                PlayerDeck.Add(card.ToNFTCard());
            }
        }
        else
        {
            //Get the player´s collection deck
            PlayerDeck = GameMng.PlayerCollection.Deck;
        }

        //Init and set the units data arrays
        SpellPreviews = new GameObject[8];
        ShipPreviews = new GameObject[8];
        UnitsMeshs = new Mesh[8];
        UnitMaterials = new Material[8];

        //Load Nfts prefabs and prepare in game deck
        if (PlayerDeck.Count == 8)
        {
            for (int i = 0; i < 8; i++)
            {
                //Load unit prefabs from nft data
                GameObject prefab = ResourcesServices.LoadCardPrefab(PlayerDeck[i].KeyId, PlayerDeck[i] as NFTsSpell != null);
                DeckUnits.Add(PlayerDeck[i].KeyId, prefab);
                //Save nft data on manager
                GameMng.GM.AddNftCardData(PlayerDeck[i], ID);
                //Load models
                GameCard card = prefab.GetComponent<GameCard>();

                if ((NFTClass)PlayerDeck[i].EntType == NFTClass.Skill)
                {
                    SpellCard spell = card as SpellCard;
                    SpellPreviews[i] = spell.PreviewEffect;

                }
                else
                {
                    UnitCard unit = card as UnitCard;
                    ShipPreviews[i] = unit.UnitMesh;
                    UnitsMeshs[i] = unit.UnitMesh.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
                    UnitMaterials[i] = unit.UnitMesh.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
                }
            }
        }

        //Update the UI info
        GameMng.UI.InitGameCards(PlayerDeck.ToArray());
        Debug.Log("--PLAYER END START--");
    }
    

    private void Update()
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }
        //Hot Keys
        for (int i = 0; i<8; i++)
        {
            if (Input.GetKeyDown(Keys[i]) && UnitDrag.IsValid())
                DeplyUnit(PlayerDeck[i]);
        }
        //Add energy over time
        AddEnergy(Time.deltaTime * SpeedEnergy);
        //Testing spawn random unit
        //if (Input.GetKeyDown(KeyCode.R) && GameMng.GM.Testing)
        //{
        //    string faction = Random.Range(1, 2) == 1 ? "ALL" : "SPI";
        //    GameMng.GM.CreateUnit($"U_{faction}_{Random.Range(1, 8)}",
        //        Random.Range(-30f, 30f), Random.Range(-24f, 24f), Random.Range(1, -1));
        //    //GameMng.UI.Players[0].gameObject.SetActive(false);
        //}
    }

    //Check if im the client
    public bool ImFake()
    {
        return GlobalManager.GMD.CurrentMatch == Match.multi && ID == 2;
    }

    //Select a card (from index 0 - 7) 
    public void SelectCard(int idu)
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }

        //If the card is already selected, deselect the card
        if (SelectedCard == idu)
        {
            SelectedCard = -1;
            GameMng.UI.DeselectCards();
            UnitDrag.setMeshActive(false);
        } else //Ii the card is not selected, select the card
        {
            GameMng.UI.DeselectCards();
            SelectedCard = idu;
            GameMng.UI.SelectCard(idu);
            //Show the preview of the card on the cursor
            if ((NFTClass)PlayerDeck[idu].EntType == NFTClass.Skill)
            {
                PrepareDeploy(SpellPreviews[idu], PlayerDeck[idu].EnergyCost);
            }
            else
            {
                PrepareDeploy(ShipPreviews[idu], PlayerDeck[idu].EnergyCost);
                //repareDeploy(UnitsMeshs[idu], UnitMaterials[idu], PlayerDeck[idu].EnergyCost);
            }
        }
    }

    //Start dragin a card
    public void DragDeckUnit(int idu)
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }

        //set the dragin card
        DragingCard = idu;
        //If the player has a selected card and is not this card, deselect the card
        if (SelectedCard != -1 && SelectedCard != DragingCard)
        {
            SelectedCard = -1;
            GameMng.UI.DeselectCards();
        }

        //Show the preview of the card on the cursor
        if ((NFTClass)PlayerDeck[idu].EntType == NFTClass.Skill)
        {
            PrepareDeploy(SpellPreviews[idu], PlayerDeck[idu].EnergyCost);
        }
        else
        {
            
            PrepareDeploy(ShipPreviews[idu], PlayerDeck[idu].EnergyCost);
             //PrepareDeploy(UnitsMeshs[idu], UnitMaterials[idu], PlayerDeck[idu].EnergyCost);
        }
    }

    //Drop the dragin card
    public void DropDeckUnit()
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }

        //Check if the player is dragin a card or if he has a selected card
        //Also check if the dragin card is on a valid spawn area
        if (UnitDrag.IsValid() && (DragingCard != -1 || SelectedCard != -1))
        {
            //Deply the unit
            DeplyUnit(DragingCard == -1 ? PlayerDeck[SelectedCard] : PlayerDeck[DragingCard]);
        }
        //Clear the selection and the dragin object controller
        UnitDrag.setMeshActive(false);
        DragingCard = -1;
        SelectedCard = -1;
        GameMng.UI.DeselectCards();
    }

    //Enables or disables the gameplay
    public void SetInControl(bool incontrol)
    {
        InControl = incontrol;
        if (!InControl)
        {
            UnitDrag.setMeshActive(false);
            DragingCard = -1;
        }
    }
    //Set if the player can generates energy
    public void SetCanGenEnergy(bool can)
    {
        CanGenEnergy = can;
    }
    //Add energy
    public void AddEnergy(float value)
    {
        if (!CanGenEnergy)
            return;

        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += value;
            GameMng.MT.AddEnergyGenerated(value);
        }
        else if (CurrentEnergy >= MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
            GameMng.MT.AddEnergyWasted(value);
        }
        GameMng.UI.UpdateEnergy(CurrentEnergy, MaxEnergy);
    }
    //Reduce energy
    public void RestEnergy(float value)
    {
        CurrentEnergy -= value;
        GameMng.MT.AddEnergyUsed(value);
        GameMng.UI.UpdateEnergy(CurrentEnergy, MaxEnergy);
    }
    //Returns if the player is dragin or selecting a card
    public bool IsPreparingDeploy()
    {
        return DragingCard != -1 || SelectedCard != -1;
    }
    //Shows the dragin card whit the specific mesh and material
    public void PrepareDeploy(Mesh mesh, Material mat, float cost)
    {
        UnitDrag.setMeshActive(true);
        UnitDrag.SetMeshAndTexture(mesh, mat);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }
    //Shows the dragin card whit a game object has preview
    public void PrepareDeploy(GameObject preview, float cost)
    {
        UnitDrag.setMeshActive(false);
        UnitDrag.CreatePreviewObj(preview);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }
    //Deploy a spell or unit
    public void DeplyUnit(NFTsCard nftcard)
    {
        //Check the card cost and current plkayer´s energy
        if (nftcard.EnergyCost <= CurrentEnergy)
        {
            //Check if the card is a SHIP OR STATION
            if ((NFTClass)nftcard.EntType != NFTClass.Skill)
            {
                //If im am the client, send a request to deploy
                if (ImFake())
                {
                    Vector3 position = CMath.GetMouseWorldPos();
                    NetUnitPack unitdata = new NetUnitPack()
                    {
                        id = GameMng.GM.GenerateUnitRequestId(),
                        key = nftcard.KeyId,
                        pos_x = position.x,
                        pos_z = position.z,
                        is_spell = false
                    };
                    GameMng.GM.RequestUnit(unitdata);
                } else //Im not the client, so is a commun deply
                {
                    Unit unit = GameMng.GM.CreateUnit(DeckUnits[nftcard.KeyId], CMath.GetMouseWorldPos(), MyTeam, nftcard.KeyId);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeployUnit(unit);
                    }
                }
                //Reduce the energy
                RestEnergy(nftcard.EnergyCost);
                GameMng.MT.AddDeploys(1);
            } else //THE CARD IS A SPELL
            {
                //If im am the client, send a request to deploy
                if (ImFake())
                {
                    Vector3 position = CMath.GetMouseWorldPos();
                    NetUnitPack unitdata = new NetUnitPack()
                    {
                        id = GameMng.GM.GenerateUnitRequestId(),
                        key = nftcard.KeyId,
                        pos_x = position.x,
                        pos_z = position.z,
                        is_spell = true
                    };
                    GameMng.GM.RequestUnit(unitdata);
                }
                else //Im not the client, so is a commun deply
                {
                    Spell spell = GameMng.GM.CreateSpell(DeckUnits[nftcard.KeyId], CMath.GetMouseWorldPos(), MyTeam, nftcard.KeyId);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeploySpell(spell);
                    }
                }
                //Reduce the energy
                RestEnergy(nftcard.EnergyCost);
            }
        }
    }
    //Returns the team of the enemy as int
    public int GetVsTeamInt()
    {
        return MyTeam == Team.Red ? 0 : 1;
    }
    //Returns the team of the enemy
    public Team GetVsTeam()
    {
        return MyTeam == Team.Red ? Team.Blue : Team.Red;
    }
    //Returns the ID of the enemy
    public int GetVsId()
    {
        return ID == 1 ? 2 : 1;
    }
}
}