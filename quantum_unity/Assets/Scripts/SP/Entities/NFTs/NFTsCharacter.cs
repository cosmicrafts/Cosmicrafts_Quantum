//NFT character class

using UnityEngine;

public class NFTsCharacter : NFTs
{
    public override string KeyId { get => $"Chr_{LocalID}"; set => base.KeyId = value; }

    public string Skill { get; set; } 

    public string PassiveSkill { get; set; } 
}
