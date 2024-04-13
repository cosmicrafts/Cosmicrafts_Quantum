
using UnityEngine;

[CreateAssetMenu(fileName = "New League", menuName = "League")]
public class LeagueSO : ScriptableObject
{
    public string leagueName;
    public string subLeagueName;
    public int minElo;
    public int maxElo;
    public Sprite leagueSprite;
}