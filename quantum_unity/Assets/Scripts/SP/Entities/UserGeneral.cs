/*
 * This is a class used for unifys the basic data and progression of the player
 * Only used for multiplayer comunication
 */

using System.Collections.Generic;

public class UserGeneral : User
{
    public int Level { get; set; }

    public int Xp { get; set; }

    public int GetNextXpGoal()
    {
        return 100 * Level;
    }

    public int CharacterNFTId;

    public List<int> DeckNFTsId;
}
