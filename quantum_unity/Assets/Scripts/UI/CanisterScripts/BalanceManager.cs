using System;
using UnityEngine;
public class BalanceManager : MonoBehaviour
{
    public static event Action OnBalanceUpdateNeeded;

    public static void UpdateBalances()
    {
        OnBalanceUpdateNeeded?.Invoke();
    }
}
