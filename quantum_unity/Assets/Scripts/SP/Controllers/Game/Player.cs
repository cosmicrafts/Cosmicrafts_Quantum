namespace CosmicraftsSP {
using System.Collections.Generic;
using UnityEngine;
/*
 ! This is the Player code
 ? Controls his energy, gameplay, deck, etc.
 * Contains the player data references and his ID and team on the game
 */
public class Player : MonoBehaviour
{
    [HideInInspector]
    public int ID = 1;
    [HideInInspector]
    public Team MyTeam = Team.Blue;
    bool InControl;
    bool CanGenEnergy;
    DragUnitCtrl UnitDrag;
    [HideInInspector]
    public Dictionary<string, GameObject> DeckUnits;

    [SerializeField] private GameObject characterPrefab;

    List<NFTsCard> PlayerDeck;
    Mesh[] UnitsMeshs;
    Material[] UnitMaterials;
    GameObject[] ShipPreviews;
    GameObject[] SpellPreviews;
    int DragingCard;
    int SelectedCard;
    GameCharacter MyCharacter;

    [Range(0, 99)]
    public float CurrentEnergy = 5;
    [Range(0, 99)]
    public float MaxEnergy = 10;
    [Range(0, 99)]
    public float SpeedEnergy = 1;
    public ShipsDataBase[] TestingDeck = new ShipsDataBase[8];
    KeyCode[] Keys;

private void Awake()
{
    Debug.Log("--PLAYER AWAKES--");
    GameMng.P = this;
    DeckUnits = new Dictionary<string, GameObject>();
    DragingCard = -1;
    SelectedCard = -1;

    GameObject basePrefabToUse = null;

    if (characterPrefab != null)
    {
        MyCharacter = Instantiate(characterPrefab, transform).GetComponent<GameCharacter>();

        if (MyCharacter.characterBaseSO != null && MyCharacter.characterBaseSO.BasePrefab != null)
        {
            basePrefabToUse = MyCharacter.characterBaseSO.BasePrefab;

            // Pass the prefab to GameMng and get the instantiated base station
            Unit baseStation = GameMng.GM.InitBaseStations(basePrefabToUse);

            // Apply overrides to the instantiated base station
            if (baseStation != null)
            {
                MyCharacter.characterBaseSO.ApplyOverridesToUnit(baseStation);
            }
        }
        else
        {
            Debug.LogError("CharacterBaseSO or BasePrefab is missing!");
        }
    }

    Debug.Log("--PLAYER END AWAKE--");
}

    private void Start()
    {
        Debug.Log("--PLAYER STARTS--");
        Keys = new KeyCode[8] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8 };
        UnitDrag = FindObjectOfType<DragUnitCtrl>();
        UnitDrag.setMeshActive(false);
        InControl = CanGenEnergy = true;

        // Load Deck from context as per your original logic
        if (GlobalManager.GMD.CurrentMatch == Match.tutorial)
        {
            if (GameMng.GM.GT != null)
            {
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
            PlayerDeck = new List<NFTsCard>();
            foreach (ShipsDataBase card in TestingDeck)
            {
                PlayerDeck.Add(card.ToNFTCard());
            }
        }
        else
        {
            PlayerDeck = GameMng.PlayerCollection.Deck;
        }

        SpellPreviews = new GameObject[8];
        ShipPreviews = new GameObject[8];
        UnitsMeshs = new Mesh[8];
        UnitMaterials = new Material[8];

        if (PlayerDeck.Count == 8)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject prefab = ResourcesServices.LoadCardPrefab(PlayerDeck[i].KeyId, PlayerDeck[i] as NFTsSpell != null);
                DeckUnits.Add(PlayerDeck[i].KeyId, prefab);
                GameMng.GM.AddNftCardData(PlayerDeck[i], ID);
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

        GameMng.UI.InitGameCards(PlayerDeck.ToArray());
        Debug.Log("--PLAYER END START--");
    }

    private void Update()
    {
        if (!InControl)
        {
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            if (Input.GetKeyDown(Keys[i]) && UnitDrag.IsValid())
                DeplyUnit(PlayerDeck[i]);
        }

        AddEnergy(Time.deltaTime * SpeedEnergy);
    }

    public bool ImFake()
    {
        return GlobalManager.GMD.CurrentMatch == Match.multi && ID == 2;
    }

    public void SelectCard(int idu)
    {
        if (!InControl)
        {
            return;
        }

        if (SelectedCard == idu)
        {
            SelectedCard = -1;
            GameMng.UI.DeselectCards();
            UnitDrag.setMeshActive(false);
        }
        else
        {
            GameMng.UI.DeselectCards();
            SelectedCard = idu;
            GameMng.UI.SelectCard(idu);
            if ((NFTClass)PlayerDeck[idu].EntType == NFTClass.Skill)
            {
                PrepareDeploy(SpellPreviews[idu], PlayerDeck[idu].EnergyCost);
            }
            else
            {
                PrepareDeploy(ShipPreviews[idu], PlayerDeck[idu].EnergyCost);
            }
        }
    }

    public void DragDeckUnit(int idu)
    {
        if (!InControl)
        {
            return;
        }

        DragingCard = idu;
        if (SelectedCard != -1 && SelectedCard != DragingCard)
        {
            SelectedCard = -1;
            GameMng.UI.DeselectCards();
        }

        if ((NFTClass)PlayerDeck[idu].EntType == NFTClass.Skill)
        {
            PrepareDeploy(SpellPreviews[idu], PlayerDeck[idu].EnergyCost);
        }
        else
        {
            PrepareDeploy(ShipPreviews[idu], PlayerDeck[idu].EnergyCost);
        }
    }

    public void DropDeckUnit()
    {
        if (!InControl)
        {
            return;
        }

        if (UnitDrag.IsValid() && (DragingCard != -1 || SelectedCard != -1))
        {
            DeplyUnit(DragingCard == -1 ? PlayerDeck[SelectedCard] : PlayerDeck[DragingCard]);
        }
        UnitDrag.setMeshActive(false);
        DragingCard = -1;
        SelectedCard = -1;
        GameMng.UI.DeselectCards();
    }

    public void SetInControl(bool incontrol)
    {
        InControl = incontrol;
        if (!InControl)
        {
            UnitDrag.setMeshActive(false);
            DragingCard = -1;
        }
    }

    public void SetCanGenEnergy(bool can)
    {
        CanGenEnergy = can;
    }

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

    public void RestEnergy(float value)
    {
        CurrentEnergy -= value;
        GameMng.MT.AddEnergyUsed(value);
        GameMng.UI.UpdateEnergy(CurrentEnergy, MaxEnergy);
    }

    public bool IsPreparingDeploy()
    {
        return DragingCard != -1 || SelectedCard != -1;
    }

    public void PrepareDeploy(Mesh mesh, Material mat, float cost)
    {
        UnitDrag.setMeshActive(true);
        UnitDrag.SetMeshAndTexture(mesh, mat);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }

    public void PrepareDeploy(GameObject preview, float cost)
    {
        UnitDrag.setMeshActive(false);
        UnitDrag.CreatePreviewObj(preview);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }

    public void DeplyUnit(NFTsCard nftcard)
    {
        if (nftcard.EnergyCost <= CurrentEnergy)
        {
            if ((NFTClass)nftcard.EntType != NFTClass.Skill)
            {
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
                }
                else
                {
                    Unit unit = GameMng.GM.CreateUnit(DeckUnits[nftcard.KeyId], CMath.GetMouseWorldPos(), MyTeam, nftcard.KeyId);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeployUnit(unit);
                    }
                }
                RestEnergy(nftcard.EnergyCost);
                GameMng.MT.AddDeploys(1);
            }
            else
            {
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
                else
                {
                    Spell spell = GameMng.GM.CreateSpell(DeckUnits[nftcard.KeyId], CMath.GetMouseWorldPos(), MyTeam, nftcard.KeyId);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeploySpell(spell);
                    }
                }
                RestEnergy(nftcard.EnergyCost);
            }
        }
    }

    public int GetVsTeamInt()
    {
        return MyTeam == Team.Red ? 0 : 1;
    }

    public Team GetVsTeam()
    {
        return MyTeam == Team.Red ? Team.Blue : Team.Red;
    }

    public int GetVsId()
    {
        return ID == 1 ? 2 : 1;
    }
}
}
