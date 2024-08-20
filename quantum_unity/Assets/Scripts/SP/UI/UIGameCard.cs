using UnityEngine;
using UnityEngine.UI;
/*
 * This code represents a in the game card instance
 * The card has the UI refrences from the inspector, to show the Icon and energy cost NFT data
 */
public class UIGameCard : MonoBehaviour
{
    //The UI card in game

    //Index of the card (0 - 8)
    public int IdCardDeck = 0;
    //Cost of the card
    public int EnergyCost = 99;
    //Text reference for the cost
    public Text TextCost;
    //Image reference for the card icon
    public Image SpIcon;
    //Selection Icon
    public GameObject Selection;
    
    //Shows or hides the selection icon
    public void SetSelection(bool selected)
    {
        Selection.SetActive(selected);
    }
}
