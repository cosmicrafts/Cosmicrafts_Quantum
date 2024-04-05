
using UnityEngine;

[CreateAssetMenu(fileName = "New League", menuName = "League")]
public class LeagueSO : ScriptableObject
{
    public string leagueName;
    public int minElo;
    public int maxElo;
    public Sprite leagueSprite;
}