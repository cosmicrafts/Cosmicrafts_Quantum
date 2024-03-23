using Candid;
using CanisterPK.BoomToken.Models;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;
using System.Collections;
using System.Linq;


public class boomtoken : MonoBehaviour
{
    public TMP_Text balanceText;
    public TMP_InputField principalInputField;
    public TMP_InputField tokenAmountInputField;
    public TMP_Text transferStatusText; 
    public Animator BoomPanel;

    private const int DECIMAL_PLACES = 6;
    void Start()
    {
        BoomPanel = GameObject.Find("BoomPanel").GetComponent<Animator>();
        FetchBalance();
    }

    private void OnEnable()
{
    BalanceManager.OnBalanceUpdateNeeded += FetchBalance;
}

private void OnDisable()
{
    BalanceManager.OnBalanceUpdateNeeded -= FetchBalance;
}

    public async void FetchBalance()
    {
        string principalId = GlobalGameData.Instance.GetUserData().WalletId;
        Debug.Log($"Fetching balance for Principal ID: {principalId}");

        try
        {
            var account = new CanisterPK.BoomToken.Models.Account(Principal.FromText(principalId), new Account.SubaccountInfo());
            var balance = await CandidApiManager.Instance.boomToken.Icrc1BalanceOf(account);

            // Trigger the balance animation with the new balance
            AnimateBalanceUpdate(balance);

            if (BoomPanel == null)
            {
                Debug.LogError("Panel Animator is not assigned.");
            }
            else
            {
                BoomPanel.Play("TokenPanelRefresh", -1, 0f);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching balance: {ex.Message}");
        }
    }

    private string FormatBalance(UnboundedUInt balance)
    {
       // string balanceString = balance.ToString().PadLeft(DECIMAL_PLACES + 1, '0'); // Add one to account for the integer part
      //  return balanceString.Substring(0, balanceString.Length - DECIMAL_PLACES);
      return balance.ToString();
    }



    private BigInteger ConvertToBigInteger(decimal value)
    {
        // Scale the decimal value to the appropriate number of decimal places and convert to BigInteger
       // return new BigInteger(value * (decimal)Math.Pow(10, DECIMAL_PLACES));
       return BigInteger.Parse(value.ToString());
    }



    // Method to start the balance update animation
    private void AnimateBalanceUpdate(UnboundedUInt newBalance)
    {
        // Assuming you have a method to convert UnboundedUInt to BigInteger or decimal for comparison
        BigInteger newBalanceBigInt = newBalance.ToBigInteger();
        StartCoroutine(IncrementBalanceAnimation(newBalanceBigInt));
    }

    // Coroutine for animating the balance update
    private IEnumerator IncrementBalanceAnimation(BigInteger targetBalance) {
    float duration = 0.5f;
    BigInteger currentBalance;

    if (!BigInteger.TryParse(SanitizeText(balanceText.text), out currentBalance)) {
        Debug.Log("Current balance is not a valid number, defaulting to zero.");
        currentBalance = BigInteger.Zero;
    }

    float elapsed = 0f;
    while (elapsed < duration) {
        elapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsed / duration);

        BigInteger progressBalance = currentBalance + (targetBalance - currentBalance) * new BigInteger((long)(progress * 100000)) / new BigInteger(100000);

        balanceText.text = progressBalance.ToString();
        yield return null;
    }

    balanceText.text = targetBalance.ToString();
   // Debug.Log($"Final balance text: {balanceText.text}");
}

    private string SanitizeText(string text) {
    return new string(text.Where(char.IsDigit).ToArray());
}

}
