using System;
/*
 * Here we save the essential player information
 */
public enum userRol
{
    None,
    User,
    Admin
}

public class User
{
    public int Id { get; set; }

    public string WalletId { get; set; } //*

    public string NikeName { get; set; } //*

    //public string PassWord { get; set; }

    public int Avatar { get; set; } //*

    public string Token { get; set; }

    public string Email { get; set; }

    public int Rol { get; set; }

    public bool Online { get; set; }

    public string Region { get; set; }

    public string SocialId { get; set; }

    //public bool FirstGame { get; set; }

    public DateTime LastConnection { get; set; }

    public DateTime Registered { get; set; }
}
