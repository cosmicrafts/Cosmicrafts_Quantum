
//Unit package structure for multiplayer comunication
public class NetUnitPack
{
    public int id { get; set; }

    public string key { get; set; }

    public int team { get; set; }

    public int player_id { get; set; }

    public float pos_x { get; set; }

    public float pos_z { get; set; }

    public float rot_y { get; set; }

    public int max_hp { get; set; }

    public int hp { get; set; }

    public int max_sh { get; set; }

    public int sh { get; set; }

    public int id_target { get; set; }

    public bool is_spell { get; set; }
}
