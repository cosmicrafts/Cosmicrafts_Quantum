namespace CosmicraftsSP {
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =("Nueva Nave") ,menuName =("Crear Nueva Nave"))]
public class ShipsDataBase : ScriptableObject
{

    #region DataBase

    //----Prefab-----------------------------------------------------------
    [Tooltip("Prefab asociado")]
    [Header("Prefab")]
    [SerializeField]
    protected GameObject Prefab;

    //----Ship´s name-----------------------------------------------------------
    [Tooltip("Informa de la Nombre de la carta")]
    [Header("Nombre de la unidad")]
    [SerializeField]
    protected string Name;

    //----Nft local ID-----------------------------------------------------------
    [Tooltip("ID local del nft")]
    [Header("ID Local")]
    [SerializeField]
    protected int LocalID;

    //----Nft faction-----------------------------------------------------------
    [Tooltip("Faccion del Nft")]
    [Header("Faccion")]
    [SerializeField]
    protected Factions Faction;

    //----Nft card type-----------------------------------------------------------
    [Tooltip("Tipo de NFT")]
    [Header("Tipo de Nft")]
    [SerializeField]
    protected NFTClass NftType;

    //----Unit HP-----------------------------------------------------------
    [Tooltip("Puntos de vida de la unidad")]
    [Header("HP")]
    [Range(1, 9999)]
    [SerializeField]
    protected int HitPoints;

    //----Unit Shield-----------------------------------------------------------
    [Tooltip("Puntos de escudo de la unidad")]
    [Header("Escudo")]
    [SerializeField]
    [Range(1, 9999)]
    protected int Shield;

    //----Unit card cost-----------------------------------------------------------
    [Tooltip("Costo de energia de la unidad")]
    [Header("Asignar el costo de la unidad")]
    [SerializeField]
    [Range(1, 9999)]
    protected int EnergyCost;

    //----Unit Damage-----------------------------------------------------------
    [Tooltip("Puntos de daño por bala de la unidad")]
    [Header("Daño")]
    [SerializeField]
    [Range(0, 9999)]
    protected int Dammage;

    //----Unit Speed-----------------------------------------------------------
    [Tooltip("Velocidad de la nave")]
    [Header("Velocidad de movimiento")]
    [SerializeField]
    [Range(0, 99)]
    protected float Speed;

    #endregion

    #region Variables de Lectura

    public GameObject prefab => Prefab;

    public string cardName => Name;

    public int localId => LocalID;

    public int faction => (int)Faction;

    public int type => (int)NftType;

    public int hp => HitPoints;

    public int shield => Shield;

    public int cost => EnergyCost;

    public int dmg => Dammage;

    public float speed => Speed;

    public NFTsUnit ToNFTCard()
    {
        NFTsUnit nFTsCard = new NFTsUnit()
        {
            EnergyCost = cost,
            HitPoints = hp,
            Shield = shield,
            Speed = speed,
            Dammage = dmg,
            Faction = faction,
            EntType = type,
            LocalID = localId,
            TypePrefix = NFTsCollection.NFTsPrefix[type],
            FactionPrefix = NFTsCollection.NFTsFactionsPrefixs[(Factions)faction],
        };
        nFTsCard.IconSprite = ResourcesServices.LoadCardIcon(nFTsCard.KeyId);
        return nFTsCard;
    }

    #endregion
}
}