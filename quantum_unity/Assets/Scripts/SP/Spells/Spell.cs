namespace CosmicraftsSP {
using UnityEngine;
/*
    This is the in-game parent spell controller
 */
public class Spell : MonoBehaviour
{
    //Defines if this spell is only a mirror of other spell (used for multiplayer)
    protected bool IsFake = false;
    //The NFT data source
    protected NFTsSpell NFTs;
    //The spell's team in the game
    public Team MyTeam;
    //The owner of this spell
    public int PlayerId = 1;
    //The spell ID in the game
    protected int Id;

    //Duration of the spell, before die
    [Range(0, 300)]
    public float Duration = 1;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Save the reference in the game manager
        GameMng.GM.AddSpell(this);
        //Destroy after duration
        if (Duration > 0)
        {
            Destroy(gameObject, Duration);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    //Sets if this spell is fake
    public virtual void setHasFake()
    {
        IsFake = true;
    }

    //Returns the NFT key
    public string getKey()
    {
        return NFTs.KeyId;
    }

    //Sets the ID of the spell
    public void setId(int id)
    {
        Id = id;
    }

    //Returns the ID of the spell
    public int getId()
    {
        return Id;
    }

    //When destroys, delete the reference on the game manager
    private void OnDestroy()
    {
        if (GameMng.GM != null)
        {
            GameMng.GM.DeleteSpell(this);
        }
    }

    //Sets the NFT data source
    public virtual void SetNfts(NFTsSpell nFTsSpell)
    {
        NFTs = nFTsSpell;

        if (GlobalManager.GMD.DebugMode || nFTsSpell == null)
            return;
    }
}
}