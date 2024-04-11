using UnityEngine;

[CreateAssetMenu(fileName = "NewChest", menuName = "Chest")]
public class ChestSO : ScriptableObject {
    public int rarity;
    public string chestName;
    public Sprite icon;
    public string description;
    // Add any other properties that all chests of this type should share
}
