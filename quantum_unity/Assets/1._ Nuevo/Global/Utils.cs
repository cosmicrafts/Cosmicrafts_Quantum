using System;

/* This script has some general utils functions */
public static class Utils
{
    //Reduce the number of characters of a long string (adding ... at the middle)
    public static string GetWalletIDShort(string walletId)
    {
        if (string.IsNullOrEmpty(walletId)) { return string.Empty; }

        if (walletId.Length > 8)
        {
            return $"{walletId.Substring(0, 4)}...{walletId.Substring(walletId.Length - 4, 4)}";
        } else
        {
            return walletId;
        }
    }
}
