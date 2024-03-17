using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using CanisterPK.chests.Models;

public static class ChestMetadataParser
{
    public static ChestData Parse(Dictionary<string, Metadata> metadataDictionary)
    {
        var chestData = new ChestData();

        if (metadataDictionary.TryGetValue("rarity", out Metadata rarityMetadata))
        {
            chestData.Rarity = (int)rarityMetadata.AsNat(); 
        } 

        return chestData;
    }
}
