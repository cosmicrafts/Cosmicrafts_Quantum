using System;
using System.Collections.Generic;
/*
 * Here we save the network data of the client (multiplayer)
 */
public class NetClientGamePack
{
    //Game Id (back end)
    public int GameId { get; set; }
    //Last time when the client sends or recives data
    public DateTime LastUpdate { get; set; }
    //The list of requested units from the client
    public List<NetUnitPack> UnitsRequested { get; set; }
    //Score
    public int ClientScore { get; set; }
}
