//This is the master NFT class
using UnityEngine;

public abstract class NFTs
{
    public int ID { get; set; }

    public string NameID { get; set; }

    public virtual string KeyId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string IconURL { get; set; }

    public Sprite IconSprite { get; set; }

    public int Rarity { get; set; }

    public int Faction { get; set; }

    public string FactionPrefix { get; set; }

    public int Level { get; set; }

    public int LocalID { get; set; }

    public int EntType { get; set; }

    public char TypePrefix { get; set; }
}
