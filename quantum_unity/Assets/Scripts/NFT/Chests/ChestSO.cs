using UnityEngine;

[CreateAssetMenu(fileName = "NewChest", menuName = "Chest")]
public class ChestSO : ScriptableObject
{
    public int rarity;
    public string chestName;
    public Sprite icon;
    public string description;

    // Additional fields if required by the general metadata
    public int generalId;
    public string faction;
    public string className;

    public void PopulateFromGeneralMetadata(GeneralInfo general)
    {
        chestName = general.Name;
        rarity = general.Rarity;
        generalId = general.UnitId;
        faction = general.Faction;
        className = general.Class;
        // Icon and description can be set based on other fields or external resources
    }
}
